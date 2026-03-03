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
        private readonly List<string> ShapeEnum = ["Ellipse"];
        private const string OutputPath = @"C:\Users\stephen.justice\source\repos\Primitive\Samples\Output.png";

        #region Input

        public string TargetPath { get; set; } = string.Empty;

        public int WorkingResolutionX { get; set; } = 256;

        public int WorkingResolutionY { get; set; } = 256;

        public int OutputResolutionX { get; set; } = 512;

        public int OutputResolutionY { get; set; } = 512;

        public ObservableCollection<ComboBoxItem> ShapeItems { get; set; }

        public int Iterations { get; set; } = 200;

        public int Trials { get; set; } = 100;

        public int Failures { get; set; } = 50;

        #endregion

        #region Output

        public BitmapImage OutputSource { get; set; }

        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;

        public int Shapes { get; set; } = 0;

        public float Score { get; set; } = 0;

        #endregion

        public MainWindow()
        {
            ShapeItems = new(ShapeEnum.Select(it => new ComboBoxItem() { Content = it }));
            OutputSource = new(new Uri(OutputPath));

            InitializeComponent();
        }

        private void Step()
        {

        }
    }
}