namespace NihongoStudyBuilder
{
    partial class App
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.mTabWriteChapterDecks = new System.Windows.Forms.TabPage();
            this.mChapterEnabledOptions = new System.Windows.Forms.CheckedListBox();
            this.mBtnWriteChapterDecks = new System.Windows.Forms.Button();
            this.mTableWriteAggregateDecks = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mAggRadioEverything = new System.Windows.Forms.RadioButton();
            this.mAggRadioPhrases = new System.Windows.Forms.RadioButton();
            this.mAggRadioAdjectives = new System.Windows.Forms.RadioButton();
            this.mAggRadioVerbs = new System.Windows.Forms.RadioButton();
            this.mAggregateRangeDisplay = new System.Windows.Forms.Label();
            this.mAggregateEnabledOptions = new System.Windows.Forms.CheckedListBox();
            this.mTrackBarMax = new System.Windows.Forms.TrackBar();
            this.mTrackBarMin = new System.Windows.Forms.TrackBar();
            this.mBtnWriteAggregateDecks = new System.Windows.Forms.Button();
            this.mReloadDeckSerializer = new System.Windows.Forms.Button();
            this.mProgressBar = new System.Windows.Forms.ProgressBar();
            this.mInfoPage = new System.Windows.Forms.TabPage();
            this.mInfoGrid = new System.Windows.Forms.DataGridView();
            this.mLblLoadResults = new System.Windows.Forms.Label();
            this.mLoadResults = new System.Windows.Forms.Label();
            this.mInfoGridChapter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mInfoGridMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.mTabWriteChapterDecks.SuspendLayout();
            this.mTableWriteAggregateDecks.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mTrackBarMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mTrackBarMin)).BeginInit();
            this.mInfoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mInfoGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.mInfoPage);
            this.tabControl1.Controls.Add(this.mTabWriteChapterDecks);
            this.tabControl1.Controls.Add(this.mTableWriteAggregateDecks);
            this.tabControl1.Location = new System.Drawing.Point(0, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(442, 374);
            this.tabControl1.TabIndex = 1;
            // 
            // mTabWriteChapterDecks
            // 
            this.mTabWriteChapterDecks.Controls.Add(this.mChapterEnabledOptions);
            this.mTabWriteChapterDecks.Controls.Add(this.mBtnWriteChapterDecks);
            this.mTabWriteChapterDecks.Location = new System.Drawing.Point(4, 22);
            this.mTabWriteChapterDecks.Name = "mTabWriteChapterDecks";
            this.mTabWriteChapterDecks.Padding = new System.Windows.Forms.Padding(3);
            this.mTabWriteChapterDecks.Size = new System.Drawing.Size(434, 348);
            this.mTabWriteChapterDecks.TabIndex = 0;
            this.mTabWriteChapterDecks.Text = "Write Chapter Decks";
            this.mTabWriteChapterDecks.UseVisualStyleBackColor = true;
            // 
            // mChapterEnabledOptions
            // 
            this.mChapterEnabledOptions.FormattingEnabled = true;
            this.mChapterEnabledOptions.Items.AddRange(new object[] {
            "Plain Form"});
            this.mChapterEnabledOptions.Location = new System.Drawing.Point(8, 35);
            this.mChapterEnabledOptions.Name = "mChapterEnabledOptions";
            this.mChapterEnabledOptions.Size = new System.Drawing.Size(207, 94);
            this.mChapterEnabledOptions.TabIndex = 7;
            // 
            // mBtnWriteChapterDecks
            // 
            this.mBtnWriteChapterDecks.Location = new System.Drawing.Point(6, 6);
            this.mBtnWriteChapterDecks.Name = "mBtnWriteChapterDecks";
            this.mBtnWriteChapterDecks.Size = new System.Drawing.Size(140, 23);
            this.mBtnWriteChapterDecks.TabIndex = 2;
            this.mBtnWriteChapterDecks.Text = "Write Chapter Decks";
            this.mBtnWriteChapterDecks.UseVisualStyleBackColor = true;
            this.mBtnWriteChapterDecks.Click += new System.EventHandler(this.mBtnWriteChapterDecks_Click);
            // 
            // mTableWriteAggregateDecks
            // 
            this.mTableWriteAggregateDecks.Controls.Add(this.panel1);
            this.mTableWriteAggregateDecks.Controls.Add(this.mAggregateRangeDisplay);
            this.mTableWriteAggregateDecks.Controls.Add(this.mAggregateEnabledOptions);
            this.mTableWriteAggregateDecks.Controls.Add(this.mTrackBarMax);
            this.mTableWriteAggregateDecks.Controls.Add(this.mTrackBarMin);
            this.mTableWriteAggregateDecks.Controls.Add(this.mBtnWriteAggregateDecks);
            this.mTableWriteAggregateDecks.Location = new System.Drawing.Point(4, 22);
            this.mTableWriteAggregateDecks.Name = "mTableWriteAggregateDecks";
            this.mTableWriteAggregateDecks.Padding = new System.Windows.Forms.Padding(3);
            this.mTableWriteAggregateDecks.Size = new System.Drawing.Size(434, 348);
            this.mTableWriteAggregateDecks.TabIndex = 1;
            this.mTableWriteAggregateDecks.Text = "Write Aggregate Decks";
            this.mTableWriteAggregateDecks.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.mAggRadioEverything);
            this.panel1.Controls.Add(this.mAggRadioPhrases);
            this.panel1.Controls.Add(this.mAggRadioAdjectives);
            this.panel1.Controls.Add(this.mAggRadioVerbs);
            this.panel1.Location = new System.Drawing.Point(221, 137);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(207, 94);
            this.panel1.TabIndex = 9;
            // 
            // mAggRadioEverything
            // 
            this.mAggRadioEverything.AutoSize = true;
            this.mAggRadioEverything.Checked = true;
            this.mAggRadioEverything.Location = new System.Drawing.Point(3, 63);
            this.mAggRadioEverything.Name = "mAggRadioEverything";
            this.mAggRadioEverything.Size = new System.Drawing.Size(75, 17);
            this.mAggRadioEverything.TabIndex = 3;
            this.mAggRadioEverything.TabStop = true;
            this.mAggRadioEverything.Text = "Everything";
            this.mAggRadioEverything.UseVisualStyleBackColor = true;
            this.mAggRadioEverything.CheckedChanged += new System.EventHandler(this.mAggRadio_CheckedChanged);
            // 
            // mAggRadioPhrases
            // 
            this.mAggRadioPhrases.AutoSize = true;
            this.mAggRadioPhrases.Location = new System.Drawing.Point(3, 43);
            this.mAggRadioPhrases.Name = "mAggRadioPhrases";
            this.mAggRadioPhrases.Size = new System.Drawing.Size(63, 17);
            this.mAggRadioPhrases.TabIndex = 2;
            this.mAggRadioPhrases.Text = "Phrases";
            this.mAggRadioPhrases.UseVisualStyleBackColor = true;
            this.mAggRadioPhrases.CheckedChanged += new System.EventHandler(this.mAggRadio_CheckedChanged);
            // 
            // mAggRadioAdjectives
            // 
            this.mAggRadioAdjectives.AutoSize = true;
            this.mAggRadioAdjectives.Location = new System.Drawing.Point(3, 23);
            this.mAggRadioAdjectives.Name = "mAggRadioAdjectives";
            this.mAggRadioAdjectives.Size = new System.Drawing.Size(74, 17);
            this.mAggRadioAdjectives.TabIndex = 1;
            this.mAggRadioAdjectives.Text = "Adjectives";
            this.mAggRadioAdjectives.UseVisualStyleBackColor = true;
            this.mAggRadioAdjectives.CheckedChanged += new System.EventHandler(this.mAggRadio_CheckedChanged);
            // 
            // mAggRadioVerbs
            // 
            this.mAggRadioVerbs.AutoSize = true;
            this.mAggRadioVerbs.Location = new System.Drawing.Point(4, 4);
            this.mAggRadioVerbs.Name = "mAggRadioVerbs";
            this.mAggRadioVerbs.Size = new System.Drawing.Size(52, 17);
            this.mAggRadioVerbs.TabIndex = 0;
            this.mAggRadioVerbs.Text = "Verbs";
            this.mAggRadioVerbs.UseVisualStyleBackColor = true;
            this.mAggRadioVerbs.CheckedChanged += new System.EventHandler(this.mAggRadio_CheckedChanged);
            // 
            // mAggregateRangeDisplay
            // 
            this.mAggregateRangeDisplay.Location = new System.Drawing.Point(177, 10);
            this.mAggregateRangeDisplay.Name = "mAggregateRangeDisplay";
            this.mAggregateRangeDisplay.Size = new System.Drawing.Size(251, 19);
            this.mAggregateRangeDisplay.TabIndex = 8;
            this.mAggregateRangeDisplay.Text = "label1";
            // 
            // mAggregateEnabledOptions
            // 
            this.mAggregateEnabledOptions.FormattingEnabled = true;
            this.mAggregateEnabledOptions.Items.AddRange(new object[] {
            "Plain Form",
            "Randomize"});
            this.mAggregateEnabledOptions.Location = new System.Drawing.Point(6, 137);
            this.mAggregateEnabledOptions.Name = "mAggregateEnabledOptions";
            this.mAggregateEnabledOptions.Size = new System.Drawing.Size(207, 94);
            this.mAggregateEnabledOptions.TabIndex = 6;
            // 
            // mTrackBarMax
            // 
            this.mTrackBarMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mTrackBarMax.Location = new System.Drawing.Point(6, 86);
            this.mTrackBarMax.Name = "mTrackBarMax";
            this.mTrackBarMax.Size = new System.Drawing.Size(422, 45);
            this.mTrackBarMax.TabIndex = 5;
            this.mTrackBarMax.ValueChanged += new System.EventHandler(this.mTrackBar_ValueChanged);
            // 
            // mTrackBarMin
            // 
            this.mTrackBarMin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mTrackBarMin.Cursor = System.Windows.Forms.Cursors.Default;
            this.mTrackBarMin.Location = new System.Drawing.Point(6, 35);
            this.mTrackBarMin.Name = "mTrackBarMin";
            this.mTrackBarMin.Size = new System.Drawing.Size(422, 45);
            this.mTrackBarMin.TabIndex = 4;
            this.mTrackBarMin.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.mTrackBarMin.ValueChanged += new System.EventHandler(this.mTrackBar_ValueChanged);
            // 
            // mBtnWriteAggregateDecks
            // 
            this.mBtnWriteAggregateDecks.Location = new System.Drawing.Point(6, 6);
            this.mBtnWriteAggregateDecks.Name = "mBtnWriteAggregateDecks";
            this.mBtnWriteAggregateDecks.Size = new System.Drawing.Size(140, 23);
            this.mBtnWriteAggregateDecks.TabIndex = 3;
            this.mBtnWriteAggregateDecks.Text = "Write Aggregate Decks";
            this.mBtnWriteAggregateDecks.UseVisualStyleBackColor = true;
            this.mBtnWriteAggregateDecks.Click += new System.EventHandler(this.mBtnWriteAggregateDecks_Click);
            // 
            // mReloadDeckSerializer
            // 
            this.mReloadDeckSerializer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mReloadDeckSerializer.Location = new System.Drawing.Point(300, 2);
            this.mReloadDeckSerializer.Name = "mReloadDeckSerializer";
            this.mReloadDeckSerializer.Size = new System.Drawing.Size(132, 23);
            this.mReloadDeckSerializer.TabIndex = 2;
            this.mReloadDeckSerializer.Text = "Reload Deck Serializer";
            this.mReloadDeckSerializer.UseVisualStyleBackColor = true;
            this.mReloadDeckSerializer.Click += new System.EventHandler(this.mReloadDeckSerializer_Click);
            // 
            // mProgressBar
            // 
            this.mProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mProgressBar.Location = new System.Drawing.Point(10, 2);
            this.mProgressBar.Name = "mProgressBar";
            this.mProgressBar.Size = new System.Drawing.Size(282, 23);
            this.mProgressBar.TabIndex = 3;
            // 
            // mInfoPage
            // 
            this.mInfoPage.Controls.Add(this.mLoadResults);
            this.mInfoPage.Controls.Add(this.mLblLoadResults);
            this.mInfoPage.Controls.Add(this.mInfoGrid);
            this.mInfoPage.Location = new System.Drawing.Point(4, 22);
            this.mInfoPage.Name = "mInfoPage";
            this.mInfoPage.Size = new System.Drawing.Size(434, 348);
            this.mInfoPage.TabIndex = 2;
            this.mInfoPage.Text = "Info";
            this.mInfoPage.UseVisualStyleBackColor = true;
            // 
            // mInfoGrid
            // 
            this.mInfoGrid.AllowUserToAddRows = false;
            this.mInfoGrid.AllowUserToDeleteRows = false;
            this.mInfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mInfoGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mInfoGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.mInfoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mInfoGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mInfoGridChapter,
            this.mInfoGridMessage});
            this.mInfoGrid.Location = new System.Drawing.Point(0, 25);
            this.mInfoGrid.Name = "mInfoGrid";
            this.mInfoGrid.ReadOnly = true;
            this.mInfoGrid.RowHeadersVisible = false;
            this.mInfoGrid.Size = new System.Drawing.Size(432, 322);
            this.mInfoGrid.TabIndex = 0;
            // 
            // mLblLoadResults
            // 
            this.mLblLoadResults.AutoSize = true;
            this.mLblLoadResults.Location = new System.Drawing.Point(3, 4);
            this.mLblLoadResults.Name = "mLblLoadResults";
            this.mLblLoadResults.Size = new System.Drawing.Size(72, 13);
            this.mLblLoadResults.TabIndex = 1;
            this.mLblLoadResults.Text = "Load Results:";
            // 
            // mLoadResults
            // 
            this.mLoadResults.AutoSize = true;
            this.mLoadResults.Location = new System.Drawing.Point(81, 4);
            this.mLoadResults.Name = "mLoadResults";
            this.mLoadResults.Size = new System.Drawing.Size(0, 13);
            this.mLoadResults.TabIndex = 2;
            // 
            // mInfoGridChapter
            // 
            this.mInfoGridChapter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.mInfoGridChapter.Frozen = true;
            this.mInfoGridChapter.HeaderText = "Chapter";
            this.mInfoGridChapter.Name = "mInfoGridChapter";
            this.mInfoGridChapter.ReadOnly = true;
            this.mInfoGridChapter.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.mInfoGridChapter.Width = 65;
            // 
            // mInfoGridMessage
            // 
            this.mInfoGridMessage.HeaderText = "Message";
            this.mInfoGridMessage.Name = "mInfoGridMessage";
            this.mInfoGridMessage.ReadOnly = true;
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 403);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.mProgressBar);
            this.Controls.Add(this.mReloadDeckSerializer);
            this.Name = "App";
            this.Text = "Minna no Nihongo - Study Converter";
            this.Load += new System.EventHandler(this.mApp_Load);
            this.tabControl1.ResumeLayout(false);
            this.mTabWriteChapterDecks.ResumeLayout(false);
            this.mTableWriteAggregateDecks.ResumeLayout(false);
            this.mTableWriteAggregateDecks.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mTrackBarMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mTrackBarMin)).EndInit();
            this.mInfoPage.ResumeLayout(false);
            this.mInfoPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mInfoGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage mTabWriteChapterDecks;
        private System.Windows.Forms.TabPage mTableWriteAggregateDecks;
        private System.Windows.Forms.Button mBtnWriteChapterDecks;
        private System.Windows.Forms.Button mBtnWriteAggregateDecks;
        private System.Windows.Forms.Label mAggregateRangeDisplay;
        private System.Windows.Forms.CheckedListBox mAggregateEnabledOptions;
        private System.Windows.Forms.TrackBar mTrackBarMax;
        private System.Windows.Forms.TrackBar mTrackBarMin;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton mAggRadioEverything;
        private System.Windows.Forms.RadioButton mAggRadioPhrases;
        private System.Windows.Forms.RadioButton mAggRadioAdjectives;
        private System.Windows.Forms.RadioButton mAggRadioVerbs;
        private System.Windows.Forms.Button mReloadDeckSerializer;
        private System.Windows.Forms.ProgressBar mProgressBar;
        private System.Windows.Forms.CheckedListBox mChapterEnabledOptions;
        private System.Windows.Forms.TabPage mInfoPage;
        private System.Windows.Forms.Label mLblLoadResults;
        private System.Windows.Forms.DataGridView mInfoGrid;
        private System.Windows.Forms.Label mLoadResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn mInfoGridChapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn mInfoGridMessage;
    }
}

