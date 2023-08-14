namespace PDFSaverTestUI
{
    public partial class Form1 : Form
    {

        bool privateDrag;
        public Form1()
        {
            InitializeComponent();

            listView1.DragDrop += ListView_DragDrop;
            listView1.DragEnter += ListView_DragEnter;
        }

        private void ListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    FileInfo fileInfo = new(file);

                    ListViewItem lvi = new ListViewItem(new string[] {
                        Directory.GetParent(fileInfo.FullName)!.ToString(),
                        fileInfo.Name
                    });

                    listView1.Items.Add(lvi);
                }
            }
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }
    }
}