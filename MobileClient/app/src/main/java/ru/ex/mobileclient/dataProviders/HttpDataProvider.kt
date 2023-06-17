package ru.ex.mobileclient.dataProviders

import android.content.Context
import android.content.SharedPreferences
import android.webkit.MimeTypeMap
import kotlinx.serialization.decodeFromString
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json
import okhttp3.*
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.RequestBody.Companion.asRequestBody
import okhttp3.RequestBody.Companion.toRequestBody
import ru.ex.mobileclient.models.FileModel
import ru.ex.mobileclient.models.FolderModel
import ru.ex.mobileclient.models.PublicLink
import ru.ex.mobileclient.models.User
import java.io.File
import java.io.InputStream
import java.net.HttpURLConnection
import java.net.URL


class HttpDataProvider(private val context: Context) : IDataProvider {
    val address = "http://192.168.1.96:7077/public/"
    private val client = OkHttpClient()
    private var baseUrl = "http://10.0.2.2:7077"
    private val jsonMediaType = "application/json; charset=utf-8".toMediaTypeOrNull()
    private val emptyBody = ByteArray(0).toRequestBody(null, 0, 0)

    override fun getRootFolder(userID: String): FolderModel {
        checkHostChange()
        val url = URL("$baseUrl/api/Folders/root/$userID")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val responseString = execute(request).body!!.string()
        val jsonConfig = Json {
            ignoreUnknownKeys = true
        }
        return jsonConfig.decodeFromString(responseString)
    }

    override fun authorize(login: String, password: String): User? {
        checkHostChange()
        val url = URL("$baseUrl/api/Authorization/authorize?login=$login&password=$password")

        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val response = execute(request)

        if (response.code == HttpURLConnection.HTTP_NO_CONTENT) {
            return null
        }

        val responseString = response.body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun register(user: User): Boolean {
        checkHostChange()
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
        checkHostChange()
        val url = URL("$baseUrl/api/Authorization/login_exists?login=$login")

        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val response = execute(request)
        return response.body!!.string().toBoolean()
    }

    override fun isEmailExists(email: String): Boolean {
        checkHostChange()
        val url = URL("$baseUrl/api/Authorization/email_exists?email=$email")

        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val response = execute(request)
        return response.body!!.string().toBoolean()
    }

    override fun getUser(id: Int): User {
        checkHostChange()
        val url = URL("$baseUrl/api/Users/$id")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val responseString = execute(request).body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun changeUser(user: User): Boolean {
        checkHostChange()
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
        checkHostChange()
        val url = URL("$baseUrl/api/Files/user_files/$userId")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val responseString = execute(request).body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun getFileModel(id: Int): FileModel {
        checkHostChange()
        val url = URL("$baseUrl/api/Files/$id")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val responseString = execute(request).body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun getFile(id: Int): InputStream {
        checkHostChange()
        val url = URL("$baseUrl/api/Files/download/$id")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        return execute(request).body!!.byteStream()
    }

    override fun deleteFile(id: Int): Boolean {
        checkHostChange()
        val url = URL("$baseUrl/api/Files/$id")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        return execute(request).isSuccessful
    }

    override fun addFile(file: File, userId: Int): Boolean {
        checkHostChange()
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
        checkHostChange()
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

    override fun addFolder(name: String, parentFolderID: Int): FolderModel {
        checkHostChange()
        val url = URL("$baseUrl/api/Folders?name=$name&parentFolderId=$parentFolderID")

        val request = Request.Builder()
            .url(url)
            .post(emptyBody)
            .build()
        val responseString = execute(request).body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun changeFolder(folderID: Int, name: String): Boolean {
        checkHostChange()
        val url = URL("$baseUrl/api/Folders/$folderID?name=$name")

        val request = Request.Builder()
            .url(url)
            .post(emptyBody)
            .build()
        return execute(request).isSuccessful
    }

    override fun deleteFolder(folderID: Int): Boolean {
        checkHostChange()
        val url = URL("$baseUrl/api/Folders/$folderID")
        val request = Request.Builder()
            .url(url)
            .delete()
            .build()
        return execute(request).isSuccessful
    }

    override fun getLink(contendID: Int, isFile: Boolean): PublicLink? {
        checkHostChange()
        val url = URL("$baseUrl/public?contentID=$contendID&isFile=$isFile")
        val request = Request.Builder()
            .url(url)
            .get()
            .build()

        val response = execute(request)
        if (response.code == 404){
            return null
        }

        val responseString = response.body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun createLink(contendID: Int, isFile: Boolean, downloadCount: Int): PublicLink {
        checkHostChange()
        val url = URL("$baseUrl/public?contentID=$contendID&isFile=$isFile&downloadCount=$downloadCount")

        val request = Request.Builder()
            .url(url)
            .post(emptyBody)
            .build()
        val responseString = execute(request).body!!.string()
        return Json.decodeFromString(responseString)
    }

    override fun deleteLink(contendID: Int, isFile: Boolean): Boolean {
        checkHostChange()
        val url = URL("$baseUrl/public?contentID=$contendID&isFile=$isFile")
        val request = Request.Builder()
            .url(url)
            .delete()
            .build()
        return execute(request).isSuccessful
    }

    fun putCurrentHost(){
        val sharedPreferences = context.getSharedPreferences("MyPrefs", Context.MODE_PRIVATE)
        val editor = sharedPreferences.edit()
        editor.putString("host", baseUrl)
        editor.apply()
    }

    private fun checkHostChange(){
        val sharedPreferences = context.getSharedPreferences("MyPrefs", Context.MODE_PRIVATE)
        val host = sharedPreferences.getString("host", "")
        if (host!!.isNotEmpty() && host != baseUrl){
            this.baseUrl = host
        }
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