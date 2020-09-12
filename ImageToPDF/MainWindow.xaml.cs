using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageToPDF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HashSet<string> fileList;
        private Regex fileNameRegex = new Regex(@"(.*\.)([a-z]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public MainWindow()
        {
            InitializeComponent();
            fileList = new HashSet<string>();
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Multiselect = true

            };
            var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value == true && openFileDialog.FileNames.Length > 0)
            {
                fileList = new HashSet<string>(openFileDialog.FileNames);
                ConvertToPDFButton.IsEnabled = true;
            }
        }

        private void ConvertToPDFButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var fileName in fileList)
            {
                string newFileName = fileNameRegex.Replace(fileName, m => m.Groups[1].Value + "pdf");
                var document = new PdfDocument();
                
                var page = document.AddPage();
                
                XGraphics graphics = XGraphics.FromPdfPage(page);

                XImage image = XImage.FromFile(fileName);

                graphics.DrawImage(image, 0, 0);

                document.Save(newFileName);
            }
            MessageBox.Show(Properties.Resources.ConversionCompletedMessage);
        }
    }
}
