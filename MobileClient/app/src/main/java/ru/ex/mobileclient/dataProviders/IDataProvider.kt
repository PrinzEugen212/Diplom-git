package ru.ex.mobileclient.dataProviders

import ru.ex.mobileclient.models.FileModel
import ru.ex.mobileclient.models.User
import java.io.File
import java.io.InputStream

interface IDataProvider {
    fun authorize(login: String, password: String) : User?
    fun register(user: User) : Boolean
    fun isLoginExists(login: String) : Boolean
    fun isEmailExists(email: String) : Boolean
    fun getUser(id: Int) : User
    fun changeUser(user: User) : Boolean
    fun getFiles(userId : Int) : List<FileModel>
    fun getFileModel(id: Int) : FileModel
    fun getFile(id: Int) : InputStream
    fun deleteFile(id: Int) : Boolean
    fun addFile(file: File, userId: Int) : Boolean
    fun changeFile(file: File, userId: Int, fileId: Int) : Boolean
}