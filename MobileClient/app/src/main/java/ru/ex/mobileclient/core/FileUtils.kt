package ru.ex.mobileclient.core

import ru.ex.mobileclient.models.FileModel
import ru.ex.mobileclient.models.FolderModel

object FileUtils {
    fun FileSizeToString(fileSize: Long): String {
        val sizes = arrayOf("B", "KB", "MB", "GB", "TB")
        var order = 0
        var doubleSize = fileSize.toDouble()
        while (doubleSize >= 1024 && order < sizes.size - 1) {
            order++
            doubleSize /= 1024
        }

        return String.format("%.1f %s", doubleSize, sizes[order])
    }

    fun SearchInNames(searchRoot: FolderModel, searchText: String): FolderModel? {
        if (searchText.isEmpty()){
            return  searchRoot
        }

        if (searchRoot.name.contains(searchText)) {
            return searchRoot.copy(folders = emptyList())
        }

        val matchingFiles = mutableListOf<FileModel>()

        searchRoot.folders?.forEach { folder ->
            val matchingSubFolder = SearchInNames(folder, searchText)
            if (matchingSubFolder != null) {
                matchingSubFolder.files?.let { matchingFiles.addAll(it) }
            }
        }

        searchRoot.files?.forEach { file ->
            if (file.name.contains(searchText)) {
                matchingFiles.add(file)
            }
        }

        return if (matchingFiles.isNotEmpty()) {
            searchRoot.copy(files = matchingFiles)
        } else {
            null
        }
    }

    fun GetFolderSize(folder: FolderModel?): Long {
        if (folder == null){
            return 0
        }

        if (folder.files!!.isEmpty() && folder.folders!!.isEmpty()){
            return 0
        }

        var size: Long = 0
        for (file in folder.files!!) {
            size += file.size
        }

        for (subFolder in folder.folders!!) {
            size += GetFolderSize(subFolder)
        }

        return size
    }

    private fun FolderModel.hasMatchingFolders(): Boolean {
        if (folders!!.isNotEmpty()) {
            return true
        }
        folders!!.forEach { folder ->
            if (folder.hasMatchingFolders()) {
                return true
            }
        }
        return false
    }
}