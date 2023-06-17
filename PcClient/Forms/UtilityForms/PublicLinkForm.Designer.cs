namespace PcClient.Forms
{
    partial class PublicLinkForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbLink = new TextBox();
            bCopy = new Button();
            bCreate = new Button();
            tbCount = new TextBox();
            label1 = new Label();
            bDelete = new Button();
            SuspendLayout();
            // 
            // tbLink
            // 
            tbLink.BorderStyle = BorderStyle.None;
            tbLink.Location = new Point(12, 12);
            tbLink.Name = "tbLink";
            tbLink.ReadOnly = true;
            tbLink.Size = new Size(484, 20);
            tbLink.TabIndex = 0;
            // 
            // bCopy
            // 
            bCopy.Location = new Point(12, 109);
            bCopy.Name = "bCopy";
            bCopy.Size = new Size(226, 29);
            bCopy.TabIndex = 1;
            bCopy.Text = "Скопировать в буфер обмена";
            bCopy.UseVisualStyleBackColor = true;
            bCopy.Click += bCopy_Click;
            // 
            // bCreate
            // 
            bCreate.Location = new Point(393, 109);
            bCreate.Name = "bCreate";
            bCreate.Size = new Size(103, 29);
            bCreate.TabIndex = 2;
            bCreate.Text = "Создать";
            bCreate.UseVisualStyleBackColor = true;
            bCreate.Click += bCreate_Click;
            // 
            // tbCount
            // 
            tbCount.Location = new Point(197, 50);
            tbCount.Name = "tbCount";
            tbCount.Size = new Size(125, 27);
            tbCount.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 57);
            label1.Name = "label1";
            label1.Size = new Size(179, 20);
            label1.TabIndex = 4;
            label1.Text = "Количество скачиваний:";
            // 
            // bDelete
            // 
            bDelete.Location = new Point(284, 109);
            bDelete.Name = "bDelete";
            bDelete.Size = new Size(103, 29);
            bDelete.TabIndex = 5;
            bDelete.Text = "Удалить";
            bDelete.UseVisualStyleBackColor = true;
            bDelete.Click += bDelete_Click;
            // 
            // PublicLinkForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(508, 150);
            Controls.Add(bDelete);
            Controls.Add(label1);
            Controls.Add(tbCount);
            Controls.Add(bCreate);
            Controls.Add(bCopy);
            Controls.Add(tbLink);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "PublicLinkForm";
            Text = "PublicLinkForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbLink;
        private Button bCopy;
        private Button bCreate;
        private TextBox tbCount;
        private Label label1;
        private Button bDelete;
    }
}