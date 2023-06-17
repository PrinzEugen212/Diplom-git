namespace PcClient.Controls.AuthControls
{
    partial class RegisterControl
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
            this.bRegister = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.bAuthorize = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbRepeatPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bRegister
            // 
            this.bRegister.Location = new System.Drawing.Point(142, 295);
            this.bRegister.Name = "bRegister";
            this.bRegister.Size = new System.Drawing.Size(120, 45);
            this.bRegister.TabIndex = 17;
            this.bRegister.Text = "Регистрация";
            this.bRegister.UseVisualStyleBackColor = true;
            this.bRegister.Click += new System.EventHandler(this.bRegister_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "Пароль";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Email";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(25, 144);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(374, 27);
            this.tbPassword.TabIndex = 14;
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(25, 91);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(374, 27);
            this.tbEmail.TabIndex = 13;
            // 
            // bAuthorize
            // 
            this.bAuthorize.Location = new System.Drawing.Point(108, 356);
            this.bAuthorize.Name = "bAuthorize";
            this.bAuthorize.Size = new System.Drawing.Size(192, 40);
            this.bAuthorize.TabIndex = 12;
            this.bAuthorize.Text = "Назад к странице входа";
            this.bAuthorize.UseVisualStyleBackColor = true;
            this.bAuthorize.Click += new System.EventHandler(this.bAuthorize_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 20);
            this.label3.TabIndex = 19;
            this.label3.Text = "Логин";
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(25, 38);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(374, 27);
            this.tbLogin.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 20);
            this.label4.TabIndex = 21;
            this.label4.Text = "Повторите пароль";
            // 
            // tbRepeatPassword
            // 
            this.tbRepeatPassword.Location = new System.Drawing.Point(25, 204);
            this.tbRepeatPassword.Name = "tbRepeatPassword";
            this.tbRepeatPassword.Size = new System.Drawing.Size(374, 27);
            this.tbRepeatPassword.TabIndex = 20;
            // 
            // RegisterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbRepeatPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.bRegister);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.bAuthorize);
            this.Name = "RegisterControl";
            this.Size = new System.Drawing.Size(425, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button bRegister;
        private Label label2;
        private Label label1;
        private TextBox tbPassword;
        private TextBox tbEmail;
        private Button bAuthorize;
        private Label label3;
        private TextBox tbLogin;
        private Label label4;
        private TextBox tbRepeatPassword;
    }
}
