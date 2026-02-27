using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Primitive.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<string> Shapes = ["Ellipse"];
        private const string OutputPath = @"C:\Users\stephen.justice\source\repos\Primitive\Samples\Output.png";

        public string TargetPath { get; set; } = string.Empty;

        public ObservableCollection<ComboBoxItem> ShapeItems { get; set; }

        public BitmapImage OutputSource { get; set; }

        public MainWindow()
        {
            ShapeItems = new(Shapes.Select(it => new ComboBoxItem() { Content = it }));
            OutputSource = new(new Uri(OutputPath));

            InitializeComponent();
        }
    }
}