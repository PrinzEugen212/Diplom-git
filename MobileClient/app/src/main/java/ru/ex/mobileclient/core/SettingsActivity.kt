package ru.ex.mobileclient.core

import android.content.Context
import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.MutableLiveData
import ru.ex.mobileclient.dataProviders.HttpDataProvider
import ru.ex.mobileclient.databinding.ActivityProfileBinding
import ru.ex.mobileclient.databinding.ActivitySettingsBinding
import ru.ex.mobileclient.models.User

class SettingsActivity : AppCompatActivity() {
    private val dataProvider = HttpDataProvider(this)
    private lateinit var binding: ActivitySettingsBinding

    private val hostLiveData = MutableLiveData<String>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivitySettingsBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.editTextHost.setText(hostLiveData.value)

        binding.editTextHost.addTextChangedListener(object : TextWatcher {
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}

            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {}

            override fun afterTextChanged(s: Editable?) {
                hostLiveData.value = s.toString()
            }
        })

        binding.buttonSubmit.setOnClickListener {
            val host = binding.editTextHost.text.toString()
            saveHostValue(host)
        }

        val sharedPreferences = this.getSharedPreferences("MyPrefs", Context.MODE_PRIVATE)
        val host = sharedPreferences.getString("host", "")
        if (host!!.isNotEmpty()){
            binding.editTextHost.setText(host)
        }
    }

    private fun saveHostValue(host: String) {
        val sharedPreferences = getSharedPreferences("MyPrefs", Context.MODE_PRIVATE)
        val editor = sharedPreferences.edit()
        editor.putString("host", host)
        editor.apply()

        Toast.makeText(this, "Host value saved: $host", Toast.LENGTH_SHORT).show()
    }
}