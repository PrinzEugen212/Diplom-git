package ru.ex.mobileclient.models

import com.fasterxml.jackson.annotation.JsonIgnoreProperties

@kotlinx.serialization.Serializable
@JsonIgnoreProperties(ignoreUnknown = true)
data class PublicLink(
    val id: String,
    val isFile: Boolean,
    val contentID: Int,
    val isDownloadCountRestricted: Boolean,
    var downloadCount: Int
)