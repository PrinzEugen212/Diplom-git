package ru.ex.mobileclient.core

import android.Manifest
import android.app.Dialog
import android.content.ClipData
import android.content.ClipboardManager
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.Environment
import android.view.MenuItem
import android.widget.Button
import android.widget.EditText
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.core.app.ActivityCompat
import ru.ex.mobileclient.R
import ru.ex.mobileclient.dataProviders.HttpDataProvider
import ru.ex.mobileclient.databinding.ActivityFileBinding
import ru.ex.mobileclient.models.FileModel
import ru.ex.mobileclient.models.FolderModel
import ru.ex.mobileclient.models.PublicLink
import ru.ex.mobileclient.models.User
import java.io.File
import java.util.*

class FileActivity : AppCompatActivity() {
    private val requestCodePermission = 1000
    private val resultCodeFileChooser = 2000

    private lateinit var binding: ActivityFileBinding
    private var dataProvider = HttpDataProvider(this)
    private var address = dataProvider.address
    private lateinit var file: FileModel
    private lateinit var user: User
    var id: Int = 0
    var userId: Int = 0

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityFileBinding.inflate(layoutInflater)
        setContentView(binding.root)

        supportActionBar?.setDisplayHomeAsUpEnabled(true)
        supportActionBar?.setDisplayShowHomeEnabled(true)

        id = intent.getIntExtra("id", 0)
        userId = intent.getIntExtra("userId", 0)
        loadInfo()
        binding.buttonDownload.setOnClickListener {
            downloadFile()
        }
        binding.buttonDelete.setOnClickListener {
            deleteFile()
        }
        binding.buttonChange.setOnClickListener{
            askPermissionAndBrowseFile()
        }
        binding.buttonLink.setOnClickListener{
            showLinkDialog(file)
        }
    }

    private fun loadInfo(){
        file = dataProvider.getFileModel(id)
        user = dataProvider.getUser(userId)
        binding.tvFileName.text = file.name
        binding.tvFileSize.text = FileUtils.FileSizeToString(file.size)
        binding.tvChangeTime.text = file.modified
    }

    private fun downloadFile() {
        val publicDownloadFolder =
            Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS)
//        val downloadFolder = getExternalFilesDir(Environment.DIRECTORY_DOWNLOADS)

        File("$publicDownloadFolder/${user.login}").mkdir()
        val publicFile = File("$publicDownloadFolder/${user.login}/${file.name}")

//        File("$downloadFolder/${user.login}").mkdir()
//        val file = File("$downloadFolder/${user.login}/${file.name}")
        val stream = dataProvider.getFile(id)
        val thread = Thread {
            stream.use { input ->
                publicFile.outputStream().use { output ->
                    input.copyTo(output)
                }
//                publicFile.copyTo(publicFile, true)
            }
        }

        thread.start()
        thread.join()
//        Toast.makeText(this@FileActivity, "Файл загружен", Toast.LENGTH_LONG).show()
        println("Файл загружен")
    }

    private fun deleteFile() {
        val downloadFolder = getExternalFilesDir(Environment.DIRECTORY_DOWNLOADS)

        if (File("$downloadFolder/${user.login}/${file.name}").exists()) {
            File("$downloadFolder/${user.login}/${file.name}").delete()
        }

        if (dataProvider.deleteFile(id)) {
            Toast.makeText(this@FileActivity, "Файл удалён", Toast.LENGTH_SHORT).show()
            finish()
        }
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        if (item.itemId == android.R.id.home) {
            finish()
        }
        return super.onOptionsItemSelected(item)
    }

    private fun askPermissionAndBrowseFile() {
        val permission = ActivityCompat.checkSelfPermission(
            this@FileActivity,
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

    private fun showLinkDialog(model: FileModel){
        val dialog = Dialog(this)
        dialog.setContentView(R.layout.dialog_public_link)
        val editTextDownloadCount = dialog.findViewById<EditText>(R.id.edit_text_download_count)
        val linkText = dialog.findViewById<TextView>(R.id.textPublicLink)
        val buttonCreateLink = dialog.findViewById<Button>(R.id.button_create_link)
        val buttonDeleteLink = dialog.findViewById<Button>(R.id.button_delete_link)
        val close = dialog.findViewById<ImageView>(R.id.image_close)
        val copy = dialog.findViewById<Button>(R.id.button_copy_link)
        var newLink : PublicLink

        val link = dataProvider.getLink(model.id, true)
        if (link != null){
            linkText.text = "$address/${link.id}"
            if (link.downloadCount == -1){
                link.downloadCount = 0
            }
            editTextDownloadCount.setText(link.downloadCount.toString())
        }

        buttonCreateLink.setOnClickListener {
            val downloadCountText = editTextDownloadCount.text.toString()
            val downloadCount = if (downloadCountText.isNotBlank()) downloadCountText.toIntOrNull() else null

            if (downloadCount != null) {
                newLink = dataProvider.createLink(model.id,true, downloadCount)
            } else {
                newLink = dataProvider.createLink(model.id,true)
            }

            linkText.text = "$address${newLink.id}"
            if (newLink.downloadCount == -1){
                newLink.downloadCount = 0
            }
            editTextDownloadCount.setText(newLink.downloadCount.toString())
        }

        buttonDeleteLink.setOnClickListener {
            dataProvider.deleteLink(model.id, true)
            dialog.dismiss()
        }

        copy.setOnClickListener {
            val clipboard = this.getSystemService(Context.CLIPBOARD_SERVICE) as ClipboardManager
            val clip = ClipData.newPlainText("Link", linkText.text)
            clipboard.setPrimaryClip(clip)
            Toast.makeText(this, "Ссылка скопирована", Toast.LENGTH_SHORT).show()
        }

        close.setOnClickListener{
            dialog.dismiss()
        }


        dialog.show()
    }

    private fun doBrowseFile() {
        var chooseFileIntent = Intent(Intent.ACTION_GET_CONTENT)
        chooseFileIntent.type = "*/*"
        chooseFileIntent.addCategory(Intent.CATEGORY_OPENABLE)
        chooseFileIntent = Intent.createChooser(chooseFileIntent, "Choose a file")
        startActivityForResult(chooseFileIntent, resultCodeFileChooser)
    }

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
                    Toast.makeText(this@FileActivity, "Permission granted!", Toast.LENGTH_SHORT)
                        .show()
                    doBrowseFile()
                } else {
                    Toast.makeText(this@FileActivity, "Permission denied!", Toast.LENGTH_SHORT)
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
                        val fileToChange = File(path)
                        if (fileToChange.exists()) {
                            val lastModified = Date(fileToChange.lastModified())
                            Toast.makeText(
                                this@FileActivity,
                                lastModified.toString(),
                                Toast.LENGTH_LONG
                            ).show()
                        }

                        if (dataProvider.changeFile(fileToChange, file.folderId, id)){
                            Toast.makeText(this@FileActivity, "Файл обновлен", Toast.LENGTH_LONG).show()
                            loadInfo()
                        }
                        else{
                            Toast.makeText(this@FileActivity, "Ошибка", Toast.LENGTH_LONG).show()
                        }
                    } catch (e: Exception) {
                        Toast.makeText(this@FileActivity, "Error: $e", Toast.LENGTH_SHORT).show()
                    }

                }
            }
        }
        super.onActivityResult(requestCode, resultCode, data)
    }
}