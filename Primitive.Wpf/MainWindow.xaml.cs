using System.Windows;
using System.Windows.Media.Imaging;

namespace Primitive.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> Shapes = ["Ellipse"];

        public MainWindow()
        {
            InitializeComponent();

            var uri = @"C:\Users\stephen.justice\source\repos\Primitive\Samples\Output.png";
            imgOutput.Source = new BitmapImage(new(uri));
        }
    }
}