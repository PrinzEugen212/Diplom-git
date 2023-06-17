namespace PcClient.Forms
{
    partial class EnterNameForm
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
            bOk = new Button();
            tbName = new TextBox();
            bCancel = new Button();
            SuspendLayout();
            // 
            // bOk
            // 
            bOk.Location = new Point(12, 54);
            bOk.Name = "bOk";
            bOk.Size = new Size(94, 29);
            bOk.TabIndex = 0;
            bOk.Text = "Ок";
            bOk.UseVisualStyleBackColor = true;
            bOk.Click += bOk_Click;
            // 
            // tbName
            // 
            tbName.Location = new Point(12, 12);
            tbName.Name = "tbName";
            tbName.Size = new Size(340, 27);
            tbName.TabIndex = 1;
            // 
            // bCancel
            // 
            bCancel.Location = new Point(258, 54);
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(94, 29);
            bCancel.TabIndex = 2;
            bCancel.Text = "Отмена";
            bCancel.UseVisualStyleBackColor = true;
            bCancel.Click += bCancel_Click;
            // 
            // EnterNameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(364, 90);
            Controls.Add(bCancel);
            Controls.Add(tbName);
            Controls.Add(bOk);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EnterNameForm";
            Text = "EnterNameForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bOk;
        private TextBox tbName;
        private Button bCancel;
    }
}