namespace PcClient.Forms
{
    partial class FilterForm
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
            cbFilesOnly = new CheckBox();
            cbFoldersOnly = new CheckBox();
            tbSize = new TextBox();
            comboBSizeScale = new ComboBox();
            label1 = new Label();
            comboBSizeComparsionType = new ComboBox();
            comboBSortDir = new ComboBox();
            label2 = new Label();
            comboBSortType = new ComboBox();
            bApply = new Button();
            bClear = new Button();
            SuspendLayout();
            // 
            // cbFilesOnly
            // 
            cbFilesOnly.AutoSize = true;
            cbFilesOnly.Location = new Point(12, 12);
            cbFilesOnly.Name = "cbFilesOnly";
            cbFilesOnly.Size = new Size(130, 24);
            cbFilesOnly.TabIndex = 0;
            cbFilesOnly.Text = "Только файлы";
            cbFilesOnly.UseVisualStyleBackColor = true;
            cbFilesOnly.Click += chk_Click;
            // 
            // cbFoldersOnly
            // 
            cbFoldersOnly.AutoSize = true;
            cbFoldersOnly.Location = new Point(157, 12);
            cbFoldersOnly.Name = "cbFoldersOnly";
            cbFoldersOnly.Size = new Size(126, 24);
            cbFoldersOnly.TabIndex = 1;
            cbFoldersOnly.Text = "Только папки";
            cbFoldersOnly.UseVisualStyleBackColor = true;
            cbFoldersOnly.Click += chk_Click;
            // 
            // tbSize
            // 
            tbSize.Location = new Point(126, 42);
            tbSize.Name = "tbSize";
            tbSize.Size = new Size(125, 27);
            tbSize.TabIndex = 2;
            tbSize.KeyPress += tbSize_KeyPress;
            // 
            // comboBSizeScale
            // 
            comboBSizeScale.FormattingEnabled = true;
            comboBSizeScale.Location = new Point(257, 41);
            comboBSizeScale.Name = "comboBSizeScale";
            comboBSizeScale.Size = new Size(61, 28);
            comboBSizeScale.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 44);
            label1.Name = "label1";
            label1.Size = new Size(63, 20);
            label1.TabIndex = 4;
            label1.Text = "Размер:";
            // 
            // comboBSizeComparsionType
            // 
            comboBSizeComparsionType.FormattingEnabled = true;
            comboBSizeComparsionType.Location = new Point(74, 41);
            comboBSizeComparsionType.Name = "comboBSizeComparsionType";
            comboBSizeComparsionType.Size = new Size(46, 28);
            comboBSizeComparsionType.TabIndex = 5;
            // 
            // comboBSortDir
            // 
            comboBSortDir.FormattingEnabled = true;
            comboBSortDir.Location = new Point(148, 115);
            comboBSortDir.Name = "comboBSortDir";
            comboBSortDir.Size = new Size(150, 28);
            comboBSortDir.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 92);
            label2.Name = "label2";
            label2.Size = new Size(95, 20);
            label2.TabIndex = 8;
            label2.Text = "Сортировка:";
            // 
            // comboBSortType
            // 
            comboBSortType.FormattingEnabled = true;
            comboBSortType.Location = new Point(14, 115);
            comboBSortType.Name = "comboBSortType";
            comboBSortType.Size = new Size(128, 28);
            comboBSortType.TabIndex = 7;
            // 
            // bApply
            // 
            bApply.Location = new Point(113, 162);
            bApply.Name = "bApply";
            bApply.Size = new Size(106, 29);
            bApply.TabIndex = 10;
            bApply.Text = "Применить";
            bApply.UseVisualStyleBackColor = true;
            bApply.Click += bApply_Click;
            // 
            // bClear
            // 
            bClear.Location = new Point(308, 4);
            bClear.Name = "bClear";
            bClear.Size = new Size(29, 31);
            bClear.TabIndex = 11;
            bClear.Text = "X";
            bClear.UseVisualStyleBackColor = true;
            bClear.Click += bClear_Click;
            // 
            // FilterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(349, 203);
            Controls.Add(bClear);
            Controls.Add(bApply);
            Controls.Add(comboBSortDir);
            Controls.Add(label2);
            Controls.Add(comboBSortType);
            Controls.Add(comboBSizeComparsionType);
            Controls.Add(label1);
            Controls.Add(comboBSizeScale);
            Controls.Add(tbSize);
            Controls.Add(cbFoldersOnly);
            Controls.Add(cbFilesOnly);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FilterForm";
            Text = "FilterForm";
            Deactivate += FilterForm_Deactivate;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox cbFilesOnly;
        private CheckBox cbFoldersOnly;
        private TextBox tbSize;
        private ComboBox comboBSizeScale;
        private Label label1;
        private ComboBox comboBSizeComparsionType;
        private ComboBox comboBSortDir;
        private Label label2;
        private ComboBox comboBSortType;
        private Button bApply;
        private Button bClear;
    }
}