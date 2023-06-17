package ru.ex.mobileclient.core

import android.Manifest
import android.content.Intent
import android.content.pm.PackageManager
import android.icu.text.CaseMap.Fold
import android.net.Uri
import android.os.Bundle
import android.text.InputType
import android.view.LayoutInflater
import android.view.Menu
import android.view.MenuItem
import android.view.View
import android.widget.ArrayAdapter
import android.widget.EditText
import android.widget.SearchView
import android.widget.Toast
import androidx.activity.OnBackPressedCallback
import androidx.appcompat.app.ActionBarDrawerToggle
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.recyclerview.widget.LinearLayoutManager
import ru.ex.mobileclient.R
import ru.ex.mobileclient.dataProviders.HttpDataProvider
import ru.ex.mobileclient.databinding.ActivityMenuBinding
import ru.ex.mobileclient.models.FolderModel
import java.io.File
import java.time.LocalDateTime
import java.util.*


class MenuActivity : AppCompatActivity() {
    private val requestCodePermission = 1000
    private val resultCodeFileChooser = 2000


    private var dataProvider = HttpDataProvider(this)
    private lateinit var adapter: FileAdapter
    private lateinit var binding: ActivityMenuBinding
    private lateinit var toggle: ActionBarDrawerToggle
    private lateinit var rootFolder: FolderModel
    var id: Int = 0

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMenuBinding.inflate(layoutInflater)
        setContentView(binding.root)

        id = intent.getIntExtra("id", 0)
        rootFolder = dataProvider.getRootFolder(id.toString())

        binding.recyclerView.layoutManager = LinearLayoutManager(this)
        adapter = FileAdapter(this)
        binding.recyclerView.adapter = adapter
        adapter.setRootFolder(rootFolder)

        binding.buttonAddFile.setOnClickListener {
            binding.buttonAddFile.visibility =  View.INVISIBLE
            binding.layoutFileFolder.visibility = View.VISIBLE
        }
        binding.btnCloseFilter.setOnClickListener {
            binding.buttonAddFile.visibility = View.VISIBLE
            binding.layoutFilter.visibility = View.INVISIBLE
        }
        binding.btnApplyFilter.setOnClickListener {
            filterFiles()
        }

        binding.btnAddFile.setOnClickListener{
            askPermissionAndBrowseFile()
        }

        val dialogBuilder = AlertDialog.Builder(this)
        val dialogView = LayoutInflater.from(this).inflate(R.layout.dialog_rename_folder, null)
        dialogBuilder.setView(dialogView)
        binding.btnAddFolder.setOnClickListener{
            dialogBuilder.setTitle("Создать папку")
                .setPositiveButton("Создать") { _, _ ->
                    val editTextName = dialogView.findViewById<EditText>(R.id.editTextName)
                    val newName = editTextName.text.toString()
                    //TODO send to server
                    val newFolder = dataProvider.addFolder(newName, adapter.getCurrent().id)
                    newFolder.folders = listOf()
                    newFolder.files = listOf()
                    rootFolder.folders = rootFolder.folders?.plus(newFolder)
                    binding.buttonAddFile.visibility = View.VISIBLE
                    binding.layoutFileFolder.visibility = View.GONE
                    adapter.setRootFolder(rootFolder)
                }
                .setNegativeButton("Отмена") { dialog, _ ->
                    dialog.dismiss()
                    binding.buttonAddFile.visibility = View.VISIBLE
                    binding.layoutFileFolder.visibility = View.GONE
                }

            val dialog = dialogBuilder.create()
            dialog.show()
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

        val fileSizeOptions = arrayOf("B", "KB", "MB", "GB")
        val adapter = ArrayAdapter(this, android.R.layout.simple_spinner_item, fileSizeOptions)
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        binding.fileSizeSpinner.adapter = adapter

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
        val fileFilter = FileFilter(binding)
        val resultingFolder = fileFilter.filterFolder(rootFolder)
        adapter.setRootFolder(resultingFolder)
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
                    val new = p0?.let { FileUtils.SearchInNames(rootFolder, it) }
                    if (new != null) {
                        adapter.setRootFolder(new)
                    }
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
            R.id.settings ->{
                val intent = Intent(this, SettingsActivity::class.java)
                startActivity(intent)
            }
        }
    }

    private fun refresh() {
        rootFolder = dataProvider.getRootFolder(id.toString())
        adapter.setRootFolder(rootFolder)
    }

    override fun onStart() {
        super.onStart()
        val callback = object : OnBackPressedCallback(true) {
            override fun handleOnBackPressed() {
                adapter.goBack()
            }
        }
        onBackPressedDispatcher.addCallback(this, callback)
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