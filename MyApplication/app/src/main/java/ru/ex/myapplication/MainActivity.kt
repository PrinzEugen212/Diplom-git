package ru.ex.myapplication

import android.os.Bundle
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import okhttp3.*
import java.io.IOException
import java.net.URL


class MainActivity : AppCompatActivity() {
    private var resp: String = "not working"
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        val textView = findViewById<TextView>(R.id.text)
        //StrictMode.setThreadPolicy(StrictMode.ThreadPolicy.Builder().detectAll().penaltyLog().build()) // - плохо

        val gfgThread = Thread {
            try {
                resp = get()
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }

        gfgThread.start()
        gfgThread.join()

        textView.text = resp
    }

    fun print(value: String){
        val textView = findViewById<TextView>(R.id.text)
        textView.text = value
    }

    fun get() : String {
        val client = OkHttpClient()
        val url = URL("https://api.github.com/users/hadley/orgs")

        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        val response = client.newCall(request).execute()
        val responseBody = response.body!!.string()
        //Response
        return responseBody
    }

    fun run() : String {
        val client = OkHttpClient()
        val request = Request.Builder()
            .url("http://publicobject.com/helloworld.txt")
            .build()
        var res = "not working"
        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                e.printStackTrace()
            }

            override fun onResponse(call: Call, response: Response) {
                response.use {
                    if (!response.isSuccessful) throw IOException("Unexpected code $response")
                    for ((name, value) in response.headers) {
                        println("$name: $value")
                    }
                    res = response.body!!.string()
                }
            }
        })
        return res
    }
}