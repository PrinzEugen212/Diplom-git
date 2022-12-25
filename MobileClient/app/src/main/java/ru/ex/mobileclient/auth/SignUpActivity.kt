package ru.ex.mobileclient.auth

import android.graphics.Color
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import androidx.core.widget.doOnTextChanged
import androidx.lifecycle.MediatorLiveData
import androidx.lifecycle.MutableLiveData
import ru.ex.mobileclient.dataProviders.HttpDataProvider
import ru.ex.mobileclient.databinding.ActivitySignUpBinding
import ru.ex.mobileclient.models.User

class SignUpActivity : AppCompatActivity() {
    private val dataProvider = HttpDataProvider()
    private lateinit var binding: ActivitySignUpBinding

    private val loginLiveData = MutableLiveData<String>()
    private val passwordLiveData = MutableLiveData<String>()
    private val repeatPasswordLiveData = MutableLiveData<String>()
    private val emailLiveData = MutableLiveData<String>()
    private val isValidLiveData = MediatorLiveData<Boolean>().apply {
        this.value = false
        addSource(loginLiveData) { login ->
            val password = passwordLiveData.value
            val repeatPassword = repeatPasswordLiveData.value
            val email = emailLiveData.value
            this.value = validateForm(login, password, repeatPassword, email)
        }

        addSource(repeatPasswordLiveData) { repeatPassword ->
            val login = loginLiveData.value
            val password = passwordLiveData.value
            val email = emailLiveData.value
            this.value = validateForm(login, password, repeatPassword, email)
        }

        addSource(emailLiveData) { email ->
            val login = loginLiveData.value
            val password = passwordLiveData.value
            val repeatPassword = repeatPasswordLiveData.value
            this.value = validateForm(login, password, repeatPassword, email)
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivitySignUpBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val signInButton = binding.buttonSignUp
        val loginLayout = binding.loginLayout
        val passLayout = binding.passLayout
        val repeatPassLayout = binding.repeatPassLayout
        val emailLayout = binding.emailLayout

        loginLayout.editText?.doOnTextChanged { text, _, _, _ ->
            loginLiveData.value = text?.toString()
        }
        passLayout.editText?.doOnTextChanged { text, _, _, _ ->
            passwordLiveData.value = text?.toString()
        }
        repeatPassLayout.editText?.doOnTextChanged { text, _, _, _ ->
            repeatPasswordLiveData.value = text?.toString()
        }
        emailLayout.editText?.doOnTextChanged { text, _, _, _ ->
            emailLiveData.value = text?.toString()
        }
        isValidLiveData.observe(this) { isValid ->
            signInButton.isEnabled = isValid
        }

        signInButton.setOnClickListener {
            register(
                loginLiveData.value.toString(),
                passwordLiveData.value.toString(),
                emailLiveData.value
            )
        }

        binding.tvSignIn.setOnClickListener {
            finish()
        }
    }

    private fun validateForm(
        login: String?,
        password: String?,
        repeatPassword: String?,
        email: String?
    ): Boolean {
        val isValidLogin = login != null && login.isNotBlank()
        val isValidPassword =
            password != null && password.isNotBlank() && password.length >= 6 && password == repeatPassword
        val isValidEmail = email != null && email.isNotBlank() && email.contains("@")
        return isValidLogin && isValidPassword && isValidEmail
    }

    private fun register(login: String?, password: String?, email: String?) {
        if (dataProvider.isLoginExists(login.toString()) || dataProvider.isEmailExists(login.toString())) {
            displayError("Данный логин уже используется")
            return
        }

        if (dataProvider.isEmailExists(email.toString())) {
            displayError("Данная почта уже используется")
            return
        }

        val user = User(login.toString(), password.toString(), email.toString())
        if (dataProvider.register(user)) {
            hideError()
            finish()
            return
        }

        displayError("Внутрення ошибка сервиса. Попробуйте позже")
    }

    private fun displayError(text: String) {
        binding.tvError.setTextColor(Color.RED)
        binding.tvError.visibility = View.VISIBLE
        binding.tvError.text = "Ошибка: $text"
    }

    private fun hideError() {
        binding.tvError.visibility = View.INVISIBLE
    }
}