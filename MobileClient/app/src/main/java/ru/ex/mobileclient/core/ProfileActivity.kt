package ru.ex.mobileclient.core

import android.graphics.Color
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.MenuItem
import android.view.View
import androidx.core.widget.doOnTextChanged
import androidx.lifecycle.MediatorLiveData
import androidx.lifecycle.MutableLiveData
import ru.ex.mobileclient.dataProviders.HttpDataProvider
import ru.ex.mobileclient.databinding.ActivityProfileBinding
import ru.ex.mobileclient.models.User

class ProfileActivity : AppCompatActivity() {
    private val dataProvider = HttpDataProvider(this)
    private lateinit var binding: ActivityProfileBinding
    private var id: Int = 0
    private var user: User? = null

    private val loginLiveData = MutableLiveData<String>()
    private val passwordLiveData = MutableLiveData<String>()
    private val emailLiveData = MutableLiveData<String>()
    private val isValidLiveData = MediatorLiveData<Boolean>().apply {
        this.value = false
        addSource(loginLiveData) { login ->
            val password = passwordLiveData.value
            val email = emailLiveData.value
            this.value = validateForm(login, password, email)
        }

        addSource(emailLiveData) { email ->
            val login = loginLiveData.value
            val password = passwordLiveData.value
            this.value = validateForm(login, password, email)
        }

        addSource(passwordLiveData) { password ->
            val login = loginLiveData.value
            val email = emailLiveData.value
            this.value = validateForm(login, password, email)
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityProfileBinding.inflate(layoutInflater)
        setContentView(binding.root)

        id = intent.getIntExtra("id", 0)
        user = dataProvider.getUser(id)

        supportActionBar?.setDisplayHomeAsUpEnabled(true)
        supportActionBar?.setDisplayShowHomeEnabled(true)

        val changeButton = binding.buttonChange
        val loginLayout = binding.loginLayout
        val passLayout = binding.passwordLayout
        val emailLayout = binding.emailLayout
        binding.etLogin.setText(user!!.login)
        binding.etEmail.setText(user!!.email)
        emailLiveData.value = user!!.email
        loginLiveData.value = user!!.login

        loginLayout.editText?.doOnTextChanged { text, _, _, _ ->
            loginLiveData.value = text?.toString()
        }
        passLayout.editText?.doOnTextChanged { text, _, _, _ ->
            passwordLiveData.value = text?.toString()
        }
        emailLayout.editText?.doOnTextChanged { text, _, _, _ ->
            emailLiveData.value = text?.toString()
        }
        isValidLiveData.observe(this) { isValid ->
            changeButton.isEnabled = isValid
        }

        changeButton.setOnClickListener {
            changeUser(
                user!!.id,
                loginLiveData.value.toString(),
                passwordLiveData.value.toString(),
                emailLiveData.value.toString()
            )
        }
    }

    private fun validateForm(login: String?, password: String?, email: String?): Boolean {
        val isValidLogin = login != null && login.isNotBlank()
        val isValidPassword = password != null && password.isNotBlank() && password.length >= 6
        val isValidEmail = email != null && email.isNotBlank() && email.contains("@")
        return isValidLogin && isValidPassword && isValidEmail
    }

    private fun changeUser(id: Int, login: String?, password: String?, email: String?) {
        hideError()
        val user = User(login.toString(), password.toString(), email.toString(), id)
        if (dataProvider.changeUser(user)) {
            finish()
            return
        }

        displayError("Данные логин или почта уже используются другим пользователем")
    }

    private fun displayError(text: String) {
        binding.tvError.setTextColor(Color.RED)
        binding.tvError.visibility = View.VISIBLE
        binding.tvError.text = "Ошибка: $text"
    }

    private fun hideError() {
        binding.tvError.visibility = View.INVISIBLE
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        if (item.itemId == android.R.id.home) {
            finish()
        }
        return super.onOptionsItemSelected(item)
    }
}