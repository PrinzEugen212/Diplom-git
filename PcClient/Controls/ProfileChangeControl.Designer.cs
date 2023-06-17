namespace PcClient.Controls
{
    partial class ProfileChangeControl
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
            bChange = new Button();
            label4 = new Label();
            tbRepeatPassword = new TextBox();
            label3 = new Label();
            tbLogin = new TextBox();
            label2 = new Label();
            label1 = new Label();
            tbPassword = new TextBox();
            tbEmail = new TextBox();
            SuspendLayout();
            // 
            // bChange
            // 
            bChange.Anchor = AnchorStyles.Bottom;
            bChange.Location = new Point(121, 265);
            bChange.Name = "bChange";
            bChange.Size = new Size(133, 46);
            bChange.TabIndex = 39;
            bChange.Text = "Изменить";
            bChange.UseVisualStyleBackColor = true;
            bChange.Click += bChange_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 170);
            label4.Name = "label4";
            label4.Size = new Size(139, 20);
            label4.TabIndex = 38;
            label4.Text = "Повторите пароль";
            // 
            // tbRepeatPassword
            // 
            tbRepeatPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbRepeatPassword.Location = new Point(3, 193);
            tbRepeatPassword.Name = "tbRepeatPassword";
            tbRepeatPassword.Size = new Size(374, 27);
            tbRepeatPassword.TabIndex = 37;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 4);
            label3.Name = "label3";
            label3.Size = new Size(52, 20);
            label3.TabIndex = 36;
            label3.Text = "Логин";
            // 
            // tbLogin
            // 
            tbLogin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbLogin.Location = new Point(3, 27);
            tbLogin.Name = "tbLogin";
            tbLogin.Size = new Size(374, 27);
            tbLogin.TabIndex = 35;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 110);
            label2.Name = "label2";
            label2.Size = new Size(62, 20);
            label2.TabIndex = 34;
            label2.Text = "Пароль";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 57);
            label1.Name = "label1";
            label1.Size = new Size(46, 20);
            label1.TabIndex = 33;
            label1.Text = "Email";
            // 
            // tbPassword
            // 
            tbPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbPassword.Location = new Point(3, 133);
            tbPassword.Name = "tbPassword";
            tbPassword.Size = new Size(374, 27);
            tbPassword.TabIndex = 32;
            // 
            // tbEmail
            // 
            tbEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbEmail.Location = new Point(3, 80);
            tbEmail.Name = "tbEmail";
            tbEmail.Size = new Size(374, 27);
            tbEmail.TabIndex = 31;
            // 
            // ProfileChangeControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(bChange);
            Controls.Add(label4);
            Controls.Add(tbRepeatPassword);
            Controls.Add(label3);
            Controls.Add(tbLogin);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tbPassword);
            Controls.Add(tbEmail);
            Name = "ProfileChangeControl";
            Size = new Size(388, 347);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bChange;
        private Label label4;
        private TextBox tbRepeatPassword;
        private Label label3;
        private TextBox tbLogin;
        private Label label2;
        private Label label1;
        private TextBox tbPassword;
        private TextBox tbEmail;
    }
}
