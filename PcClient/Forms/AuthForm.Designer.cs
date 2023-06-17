namespace PcClient.Forms
{
    partial class AuthForm
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
            this.pAuth = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pAuth
            // 
            this.pAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAuth.Location = new System.Drawing.Point(0, 0);
            this.pAuth.Name = "pAuth";
            this.pAuth.Size = new System.Drawing.Size(407, 403);
            this.pAuth.TabIndex = 6;
            // 
            // AuthForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 403);
            this.Controls.Add(this.pAuth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AuthForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AuthForm";
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pAuth;
    }
}