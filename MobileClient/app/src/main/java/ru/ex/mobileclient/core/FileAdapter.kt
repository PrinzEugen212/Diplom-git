package ru.ex.mobileclient.core

import android.content.Context
import android.content.Intent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.RelativeLayout
import androidx.recyclerview.widget.RecyclerView
import ru.ex.mobileclient.R
import ru.ex.mobileclient.databinding.FileCardBinding
import ru.ex.mobileclient.models.FileModel

class FileAdapter(private val context: Context) :
    RecyclerView.Adapter<FileAdapter.FileHolder>() {
    private var listFileModels: ArrayList<FileModel> = arrayListOf()

    class FileHolder(val view: View) : RecyclerView.ViewHolder(view) {
        private val binding = FileCardBinding.bind(view)

        fun bind(fileModel: FileModel) {
            binding.fileName.text = fileModel.name
            binding.fileSize.text = fileModel.size.toString() + "KB"
            binding.modified.text = fileModel.modified
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): FileHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.file_card, parent, false)
        return FileHolder(view)
    }

    override fun onBindViewHolder(holder: FileHolder, position: Int) {
        holder.bind(listFileModels[position])

        holder.view.findViewById<RelativeLayout>(R.id.card).setOnClickListener {
            val intent = Intent(context, FileActivity::class.java)
            intent.putExtra("id", listFileModels[position].id)
            context.startActivity(intent)
        }
    }

    override fun getItemCount(): Int {
        return listFileModels.size
    }

    fun setListFiles(listFileModels: ArrayList<FileModel>) {
        this.listFileModels = ArrayList(listFileModels)
        notifyDataSetChanged()
    }
}