using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Course_work_on_Windows_Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            OpenTabs();
            SaveTabs();
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }
        #region Properti SelectedNameTb
        public RichTextBox SelectedNameTb
        {
            get
            {
                if (tabControl.Controls.Count != 0)
                {
                    foreach (var item in tabControl.SelectedTab.Controls.OfType<RichTextBox>())
                    {
                        if (item.Name == "richTextBox")
                        {
                            return item;
                        }
                    }
                }
                return null;
            }
        }
        #endregion
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex != -1 && SelectedNameTb != null)
            {
                SelectedNameTb.TextChanged -= TextBox_TextChanged;
                SelectedNameTb.TextChanged += TextBox_TextChanged;

                TextBox_TextChanged(sender, e);
            }
        }


        private void TextBox_Size_Changed(object sender, EventArgs e)
        {
            SelectedNameTb.Size = new Size(tabControl.Width, tabControl.Height);
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            toolStripCountWordsLabel.Text = Regex.Matches(SelectedNameTb.Text.ToString(), @"[a-zA-Z]{1,}|[à-ÿÀ-ß]{1,}").Count.ToString();
            toolStripCountSymbolsLabel.Text = Regex.Matches(SelectedNameTb.Text.ToString(), @"[a-zA-Z]|[à-ÿÀ-ß]").Count.ToString();
            toolStripCountNumbersLabel.Text = Regex.Matches(SelectedNameTb.Text.ToString(), @"\d+").Count.ToString();
            toolStripSymbolsProgressBar.Value = SelectedNameTb.Text.Length;
            toolStripMaxLenghtLabel.Text = SelectedNameTb.Text.Length.ToString() + $"/{SelectedNameTb.MaxLength}";

        }
        private void toolStripButtonLoadFile_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex != -1)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                openFileDialog.Filter = "All file(*.*)|*.*|Text files(*.txt)|*.txt|Rtf files(*.rtf)|*.rtf||";
                openFileDialog.FilterIndex = 2;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(openFileDialog.FileName) == ".rtf")
                    {
                        MessageBox.Show(openFileDialog.FileName);
                        SelectedNameTb.LoadFile(openFileDialog.FileName);
                        string fileName = Path.GetFileName(openFileDialog.FileName);
                        tabControl.SelectedTab.Text = fileName;

                        tabControl.SelectedTab.Tag = openFileDialog.FileName;
                    }
                    else if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                    {
                        string message = File.ReadAllText(openFileDialog.FileName);

                        SelectedNameTb.Text = message;

                        string fileName = Path.GetFileName(openFileDialog.FileName);
                        tabControl.SelectedTab.Text = fileName;

                        tabControl.SelectedTab.Tag = openFileDialog.FileName;
                    }
                    SaveTabs();
                }
            }
            else
            {
                MessageBox.Show("You need to select a tab at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex != -1)
            {
                string currentTabText = tabControl.SelectedTab.Text;
                if (currentTabText.StartsWith("Untitled"))
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    //saveFileDialog.CreatePrompt = true;
                    saveFileDialog.OverwritePrompt = true;
                    saveFileDialog.Filter = "Text Documents (*.txt)|*.txt|All files (*.*)|*.*";

                    saveFileDialog.FilterIndex = 1;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog.FileName;
                        tabControl.SelectedTab.Tag = fileName;
                        File.WriteAllText(fileName, SelectedNameTb.Text);

                        string tabText = Path.GetFileName(fileName);
                        tabControl.SelectedTab.Text = tabText;

                        SaveTabs();
                    }
                }
                else
                {
                    string fileName = (string)tabControl.SelectedTab.Tag;

                    File.WriteAllText(fileName, SelectedNameTb.Text);
                }
            }
            else
            {
                MessageBox.Show("You need to select a tab at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            SelectedNameTb.Copy();
        }
        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            if (SelectedNameTb != null)
                SelectedNameTb.Cut();
        }
        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            if (SelectedNameTb != null)
                SelectedNameTb.Paste();
        }
        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            if (SelectedNameTb != null)
                SelectedNameTb.Clear();
        }
        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            if (SelectedNameTb != null)
                SelectedNameTb.Undo();
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            if (SelectedNameTb != null)
                SelectedNameTb.Redo();
        }
        private void toolStripButtonSetingColorText_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedNameTb.SelectionColor = colorDialog.Color;
            }
        }
        private void toolStripButtonSetingColorFont_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedNameTb.SelectionBackColor = colorDialog.Color;
            }
        }
        private void toolStripButtonSetingText_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedNameTb.SelectionFont = fontDialog.Font;
            }
        }
        private void exitToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            SaveTabs();
            this.Close();
        }
        private void fontAllTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedNameTb.Font = fontDialog.Font;
            }
        }
        private void bacColorAllTexlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedNameTb.BackColor = colorDialog.Color;
            }
        }
        private void foreColorAllTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedNameTb.ForeColor = colorDialog.Color;
            }
        }
        private void AddTab()
        {
            TabPage tabPage = new TabPage($"Untitled {tabControl.TabPages.Count + 1}");
            tabPage.UseVisualStyleBackColor = true;
            RichTextBox mainRichTextBox = new RichTextBox();
            mainRichTextBox.ContextMenuStrip = contextMenuStripRichTextBox;
            mainRichTextBox.Location = new Point(0, 0);
            mainRichTextBox.Name = "richTextBox";
            mainRichTextBox.Size = new Size(tabControl.Width, tabControl.Height);
            mainRichTextBox.MaxLength = 10000;
            mainRichTextBox.TabIndex = 13;
            mainRichTextBox.Text = "";
            mainRichTextBox.TextChanged += new EventHandler(TextBox_TextChanged);
            mainRichTextBox.Click += new EventHandler(TextBox_Size_Changed);


            tabPage.Controls.Add(mainRichTextBox);

            mainRichTextBox.AllowDrop = true;

            tabPage.Tag = null;

            tabControl.TabPages.Add(tabPage);

            SaveTabs();
        }
        private void toolStripButtonAddTab_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void MainrichTextBox_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string item = e.Data.GetData(DataFormats.Text).ToString();
                SelectedNameTb.Text = item;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string res = SelectedNameTb.Text;
                foreach (var item in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    if (Path.GetExtension(item) == ".rtf")
                    {
                        SelectedNameTb.LoadFile(item);
                        res += SelectedNameTb.Text + "\n";
                        SelectedNameTb.Text = res;
                    }
                }
            }
        }

        private void SaveTabs()
        {
            List<string> paths = new List<string>();
            int countUntitled = 0;

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (tabPage.Tag != null)
                {
                    paths.Add((string)tabPage.Tag);
                }
                else
                {
                    countUntitled++;
                }
            }

            string filePath = "tabPaths.txt";
            string untitledFilePath = "untitledFiles.txt";
            try
            {
                File.WriteAllLines(filePath, paths);
                File.WriteAllText(untitledFilePath, countUntitled.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void OpenTabs()
        {
            string filePath = "tabPaths.txt";
            string untitledFilePath = "untitledFiles.txt";
            try
            {
                if (File.Exists(filePath) && File.Exists(untitledFilePath))
                {
                    string[] savedPaths = File.ReadAllLines(filePath);

                    foreach (string path in savedPaths)
                    {
                        TabPage tabPage = new TabPage(Path.GetFileName(path));

                        RichTextBox mainrichTextBox = new RichTextBox();
                        tabPage.UseVisualStyleBackColor = true;
                        mainrichTextBox.ContextMenuStrip = contextMenuStripRichTextBox;
                        mainrichTextBox.AllowDrop = true;
                        mainrichTextBox.Location = new Point(0, 0);
                        mainrichTextBox.Margin = new Padding(3, 3, 3, 3);
                        mainrichTextBox.MaxLength = 10000;
                        mainrichTextBox.Name = "richTextBox";
                        mainrichTextBox.Size = new Size(tabControl.Width, tabControl.Height);
                        mainrichTextBox.TabIndex = 29;
                        if (Path.GetExtension(path) == ".rtf")
                            mainrichTextBox.LoadFile(path);
                        if (Path.GetExtension(path) == ".txt")
                            mainrichTextBox.Text = File.ReadAllText(path);
                        mainrichTextBox.Click += (sender, e) =>
                        {
                            TextBox_TextChanged(sender, e);
                            TextBox_Size_Changed(sender, e);
                        };

                        toolStripMaxLenghtLabel.Text = $"{0}/{mainrichTextBox.MaxLength}";
                        toolStripSymbolsProgressBar.Value = 0;
                        toolStripCountNumbersLabel.Text = $"{0}";
                        toolStripCountSymbolsLabel.Text = $"{0}";
                        toolStripCountWordsLabel.Text = $"{0}";
                        mainrichTextBox.TextChanged += (sender, e) =>
                        {
                            TextBox_TextChanged(sender, e);
                        };
                        mainrichTextBox.DragDrop += MainrichTextBox_DragDrop;

                        tabPage.Controls.Add(mainrichTextBox);
                        tabPage.Tag = path;
                        tabControl.TabPages.Add(tabPage);

                    }

                    string countString = File.ReadAllText(untitledFilePath);
                    if (int.TryParse(countString, out int count))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            AddTab();
                        }
                    }
                }
                else if (File.Exists(filePath))
                {
                    string[] savedPaths = File.ReadAllLines(filePath);

                    foreach (string path in savedPaths)
                    {
                        TabPage tabPage = new TabPage(Path.GetFileName(path));
                        RichTextBox mainrichTextBox = new RichTextBox();
                        tabPage.UseVisualStyleBackColor = true;
                        mainrichTextBox.ContextMenuStrip = contextMenuStripRichTextBox;
                        mainrichTextBox.AllowDrop = true;
                        mainrichTextBox.Location = new Point(0, 0);
                        mainrichTextBox.Margin = new Padding(3, 4, 3, 4);
                        mainrichTextBox.MaxLength = 10000;
                        mainrichTextBox.Name = "richTextBox";
                        mainrichTextBox.Size = new Size(tabControl.Width, tabControl.Height);
                        mainrichTextBox.TabIndex = 29;
                        if (Path.GetExtension(path) == ".rtf")
                            mainrichTextBox.LoadFile(path);
                        if (Path.GetExtension(path) == ".txt")
                            mainrichTextBox.Text = File.ReadAllText(path);
                        mainrichTextBox.Click += (sender, e) =>
                        {
                            TextBox_TextChanged(sender, e);
                            TextBox_Size_Changed(sender, e);
                        };

                        toolStripMaxLenghtLabel.Text = $"{0}/{mainrichTextBox.MaxLength}";
                        toolStripSymbolsProgressBar.Value = 0;
                        toolStripCountNumbersLabel.Text = $"{0}";
                        toolStripCountSymbolsLabel.Text = $"{0}";
                        toolStripCountWordsLabel.Text = $"{0}";
                        mainrichTextBox.TextChanged += (sender, e) =>
                        {
                            TextBox_TextChanged(sender, e);
                        };
                        mainrichTextBox.DragDrop += MainrichTextBox_DragDrop;

                        tabPage.Controls.Add(mainrichTextBox);
                        tabPage.Tag = path;
                        tabControl.TabPages.Add(tabPage);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void toolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex != -1)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                //saveFileDialog.CreatePrompt = true;
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.Filter = "Text Documents (*.txt)|*.txt|All files (*.*)|*.*";

                saveFileDialog.FilterIndex = 1;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    File.WriteAllText(fileName, SelectedNameTb.Text);

                    string tabText = Path.GetFileName(fileName);
                    tabControl.SelectedTab.Text = tabText;

                    tabControl.SelectedTab.Tag = saveFileDialog.FileName;

                    SaveTabs();
                }
            }
            else
            {
                MessageBox.Show("You need to select a tab at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void toolStripButtonRemoveTab_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex != -1)
            {
                if (SelectedNameTb.Text.Length != 0)
                {
                    DialogResult result = MessageBox.Show("Do you want to save the current file?", "Warning!",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        //saveFileDialog.CreatePrompt = true;
                        saveFileDialog.OverwritePrompt = true;
                        saveFileDialog.Filter = "Text Documents (*.txt)|*.txt|All files (*.*)|*.*";
                        saveFileDialog.FilterIndex = 1;

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllText(saveFileDialog.FileName, SelectedNameTb.Text);
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                tabControl.TabPages.RemoveAt(tabControl.SelectedIndex);
                SaveTabs();
            }
            else
            {
                MessageBox.Show("You need to select a tab at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemNewDocument_Click(object sender, EventArgs e)
        {

        }

    }
}
