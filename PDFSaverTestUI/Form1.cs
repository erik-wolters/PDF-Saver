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
            _converter.Converted += OnConverted;
            _converter.QueueUpdated += OnQueueUpdated;

            listView1.DragDrop += ListView_DragDrop;
            listView1.DragEnter += ListView_DragEnter;
        }

        private void OnConverted(object sender, ConvertedEventArgs args)
        {
            string status = args.Isssuccessfull ? "Info" : "Error";
            AddLog($"{status} - {args.Message}");
        }

        private void OnQueueUpdated(object sender, QueueUpdatedEventArgs args)
        {
            string status = args.Issuccessful ? "Info" : "Error";
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

            foreach (FileInfo fileInfo in _converter.GetQueue())
            {
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
            txtLog.Text += $"{message}{Environment.NewLine}";
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
    }
}