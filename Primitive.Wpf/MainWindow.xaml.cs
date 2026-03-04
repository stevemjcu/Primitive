using Microsoft.Win32;
using Primitive.Shapes;
using Primitive.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Image = SixLabors.ImageSharp.Image;

namespace Primitive.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly List<string> ShapeEnum = ["Ellipse"];

        #region Input

        public string TargetPath
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(TargetPath));
            }
        } = "...";

        public string AspectRatio { get; set; } = "1:1";

        public int WorkingResolution { get; set; } = 256;

        public int OutputResolution { get; set; } = 512;

        public List<string> ShapeItems { get; set; }

        public int Iterations { get; set; } = 200;

        public int Trials { get; set; } = 100;

        public int Failures { get; set; } = 50;

        #endregion

        #region Output

        public BitmapImage? OutputImage { get; set; } = null;

        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;

        public int Shapes
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Shapes));
            }
        } = 0;

        public float Score { get; set; } = 0;

        #endregion

        private Model? Model = null;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainWindow()
        {
            ShapeItems = [.. ShapeEnum];

            InitializeComponent();
        }

        public void OnUpload(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                TargetPath = dialog.FileName;
            }
        }

        public void OnStep(object sender, RoutedEventArgs e)
        {
            if (Model is null)
            {
                Initialize();
            }

            Model!.Add<Ellipse>(Trials, Failures);
            Shapes++;

            // FIXME: Memory leak

            using var stream = new System.IO.MemoryStream();
            Model.Current.SaveAsBmp(stream);

            OutputImage = new BitmapImage();
            OutputImage.BeginInit();
            OutputImage.StreamSource = stream;
            OutputImage.CacheOption = BitmapCacheOption.OnLoad;
            OutputImage.EndInit();

            OnPropertyChanged(nameof(OutputImage));
        }

        private void Initialize()
        {
            var input = Image.Load<Rgba32>(TargetPath);
            input.Mutate(x => x.Resize(new ResizeOptions { Size = new(WorkingResolution, WorkingResolution) }));
            Model = new Model(input, Helper.AverageColor(input));
        }
    }
}