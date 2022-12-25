package ru.ex.mobileclient.models

import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

@kotlinx.serialization.Serializable
data class FileModel(
    val name : String,
    val size : Int,
    val id : Int,
    val userId : Int,
    val modified: String
){
    fun getDate() : LocalDateTime{
        return LocalDateTime.parse(modified, DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"))
    }
}
