namespace PcClient.Controls
{
    partial class FileControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileControl));
            lName = new Label();
            pbIcon = new PictureBox();
            lSize = new Label();
            lDate = new Label();
            pProgress = new Panel();
            bDownload = new Button();
            pbPublic = new PictureBox();
            ttFile = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbPublic).BeginInit();
            SuspendLayout();
            // 
            // lName
            // 
            lName.AutoSize = true;
            lName.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            lName.Location = new Point(65, 17);
            lName.Name = "lName";
            lName.Size = new Size(68, 31);
            lName.TabIndex = 0;
            lName.Text = "Файл";
            // 
            // pbIcon
            // 
            pbIcon.Image = (Image)resources.GetObject("pbIcon.Image");
            pbIcon.Location = new Point(3, 3);
            pbIcon.Name = "pbIcon";
            pbIcon.Size = new Size(60, 65);
            pbIcon.TabIndex = 1;
            pbIcon.TabStop = false;
            // 
            // lSize
            // 
            lSize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lSize.AutoSize = true;
            lSize.Location = new Point(432, 35);
            lSize.Name = "lSize";
            lSize.Size = new Size(60, 20);
            lSize.TabIndex = 2;
            lSize.Text = "Размер";
            // 
            // lDate
            // 
            lDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lDate.AutoSize = true;
            lDate.Location = new Point(451, 55);
            lDate.Name = "lDate";
            lDate.Size = new Size(41, 20);
            lDate.TabIndex = 3;
            lDate.Text = "Дата";
            // 
            // pProgress
            // 
            pProgress.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pProgress.BackColor = Color.LawnGreen;
            pProgress.Location = new Point(0, 0);
            pProgress.Name = "pProgress";
            pProgress.Size = new Size(101, 75);
            pProgress.TabIndex = 7;
            // 
            // bDownload
            // 
            bDownload.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            bDownload.Enabled = false;
            bDownload.Location = new Point(139, 6);
            bDownload.Name = "bDownload";
            bDownload.Size = new Size(90, 60);
            bDownload.TabIndex = 4;
            bDownload.Text = "Скачать";
            bDownload.UseVisualStyleBackColor = true;
            bDownload.Visible = false;
            bDownload.Resize += FileControl_Resize;
            // 
            // pbPublic
            // 
            pbPublic.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pbPublic.InitialImage = (Image)resources.GetObject("pbPublic.InitialImage");
            pbPublic.Location = new Point(481, 3);
            pbPublic.Name = "pbPublic";
            pbPublic.Size = new Size(34, 34);
            pbPublic.TabIndex = 8;
            pbPublic.TabStop = false;
            // 
            // ttFile
            // 
            ttFile.ToolTipTitle = "Путь";
            // 
            // FileControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            Controls.Add(pbPublic);
            Controls.Add(bDownload);
            Controls.Add(lDate);
            Controls.Add(lSize);
            Controls.Add(pbIcon);
            Controls.Add(lName);
            Controls.Add(pProgress);
            Name = "FileControl";
            Size = new Size(516, 75);
            Click += FileControl_Click;
            MouseEnter += FileControl_MouseEnter;
            Resize += FileControl_Resize;
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbPublic).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lName;
        private PictureBox pbIcon;
        private Label lSize;
        private Label lDate;
        private Button bDownload;
        public Panel pProgress;
        private PictureBox pbPublic;
        private ToolTip ttFile;
    }
}
