package ru.ex.mobileclient.core

import android.Manifest
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.fragment.app.Fragment
import com.google.android.material.floatingactionbutton.FloatingActionButton
import ru.ex.mobileclient.R
import java.io.File

/**
 * A simple [Fragment] subclass.
 * Use the [FileLoader.newInstance] factory method to
 * create an instance of this fragment.
 */
class FileLoader : Fragment() {

    private val MY_REQUEST_CODE_PERMISSION = 1000
    private val MY_RESULT_CODE_FILECHOOSER = 2000
    private lateinit var file: File

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val rootView = inflater.inflate(R.layout.fragment_file_loader, container, false)
        val button = rootView.findViewById<FloatingActionButton>(R.id.button_add_file)
        button.setOnClickListener(View.OnClickListener {
            askPermissionAndBrowseFile()
        })
        return rootView
    }

    private fun askPermissionAndBrowseFile() {
        val permission = ActivityCompat.checkSelfPermission(
            this.requireContext(),
            Manifest.permission.READ_EXTERNAL_STORAGE
        )
        if (permission != PackageManager.PERMISSION_GRANTED) {
            // If don't have permission so prompt the user.
            requestPermissions(
                arrayOf(Manifest.permission.READ_EXTERNAL_STORAGE),
                MY_REQUEST_CODE_PERMISSION
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
        startActivityForResult(chooseFileIntent, MY_RESULT_CODE_FILECHOOSER)
    }

    // When you have the request results

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        when (requestCode) {
            MY_REQUEST_CODE_PERMISSION -> {
                if (grantResults.isNotEmpty()
                    && grantResults[0] == PackageManager.PERMISSION_GRANTED
                ) {
                    Toast.makeText(this.requireContext(), "Permission granted!", Toast.LENGTH_SHORT)
                        .show()
                    doBrowseFile()
                } else {
                    Toast.makeText(this.requireContext(), "Permission denied!", Toast.LENGTH_SHORT)
                        .show()
                }
            }
        }
    }


    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        when (requestCode) {
            MY_RESULT_CODE_FILECHOOSER -> if (resultCode == AppCompatActivity.RESULT_OK) {
                if (data != null) {
                    val fileUri: Uri? = data.data
                    try {
                        val file: File = File(fileUri.toString())
                        this.file = file
                        Toast.makeText(this.requireContext(), file.absolutePath, Toast.LENGTH_LONG)
                            .show()
                    } catch (e: Exception) {
                        Toast.makeText(this.requireContext(), "Error: $e", Toast.LENGTH_SHORT)
                            .show()
                    }

                }
            }
        }
        super.onActivityResult(requestCode, resultCode, data)
    }

    fun getFile(): File {
        return file
    }
}