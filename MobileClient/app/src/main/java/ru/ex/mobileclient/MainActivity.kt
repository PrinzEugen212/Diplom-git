package ru.ex.mobileclient

import android.content.Intent
import android.graphics.Color
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.text.method.LinkMovementMethod
import android.view.View
import androidx.core.widget.doOnTextChanged
import androidx.lifecycle.MediatorLiveData
import androidx.lifecycle.MutableLiveData
import ru.ex.mobileclient.auth.SignUpActivity
import ru.ex.mobileclient.core.MenuActivity
import ru.ex.mobileclient.core.SettingsActivity
import ru.ex.mobileclient.dataProviders.HttpDataProvider
import ru.ex.mobileclient.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {
    private val dataProvider = HttpDataProvider(this)
    private lateinit var binding: ActivityMainBinding


    private val loginLiveData = MutableLiveData<String>()
    private val passwordLiveData = MutableLiveData<String>()
    private val isValidLiveData = MediatorLiveData<Boolean>().apply {
        this.value = false
        addSource(loginLiveData) { login ->
            val password = passwordLiveData.value
            this.value = validateForm(login, password)
        }

        addSource(passwordLiveData) { password ->
            val login = loginLiveData.value
            this.value = validateForm(login, password)
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        //Проверка валидности вводимых данных
        val loginLayout = binding.loginLayout
        val passLayout = binding.passLayout
        val signInButton = binding.buttonSignIn
        val settingsButton = binding.buttonSettings
        loginLayout.editText?.doOnTextChanged { text, _, _, _ ->
            loginLiveData.value = text?.toString()
        }
        passLayout.editText?.doOnTextChanged { text, _, _, _ ->
            passwordLiveData.value = text?.toString()
        }
        isValidLiveData.observe(this) { isValid ->
            signInButton.isEnabled = isValid
        }

        //Текст с переходом к регистрации
        val tvSignUp = binding.tvSignUp
        tvSignUp.setOnClickListener {
            val myIntent = Intent(this, SignUpActivity::class.java)
            startActivity(myIntent)
            tvSignUp.movementMethod = LinkMovementMethod.getInstance()
        }

        //Кнопка входа
        signInButton.setOnClickListener {
            authorize(loginLiveData.value.toString(), passwordLiveData.value.toString())
        }

        //Настройки
        settingsButton.setOnClickListener {
            val intent = Intent(this, SettingsActivity::class.java)
            startActivity(intent)
        }

        dataProvider.putCurrentHost()

        loginLayout.editText?.setText("TestUser")
        passLayout.editText?.setText("TestUser")
    }

    private fun validateForm(login: String?, password: String?): Boolean {
        val isValidLogin = !login.isNullOrBlank()
        val isValidPassword = !password.isNullOrBlank() && password.length >= 6
        return isValidLogin && isValidPassword
    }

    private fun authorize(login: String, password: String) {
        hideError()
        val user = dataProvider.authorize(login, password)
        if (!dataProvider.isLoginExists(login)){
            displayError("Данного логина не существует")
            return
        }

        if (user == null){
            displayError("Неверный пароль")
            return
        }

        val intent = Intent(this, MenuActivity::class.java)
        intent.putExtra("id", user.id)
        startActivity(intent)
    }

    private fun displayError(text: String){
        binding.tvError.setTextColor(Color.RED)
        binding.tvError.visibility = View.VISIBLE
        binding.tvError.text = "Ошибка: $text"
    }

    private fun hideError(){
        binding.tvError.visibility = View.INVISIBLE
    }
}