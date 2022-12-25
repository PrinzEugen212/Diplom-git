package ru.ex.mobileclient.models

@kotlinx.serialization.Serializable
data class User(
    val login: String,
    val password: String,
    val email: String,
    val id: Int = 0)