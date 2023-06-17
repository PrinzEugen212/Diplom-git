package ru.ex.mobileclient.models

import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

@kotlinx.serialization.Serializable
data class FileModel(
    val id : Int,
    val name : String,
    val size : Long,
    val modified: String,
    val folderId : Int,
){
    fun getDate() : LocalDateTime{
        return LocalDateTime.parse(modified, DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"))
    }
}
