using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenGLSrfReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {       
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnLoadImageClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SRF Files (*.srf)|*.srf|RAW Files (*.raw)|*.raw";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                SrfFileReader reader = new SrfFileReader(filePath);
                SrfFileData data = reader.ReadSrfFile();
                DisplayImage(data);
            }
        }

        private void DisplayImage(SrfFileData data)
        {
            glControl.RenderImage(data, debugTextBlock);
        }
    }
}