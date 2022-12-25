package ru.ex.mobileclient.core

import android.Manifest
import android.app.DatePickerDialog
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.opengl.Visibility
import android.os.Bundle
import android.text.InputType
import android.view.Menu
import android.view.MenuItem
import android.view.View
import android.widget.ArrayAdapter
import android.widget.EditText
import android.widget.SearchView
import android.widget.Toast
import androidx.appcompat.app.ActionBarDrawerToggle
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.recyclerview.widget.LinearLayoutManager
import ru.ex.mobileclient.R
import ru.ex.mobileclient.dataProviders.HttpDataProvider
import ru.ex.mobileclient.databinding.ActivityMenuBinding
import ru.ex.mobileclient.models.FileModel
import java.io.File
import java.time.LocalDate
import java.time.LocalDateTime
import java.util.*


class MenuActivity : AppCompatActivity() {
    private val requestCodePermission = 1000
    private val resultCodeFileChooser = 2000


    private var dataProvider = HttpDataProvider()
    private lateinit var adapter: FileAdapter
    private lateinit var binding: ActivityMenuBinding
    private lateinit var toggle: ActionBarDrawerToggle
    private lateinit var fileModels: List<FileModel>
    var id: Int = 0

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMenuBinding.inflate(layoutInflater)
        setContentView(binding.root)

        id = intent.getIntExtra("id", 0)
        fileModels = dataProvider.getFiles(id)

        binding.recyclerView.layoutManager = LinearLayoutManager(this)
        adapter = FileAdapter(this)
        binding.recyclerView.adapter = adapter
        adapter.setListFiles(ArrayList(fileModels))

        binding.buttonAddFile.setOnClickListener {
            askPermissionAndBrowseFile()
        }
        binding.btnCloseFilter.setOnClickListener {
            binding.layoutFilter.visibility = View.GONE
            binding.buttonAddFile.visibility = View.VISIBLE
        }
        binding.btnApplyFilter.setOnClickListener {
            filterFiles()
        }
        val spinner = binding.planetsSpinner
        ArrayAdapter.createFromResource(
            this,
            R.array.filter_array,
            android.R.layout.simple_spinner_item
        ).also { adapter ->
            adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
            spinner.adapter = adapter
        }

        binding.sizeMin.inputType =
            InputType.TYPE_CLASS_NUMBER
        binding.sizeMax.inputType =
            InputType.TYPE_CLASS_NUMBER

        binding.apply {
            toggle = ActionBarDrawerToggle(this@MenuActivity, drawer, R.string.open, R.string.close)
            drawer.addDrawerListener(toggle)
            toggle.syncState()

            supportActionBar?.setDisplayHomeAsUpEnabled(true)

            navView.setNavigationItemSelectedListener {
                handleMenuItem(it)
                true
            }
        }
    }

    private fun filterFiles() {
        var new = fileModels
        if (binding.sizeMin.text.isNotBlank()) {
            new = new.filter { file ->
                file.size >= binding.sizeMin.text.toString().toInt()
            }
        }

        if (binding.sizeMax.text.isNotBlank()) {
            new = new.filter { file ->
                file.size <= binding.sizeMax.text.toString().toInt()
            }
        }

        var date = LocalDateTime.now()
        when (binding.planetsSpinner.selectedItemId) {
            0L -> {
                date = date.minusDays(1)
            }
            1L -> {
                date = date.minusWeeks(1)
            }
            2L -> {
                date = date.minusMonths(1)
            }
            3L -> {
                date = date.minusYears(1)
            }
        }

        new = new.filter { file ->
            file.getDate() >= date
        }

        adapter.setListFiles(ArrayList(new))
    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        menuInflater.inflate(R.menu.search_menu, menu)

        val search = menu!!.findItem(R.id.search)
        val searchView = search!!.actionView as SearchView
        searchView.queryHint = "Название или его часть"

        searchView.setOnQueryTextListener(
            object : SearchView.OnQueryTextListener {
                override fun onQueryTextSubmit(p0: String?): Boolean {
                    return false
                }

                override fun onQueryTextChange(p0: String?): Boolean {
                    val new = fileModels.filter { file ->
                        file.name.lowercase().contains(p0!!.lowercase())
                    }
                    adapter.setListFiles(ArrayList(new))
                    return true
                }
            }
        )

        return super.onCreateOptionsMenu(menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        if (toggle.onOptionsItemSelected(item)) {
            return true
        }
        if (item.itemId == R.id.filter) {
            binding.layoutFilter.visibility = View.VISIBLE
            binding.buttonAddFile.visibility = View.INVISIBLE
            return true
        }
        return super.onOptionsItemSelected(item)
    }

    private fun handleMenuItem(item: MenuItem) {
        when (item.itemId) {
            R.id.files -> {
                refresh()
            }
            R.id.profile -> {
                val intent = Intent(this, ProfileActivity::class.java)
                intent.putExtra("id", id)
                startActivity(intent)
            }
        }
    }

    private fun refresh() {
        fileModels = dataProvider.getFiles(id)
        adapter.setListFiles(ArrayList(fileModels))
    }

    private fun askPermissionAndBrowseFile() {
        val permission = ActivityCompat.checkSelfPermission(
            this@MenuActivity,
            Manifest.permission.READ_EXTERNAL_STORAGE
        )
        if (permission != PackageManager.PERMISSION_GRANTED) {
            requestPermissions(
                arrayOf(Manifest.permission.READ_EXTERNAL_STORAGE),
                requestCodePermission
            )
            return
        }
        doBrowseFile()
    }

    private fun doBrowseFile() {
        var chooseFileIntent = Intent(Intent.ACTION_GET_CONTENT)
        chooseFileIntent.type = "*/*"
        // Only return URIs that can be opened with ContentResolver
        chooseFileIntent.addCategory(Intent.CATEGORY_OPENABLE)
        chooseFileIntent = Intent.createChooser(chooseFileIntent, "Choose a file")
        startActivityForResult(chooseFileIntent, resultCodeFileChooser)
    }

    // When you have the request results

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        when (requestCode) {
            requestCodePermission -> {
                if (grantResults.isNotEmpty()
                    && grantResults[0] == PackageManager.PERMISSION_GRANTED
                ) {
                    Toast.makeText(this@MenuActivity, "Permission granted!", Toast.LENGTH_SHORT)
                        .show()
                    doBrowseFile()
                } else {
                    Toast.makeText(this@MenuActivity, "Permission denied!", Toast.LENGTH_SHORT)
                        .show()
                }
            }
        }
    }


    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        when (requestCode) {
            resultCodeFileChooser -> if (resultCode == RESULT_OK) {
                if (data != null) {
                    val fileUri: Uri? = data.data
                    try {
                        var path = fileUri!!.path
                        if (path!!.contains("document/raw:")) {
                            path = path.replace("/document/raw:", "");
                        }
                        val file = File(path)
                        if (file.exists()) {
                            val lastModified = Date(file.lastModified())
                            Toast.makeText(
                                this@MenuActivity,
                                lastModified.toString(),
                                Toast.LENGTH_LONG
                            ).show()
                        }
                        dataProvider.addFile(file, id)
                        Toast.makeText(this@MenuActivity, "Файл загружен", Toast.LENGTH_LONG).show()
                        refresh()
                    } catch (e: Exception) {
                        Toast.makeText(this@MenuActivity, "Error: $e", Toast.LENGTH_SHORT).show()
                    }

                }
            }
        }
        super.onActivityResult(requestCode, resultCode, data)
    }
}