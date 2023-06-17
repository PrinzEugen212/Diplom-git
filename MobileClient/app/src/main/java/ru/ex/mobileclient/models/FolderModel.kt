package ru.ex.mobileclient.models

import com.fasterxml.jackson.annotation.JsonIgnoreProperties
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

@kotlinx.serialization.Serializable
@JsonIgnoreProperties(ignoreUnknown = true)
data class FolderModel(
    val id : Int,
    var name : String,
    val modified: String,
    val userId : Int,
    val isRoot: Boolean,
    var files: List<FileModel>?,
    var folders: List<FolderModel>?,
    var path: String
){
    fun getDate() : LocalDateTime {
        return LocalDateTime.parse(modified, DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"))
    }
}