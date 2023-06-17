namespace PcClient.Controls.AuthControls
{
    partial class AuthControl
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
            bRegister = new Button();
            label2 = new Label();
            label1 = new Label();
            tbPasswod = new TextBox();
            tbLogin = new TextBox();
            bAuthorize = new Button();
            SuspendLayout();
            // 
            // bRegister
            // 
            bRegister.Location = new Point(145, 332);
            bRegister.Name = "bRegister";
            bRegister.Size = new Size(120, 45);
            bRegister.TabIndex = 11;
            bRegister.Text = "Регистрация";
            bRegister.UseVisualStyleBackColor = true;
            bRegister.Click += bRegister_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 115);
            label2.Name = "label2";
            label2.Size = new Size(62, 20);
            label2.TabIndex = 10;
            label2.Text = "Пароль";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 62);
            label1.Name = "label1";
            label1.Size = new Size(52, 20);
            label1.TabIndex = 9;
            label1.Text = "Логин";
            // 
            // tbPasswod
            // 
            tbPasswod.Location = new Point(24, 138);
            tbPasswod.Name = "tbPasswod";
            tbPasswod.PasswordChar = '*';
            tbPasswod.Size = new Size(374, 27);
            tbPasswod.TabIndex = 8;
            tbPasswod.Text = "TestUser";
            // 
            // tbLogin
            // 
            tbLogin.Location = new Point(24, 85);
            tbLogin.Name = "tbLogin";
            tbLogin.Size = new Size(374, 27);
            tbLogin.TabIndex = 7;
            tbLogin.Text = "TestUser";
            // 
            // bAuthorize
            // 
            bAuthorize.Location = new Point(145, 266);
            bAuthorize.Name = "bAuthorize";
            bAuthorize.Size = new Size(120, 40);
            bAuthorize.TabIndex = 6;
            bAuthorize.Text = "Войти";
            bAuthorize.UseVisualStyleBackColor = true;
            bAuthorize.Click += bAuthorize_Click;
            // 
            // AuthControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(bRegister);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tbPasswod);
            Controls.Add(tbLogin);
            Controls.Add(bAuthorize);
            Name = "AuthControl";
            Size = new Size(425, 450);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bRegister;
        private Label label2;
        private Label label1;
        private TextBox tbPasswod;
        private TextBox tbLogin;
        private Button bAuthorize;
    }
}
