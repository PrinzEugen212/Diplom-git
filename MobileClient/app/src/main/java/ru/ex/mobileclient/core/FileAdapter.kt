package ru.ex.mobileclient.core

import android.app.Dialog
import android.content.ClipData
import android.content.ClipboardManager
import android.content.Context
import android.content.Intent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.ImageView
import android.widget.RelativeLayout
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.widget.PopupMenu
import androidx.recyclerview.widget.RecyclerView
import ru.ex.mobileclient.R
import ru.ex.mobileclient.dataProviders.HttpDataProvider
import ru.ex.mobileclient.databinding.FileCardBinding
import ru.ex.mobileclient.databinding.FolderCardBinding
import ru.ex.mobileclient.models.FileModel
import ru.ex.mobileclient.models.FolderModel
import ru.ex.mobileclient.models.PublicLink
import java.util.Stack

class FileAdapter(private val context: Context) :
    RecyclerView.Adapter<RecyclerView.ViewHolder>() {

    private var dataProvider = HttpDataProvider(context)
    private var address = dataProvider.address
    private val items: MutableList<FileItem> = mutableListOf()
    private val folderStack: Stack<FolderModel> = Stack()
    private lateinit var currentFolder: FolderModel

    sealed class FileItem {
        data class File(val fileModel: FileModel) : FileItem()
        data class Folder(val folderModel: FolderModel) : FileItem()
    }

    class FileHolder(val view: View) : RecyclerView.ViewHolder(view) {
        private val binding = FileCardBinding.bind(view)

        fun bind(fileModel: FileModel) {
            binding.fileName.text = fileModel.name
            binding.fileSize.text = FileUtils.FileSizeToString(fileModel.size)
            binding.modified.text = fileModel.modified
        }
    }

    class FolderHolder(val view: View) : RecyclerView.ViewHolder(view) {
        private val binding = FolderCardBinding.bind(view)

        fun bind(folderModel: FolderModel) {
            binding.folderName.text = folderModel.name
            binding.modified.text = folderModel.modified
            binding.fileSize.text = FileUtils.FileSizeToString(FileUtils.GetFolderSize(folderModel))
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        return when (viewType) {
            R.layout.file_card -> {
                val view =
                    LayoutInflater.from(parent.context).inflate(R.layout.file_card, parent, false)
                FileHolder(view)
            }

            R.layout.folder_card -> {
                val view =
                    LayoutInflater.from(parent.context).inflate(R.layout.folder_card, parent, false)
                FolderHolder(view)
            }

            else -> throw IllegalArgumentException("Invalid viewType: $viewType")
        }
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int) {
        when (val item = items[position]) {
            is FileItem.File -> {
                val fileHolder = holder as FileHolder
                fileHolder.bind(item.fileModel)
                fileHolder.view.findViewById<RelativeLayout>(R.id.card).setOnClickListener {
                    val intent = Intent(context, FileActivity::class.java)
                    intent.putExtra("id", item.fileModel.id)
                    intent.putExtra("userId", currentFolder.userId)
                    context.startActivity(intent)
                }
            }

            is FileItem.Folder -> {
                val folderHolder = holder as FolderHolder
                folderHolder.bind(item.folderModel)
                val folderCard = folderHolder.view.findViewById<RelativeLayout>(R.id.card)

                folderHolder.view.findViewById<RelativeLayout>(R.id.card).setOnClickListener {
                    val clickedFolder = item.folderModel
                    folderStack.push(currentFolder)
                    currentFolder = clickedFolder
                    items.clear()
                    items.addAll(createFileItems(clickedFolder))
                    notifyDataSetChanged()
                }

                folderCard.setOnLongClickListener {
                    showMenu(folderCard, item.folderModel)
                    true // Return true to indicate the long click event is consumed
                }
            }

        }
    }

    private fun showMenu(view: View, folderModel: FolderModel) {
        val popupMenu = PopupMenu(context, view)
        popupMenu.inflate(R.menu.folder_menu)
        popupMenu.setOnMenuItemClickListener { menuItem ->
            when (menuItem.itemId) {
                R.id.menu_delete -> {
                    showDeleteConfirmationDialog(folderModel)
                    true
                }
                R.id.menu_rename -> {
                    showRenameDialog(folderModel)
                    true
                }
                R.id.menu_public -> {
                    showLinkDialog(folderModel)
                    true
                }
                else -> false
            }
        }
        popupMenu.show()
    }

    private fun showLinkDialog(model: FolderModel){
        val dialog = Dialog(context)
        dialog.setContentView(R.layout.dialog_public_link)
        val editTextDownloadCount = dialog.findViewById<EditText>(R.id.edit_text_download_count)
        val linkText = dialog.findViewById<TextView>(R.id.textPublicLink)
        val buttonCreateLink = dialog.findViewById<Button>(R.id.button_create_link)
        val buttonDeleteLink = dialog.findViewById<Button>(R.id.button_delete_link)
        val close = dialog.findViewById<ImageView>(R.id.image_close)
        val copy = dialog.findViewById<Button>(R.id.button_copy_link)
        var newLink : PublicLink

        val link = dataProvider.getLink(model.id, false)
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
                newLink = dataProvider.createLink(model.id,false, downloadCount)
            } else {
                newLink = dataProvider.createLink(model.id,false)
            }
            linkText.text = "$address${newLink.id}"
            if (newLink.downloadCount == -1){
                newLink.downloadCount = 0
            }
            editTextDownloadCount.setText(newLink.downloadCount.toString())
        }

        buttonDeleteLink.setOnClickListener {
            dataProvider.deleteLink(model.id, false)
            dialog.dismiss()
        }

        copy.setOnClickListener {
            val clipboard = context.getSystemService(Context.CLIPBOARD_SERVICE) as ClipboardManager
            val clip = ClipData.newPlainText("Link", linkText.text)
            clipboard.setPrimaryClip(clip)
            Toast.makeText(context, "Ссылка скопирована", Toast.LENGTH_SHORT).show()
        }

        close.setOnClickListener{
            dialog.dismiss()
        }


        dialog.show()
    }

    private fun showDeleteConfirmationDialog(folderModel: FolderModel) {
        AlertDialog.Builder(context)
            .setTitle("Удалить папку")
            .setMessage("Вы уверены что хотите удалить папку?")
            .setPositiveButton("Удалить") { _, _ ->
                currentFolder.folders?.toMutableList()?.apply {
                    remove(folderModel)
                }

                dataProvider.deleteFolder(folderModel.id)
                val folderIndex = items.indexOfFirst { it is FileItem.Folder && it.folderModel == folderModel }
                if (folderIndex != -1) {
                    items.removeAt(folderIndex)
                    notifyItemRemoved(folderIndex)
                }
            }
            .setNegativeButton("Отмена", null)
            .show()
    }

    private fun showRenameDialog(folderModel: FolderModel) {
        val dialogBuilder = AlertDialog.Builder(context)
        val dialogView = LayoutInflater.from(context).inflate(R.layout.dialog_rename_folder, null)
        dialogBuilder.setView(dialogView)

        val editTextName = dialogView.findViewById<EditText>(R.id.editTextName)
        editTextName.setText(folderModel.name)

        dialogBuilder.setTitle("Переименовать папку")
            .setPositiveButton("Переименовать") { _, _ ->
                val newName = editTextName.text.toString()
                //TODO send new name to server
                folderModel.name = newName
                notifyDataSetChanged()
            }
            .setNegativeButton("Отмена") { dialog, _ ->
                dialog.dismiss()
            }

        val dialog = dialogBuilder.create()
        dialog.show()
    }

    override fun getItemCount(): Int {
        return items.size
    }

    override fun getItemViewType(position: Int): Int {
        return when (items[position]) {
            is FileItem.File -> R.layout.file_card
            is FileItem.Folder -> R.layout.folder_card
        }
    }

    fun setRootFolder(rootFolder: FolderModel) {
        items.clear()
        items.addAll(createFileItems(rootFolder))
        currentFolder = rootFolder
        notifyDataSetChanged()
    }

    private fun createFileItems(folder: FolderModel): List<FileItem> {
        val fileItems = mutableListOf<FileItem>()

        // Add files
        for (file in folder.files!!) {
            fileItems.add(FileItem.File(file))
        }

        // Add sub-folders
        for (subFolder in folder.folders!!) {
            fileItems.add(FileItem.Folder(subFolder))
        }

        return fileItems
    }

    fun goBack() {
        if (folderStack.isNotEmpty()) {
            currentFolder = folderStack.pop()
            items.clear()
            items.addAll(createFileItems(currentFolder))
            notifyDataSetChanged()
        }
    }

    fun getCurrent() : FolderModel{
        return currentFolder
    }
}