namespace PcClient.Controls
{
    partial class FolderControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderControl));
            lDate = new Label();
            pbIcon = new PictureBox();
            tbName = new TextBox();
            pbPublic = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbPublic).BeginInit();
            SuspendLayout();
            // 
            // lDate
            // 
            lDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lDate.AutoSize = true;
            lDate.Location = new Point(347, 52);
            lDate.Name = "lDate";
            lDate.Size = new Size(41, 20);
            lDate.TabIndex = 7;
            lDate.Text = "Дата";
            // 
            // pbIcon
            // 
            pbIcon.Image = (Image)resources.GetObject("pbIcon.Image");
            pbIcon.Location = new Point(3, 3);
            pbIcon.Name = "pbIcon";
            pbIcon.Size = new Size(64, 65);
            pbIcon.TabIndex = 5;
            pbIcon.TabStop = false;
            // 
            // tbName
            // 
            tbName.BorderStyle = BorderStyle.None;
            tbName.Cursor = Cursors.IBeam;
            tbName.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            tbName.Location = new Point(73, 22);
            tbName.Name = "tbName";
            tbName.Size = new Size(170, 31);
            tbName.TabIndex = 8;
            tbName.KeyDown += tbName_KeyDown;
            tbName.KeyPress += tbName_KeyPress;
            // 
            // pbPublic
            // 
            pbPublic.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pbPublic.InitialImage = (Image)resources.GetObject("pbPublic.InitialImage");
            pbPublic.Location = new Point(383, 3);
            pbPublic.Name = "pbPublic";
            pbPublic.Size = new Size(34, 34);
            pbPublic.TabIndex = 9;
            pbPublic.TabStop = false;
            // 
            // FolderControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pbPublic);
            Controls.Add(tbName);
            Controls.Add(lDate);
            Controls.Add(pbIcon);
            Name = "FolderControl";
            Size = new Size(420, 75);
            Click += FolderControl_Click;
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbPublic).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lDate;
        private PictureBox pbIcon;
        private TextBox tbName;
        private PictureBox pbPublic;
    }
}
