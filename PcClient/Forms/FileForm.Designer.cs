namespace PcClient.Forms
{
    partial class FileForm
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
            components = new System.ComponentModel.Container();
            pMain = new Panel();
            flpCards = new FlowLayoutPanel();
            bBack = new Button();
            lCurrent = new Label();
            cmsMain = new ContextMenuStrip(components);
            tsmiCreate = new ToolStripMenuItem();
            tsmiCreateFile = new ToolStripMenuItem();
            tsmiCreateFolder = new ToolStripMenuItem();
            tbSearch = new TextBox();
            bClearSearch = new Button();
            bShowFilters = new Button();
            pMain.SuspendLayout();
            cmsMain.SuspendLayout();
            SuspendLayout();
            // 
            // pMain
            // 
            pMain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pMain.BackColor = SystemColors.Window;
            pMain.Controls.Add(flpCards);
            pMain.Location = new Point(0, 40);
            pMain.Name = "pMain";
            pMain.Size = new Size(1262, 651);
            pMain.TabIndex = 1;
            // 
            // flpCards
            // 
            flpCards.AutoScroll = true;
            flpCards.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCards.BackColor = SystemColors.Window;
            flpCards.Dock = DockStyle.Fill;
            flpCards.FlowDirection = FlowDirection.TopDown;
            flpCards.Location = new Point(0, 0);
            flpCards.Name = "flpCards";
            flpCards.Size = new Size(1262, 651);
            flpCards.TabIndex = 3;
            flpCards.MouseClick += flpCards_MouseClick;
            // 
            // bBack
            // 
            bBack.Location = new Point(12, 5);
            bBack.Name = "bBack";
            bBack.Size = new Size(94, 29);
            bBack.TabIndex = 2;
            bBack.Text = "Назад";
            bBack.UseVisualStyleBackColor = true;
            bBack.Click += bBack_Click;
            // 
            // lCurrent
            // 
            lCurrent.AutoSize = true;
            lCurrent.Dock = DockStyle.Right;
            lCurrent.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            lCurrent.Location = new Point(1148, 0);
            lCurrent.Name = "lCurrent";
            lCurrent.Size = new Size(114, 31);
            lCurrent.TabIndex = 3;
            lCurrent.Text = "Название";
            // 
            // cmsMain
            // 
            cmsMain.ImageScalingSize = new Size(20, 20);
            cmsMain.Items.AddRange(new ToolStripItem[] { tsmiCreate });
            cmsMain.Name = "contextMenuStrip1";
            cmsMain.Size = new Size(134, 28);
            // 
            // tsmiCreate
            // 
            tsmiCreate.DropDownItems.AddRange(new ToolStripItem[] { tsmiCreateFile, tsmiCreateFolder });
            tsmiCreate.Name = "tsmiCreate";
            tsmiCreate.Size = new Size(133, 24);
            tsmiCreate.Text = "Создать";
            // 
            // tsmiCreateFile
            // 
            tsmiCreateFile.Name = "tsmiCreateFile";
            tsmiCreateFile.Size = new Size(134, 26);
            tsmiCreateFile.Text = "Файл";
            tsmiCreateFile.Click += tsmiCreateFile_Click;
            // 
            // tsmiCreateFolder
            // 
            tsmiCreateFolder.Name = "tsmiCreateFolder";
            tsmiCreateFolder.Size = new Size(134, 26);
            tsmiCreateFolder.Text = "Папку";
            tsmiCreateFolder.Click += tsmiCreateFolder_Click;
            // 
            // tbSearch
            // 
            tbSearch.Location = new Point(398, 7);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(416, 27);
            tbSearch.TabIndex = 4;
            tbSearch.KeyPress += tbSearch_KeyPress;
            // 
            // bClearSearch
            // 
            bClearSearch.Location = new Point(820, 6);
            bClearSearch.Name = "bClearSearch";
            bClearSearch.Size = new Size(35, 29);
            bClearSearch.TabIndex = 5;
            bClearSearch.Text = "X";
            bClearSearch.UseVisualStyleBackColor = true;
            bClearSearch.Click += bClearSearch_Click;
            // 
            // bShowFilters
            // 
            bShowFilters.Location = new Point(861, 6);
            bShowFilters.Name = "bShowFilters";
            bShowFilters.Size = new Size(36, 29);
            bShowFilters.TabIndex = 6;
            bShowFilters.Text = "Ф";
            bShowFilters.UseVisualStyleBackColor = true;
            bShowFilters.Click += bShowFilters_Click;
            // 
            // FileForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1262, 691);
            Controls.Add(bShowFilters);
            Controls.Add(bClearSearch);
            Controls.Add(tbSearch);
            Controls.Add(lCurrent);
            Controls.Add(bBack);
            Controls.Add(pMain);
            Name = "FileForm";
            Text = "FileForm";
            Resize += FileForm_Resize;
            pMain.ResumeLayout(false);
            cmsMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pMain;
        private FlowLayoutPanel flpCards;
        private Button bBack;
        private Label lCurrent;
        private ContextMenuStrip cmsMain;
        private ToolStripMenuItem tsmiCreate;
        private ToolStripMenuItem tsmiCreateFile;
        private ToolStripMenuItem tsmiCreateFolder;
        private TextBox tbSearch;
        private Button bClearSearch;
        private Button bShowFilters;
    }
}