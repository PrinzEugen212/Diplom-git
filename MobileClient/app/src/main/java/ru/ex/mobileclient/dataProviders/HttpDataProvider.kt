package ru.ex.mobileclient.dataProviders

import android.webkit.MimeTypeMap
import kotlinx.serialization.decodeFromString
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json
import okhttp3.*
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.RequestBody.Companion.asRequestBody
import okhttp3.RequestBody.Companion.toRequestBody
import ru.ex.mobileclient.models.FileModel
import ru.ex.mobileclient.models.User
import java.io.File
import java.io.InputStream
import java.net.HttpURLConnection
import java.net.URL


class HttpDataProvider() : IDataProvider {
    private val client = OkHttpClient()
    private var baseUrl = "http://192.168.1.75:7077"
    private val jsonMediaType = "application/json; charset=utf-8".toMediaTypeOrNull()

    override fun authorize(login: String, password: String): User? {
        val url = URL("$baseUrl/api/Users/authorize?login=$login&password=$password")

        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val response = execute(request)
        if (!response.isSuccessful) {
            return null
        }

        if (response.code == HttpURLConnection.HTTP_NO_CONTENT) {
            return null
        }

        val responseString = response.body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun register(user: User): Boolean {
        val json = Json.encodeToString(user)
        val url = URL("$baseUrl/api/Users")

        val body = json.toRequestBody(jsonMediaType)
        val request = Request.Builder()
            .url(url)
            .post(body)
            .build()
        return execute(request).isSuccessful
    }

    override fun isLoginExists(login: String): Boolean {
        val url = URL("$baseUrl/api/Users/login_exists?login=$login")

        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val response = execute(request)
        return response.body!!.string().toBoolean()
    }

    override fun isEmailExists(email: String): Boolean {
        val url = URL("$baseUrl/api/Users/email_exists?email=$email")

        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val response = execute(request)
        return response.body!!.string().toBoolean()
    }

    override fun getUser(id: Int): User {
        val url = URL("$baseUrl/api/Users/$id")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val responseString = execute(request).body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun changeUser(user: User): Boolean {
        val json = Json.encodeToString(user)
        val url = URL("$baseUrl/api/Users/${user.id}")

        val body = json.toRequestBody(jsonMediaType)
        val request = Request.Builder()
            .url(url)
            .put(body)
            .build()
        return execute(request).isSuccessful
    }

    override fun getFiles(userId: Int): List<FileModel> {
        val url = URL("$baseUrl/api/Files/user_files/$userId")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val responseString = execute(request).body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun getFileModel(id: Int): FileModel {
        val url = URL("$baseUrl/api/Files/$id")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val responseString = execute(request).body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun getFile(id: Int): InputStream {
        val url = URL("$baseUrl/api/Files/download/$id")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        return execute(request).body!!.byteStream()
    }

    override fun deleteFile(id: Int): Boolean {
        val url = URL("$baseUrl/api/Files/$id")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        return execute(request).isSuccessful
    }

    override fun addFile(file: File, userId: Int): Boolean {
        val url = URL("$baseUrl/api/Files?userId=$userId")
        val mimeType = getMimeType(file)
        val requestBody: RequestBody =
            MultipartBody.Builder().setType(MultipartBody.FORM)
                .addFormDataPart(
                    "uploaded_file",
                    file.name,
                    file.asRequestBody(mimeType!!.toMediaTypeOrNull())
                )
                .build()

        val request = Request.Builder()
            .url(url)
            .post(requestBody)
            .build()
        return execute(request).isSuccessful
    }

    override fun changeFile(file: File, userId: Int, fileId: Int): Boolean {
        val url = URL("$baseUrl/api/Files/$fileId?userId=$userId")
        val mimeType = getMimeType(file)
        val requestBody: RequestBody =
            MultipartBody.Builder().setType(MultipartBody.FORM)
                .addFormDataPart(
                    "uploaded_file",
                    file.name,
                    file.asRequestBody(mimeType!!.toMediaTypeOrNull())
                )
                .build()

        val request = Request.Builder()
            .url(url)
            .put(requestBody)
            .build()
        return execute(request).isSuccessful
    }

    private fun execute(request: Request): Response {
        var resp: Response? = null
        val thread = Thread {
            resp = client.newCall(request).execute()
        }

        thread.start()
        thread.join()
        return resp!!
    }

    private fun getMimeType(file: File): String? {
        var type: String? = null
        val extension = file.extension
        type = MimeTypeMap.getSingleton().getMimeTypeFromExtension(extension)
        return type
    }
}