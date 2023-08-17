using PDFSaverTestUI.Models;
using PDFSaverLibrary;
using PDFSaverLibrary.Events;

namespace PDFSaverTestUI
{
    public partial class Form1 : Form
    {
        private readonly Converter _converter;

        public Form1()
        {
            InitializeComponent();

            _converter = new();
            _converter.ConverterUpdated += OnConverterUpdated;

            listView1.DragDrop += ListView_DragDrop!;
            listView1.DragEnter += ListView_DragEnter!;
        }

        private void OnConverterUpdated(object sender, ConverterUpdatedEventArgs args)
        {
            string status = args.IsSuccessful ? "Info" : "Error";
            AddLog($"{status} - {args.Message}");
        }

        private void ListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    FileInfo fileInfo = new(file);

                    _converter.AddToQueue(fileInfo);
                    UpdateView();
                }
            }
        }

        private void UpdateView()
        {
            listView1.Items.Clear();

            foreach (string file in _converter.GetQueue())
            {
                FileInfo fileInfo = new FileInfo(file);

                ListViewItem lvi = new ListViewItem(new string[] {
                        Directory.GetParent(fileInfo.FullName)!.ToString(),
                        fileInfo.Name
                    });

                listView1.Items.Add(lvi);
            }
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void AddLog(string message)
        {
            txtLog.AppendText($"{message}{Environment.NewLine}");
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            _converter.Convert();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrEmpty(
                folderBrowserDialog.SelectedPath))
            {
                txtOutputDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void txtOutputDir_TextChanged(object sender, EventArgs e)
        {
            _converter.SetOutputDir(txtOutputDir.Text);
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new())
            {
                ofd.InitialDirectory = Environment.GetFolderPath(
                    Environment.SpecialFolder.UserProfile
                    );

                ofd.Filter = "Word bestanden (*.docx)|*.docx|Oude Word bestanden (*.doc)|*.doc";

                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fileInfo = new FileInfo(ofd.FileName);

                    _converter.AddToQueue(fileInfo);
                    UpdateView();
                }
            }
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                FileInfo fileInfo = new(@$"{lvi.Text}\{lvi.SubItems[1].Text}");

                _converter.RemoveFromQueue(fileInfo);

                UpdateView();
            }
        }
    }
}