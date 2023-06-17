namespace PcClient.Controls
{
    partial class ProfileControl
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
            lUserName = new Label();
            lEmail = new Label();
            label1 = new Label();
            lFreeSize = new Label();
            pbSizeComparsion = new ProgressBar();
            lFilesSize = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // lUserName
            // 
            lUserName.AutoSize = true;
            lUserName.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            lUserName.Location = new Point(3, 0);
            lUserName.Name = "lUserName";
            lUserName.RightToLeft = RightToLeft.Yes;
            lUserName.Size = new Size(59, 31);
            lUserName.TabIndex = 1;
            lUserName.Text = "Имя";
            lUserName.TextAlign = ContentAlignment.TopRight;
            // 
            // lEmail
            // 
            lEmail.AutoSize = true;
            lEmail.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            lEmail.Location = new Point(3, 43);
            lEmail.Name = "lEmail";
            lEmail.RightToLeft = RightToLeft.Yes;
            lEmail.Size = new Size(77, 31);
            lEmail.TabIndex = 2;
            lEmail.Text = "Почта";
            lEmail.TextAlign = ContentAlignment.TopRight;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(3, 128);
            label1.Name = "label1";
            label1.RightToLeft = RightToLeft.No;
            label1.Size = new Size(118, 31);
            label1.TabIndex = 3;
            label1.Text = "Доступно:";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // lFreeSize
            // 
            lFreeSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lFreeSize.AutoSize = true;
            lFreeSize.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            lFreeSize.Location = new Point(127, 128);
            lFreeSize.Name = "lFreeSize";
            lFreeSize.RightToLeft = RightToLeft.No;
            lFreeSize.Size = new Size(45, 31);
            lFreeSize.TabIndex = 4;
            lFreeSize.Text = "0 B";
            lFreeSize.TextAlign = ContentAlignment.TopRight;
            // 
            // pbSizeComparsion
            // 
            pbSizeComparsion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbSizeComparsion.ForeColor = Color.FromArgb(0, 192, 0);
            pbSizeComparsion.Location = new Point(3, 176);
            pbSizeComparsion.Name = "pbSizeComparsion";
            pbSizeComparsion.Size = new Size(302, 29);
            pbSizeComparsion.TabIndex = 5;
            // 
            // lFilesSize
            // 
            lFilesSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lFilesSize.AutoSize = true;
            lFilesSize.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            lFilesSize.Location = new Point(127, 97);
            lFilesSize.Name = "lFilesSize";
            lFilesSize.RightToLeft = RightToLeft.No;
            lFilesSize.Size = new Size(45, 31);
            lFilesSize.TabIndex = 7;
            lFilesSize.Text = "0 B";
            lFilesSize.TextAlign = ContentAlignment.TopRight;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(3, 97);
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.No;
            label3.Size = new Size(90, 31);
            label3.TabIndex = 6;
            label3.Text = "Занято:";
            label3.TextAlign = ContentAlignment.TopRight;
            // 
            // ProfileControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lFilesSize);
            Controls.Add(label3);
            Controls.Add(pbSizeComparsion);
            Controls.Add(lFreeSize);
            Controls.Add(label1);
            Controls.Add(lEmail);
            Controls.Add(lUserName);
            Name = "ProfileControl";
            Size = new Size(308, 208);
            Click += ProfileControl_Click;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lUserName;
        private Label lEmail;
        private Label label1;
        private Label lFreeSize;
        private ProgressBar pbSizeComparsion;
        private Label lFilesSize;
        private Label label3;
    }
}
