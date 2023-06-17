package ru.ex.mobileclient.core

import ru.ex.mobileclient.databinding.ActivityMenuBinding
import ru.ex.mobileclient.models.FileModel
import ru.ex.mobileclient.models.FolderModel
import java.time.LocalDateTime

class FileFilter(private val binding: ActivityMenuBinding) {
    private val fileSizeMultipliers = arrayOf(1L, 1024L, 1048576L, 1073741824L)

    fun filterFirstFolder(folder: FolderModel): FolderModel {
        val filteredFiles = mutableListOf<FileModel>()

        val minSizeText = binding.sizeMin.text.toString()
        val maxSizeText = binding.sizeMax.text.toString()
        val selectedSizeMultiplier = fileSizeMultipliers[binding.fileSizeSpinner.selectedItemPosition]

        val minSize = if (minSizeText.isNotBlank()) minSizeText.toInt() * selectedSizeMultiplier else 0L
        val maxSize = if (maxSizeText.isNotBlank()) maxSizeText.toInt() * selectedSizeMultiplier else Long.MAX_VALUE

        for (file in folder.files!!) {
            val date = file.getDate() >= getDateFilter()
            if (file.size in minSize..maxSize && date) {
                filteredFiles.add(file)
            }
        }

        return folder.copy(files = filteredFiles)
    }

    fun filterFolder(folder: FolderModel): FolderModel {
        val filteredFirstFolder = filterFirstFolder(folder)

        val filteredFolders = mutableListOf<FolderModel>()
        for (subFolder in folder.folders!!) {
            val filteredSubFolder = filterFolder(subFolder)
            if (filteredSubFolder.files!!.isNotEmpty() || filteredSubFolder.folders!!.isNotEmpty()) {
                filteredFolders.add(filteredSubFolder)
            }
        }

        return filteredFirstFolder.copy(folders = filteredFolders)
    }

    private fun getDateFilter(): LocalDateTime {
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

        return date
    }
}