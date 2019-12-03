using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using Path = System.IO.Path;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private enum FileFormat
        {
            PNG,
            JPEG
        }

        private FileFormat imageFormat;
        private string imagePath;
        private const string imageFilter = "Файлы изображений (*.png;*.jpg)|*.png;*.jpg|PNG (*.png*)|*.png|JPEG (*.jpg*)|*.jpg";
        public static readonly RoutedCommand CreateCommand = new RoutedCommand();
        public static readonly RoutedCommand OpenCommand = new RoutedCommand();
        public static readonly RoutedCommand CloseCommand = new RoutedCommand();
        public static readonly RoutedCommand SaveCommand = new RoutedCommand();
        public static readonly RoutedCommand SaveNewCommand = new RoutedCommand();
        public static readonly RoutedCommand PrintCommand = new RoutedCommand();
        public static readonly RoutedCommand HelpCommand = new RoutedCommand();
        public MainWindow()
        {
            CreateCommand.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            OpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            CloseCommand.InputGestures.Add(new KeyGesture(Key.W, ModifierKeys.Control));
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            HelpCommand.InputGestures.Add(new KeyGesture(Key.F1, ModifierKeys.None));
            PrintCommand.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control));
            InitializeComponent();
        }




        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }



        private bool JustChecked;
        private void RB_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton s = (RadioButton)sender;
            JustChecked = true;
        }

        private void RB_Clicked(object sender, RoutedEventArgs e)
        {
            if (JustChecked)
            {
                JustChecked = false;
                e.Handled = true;
                return;
            }
            RadioButton s = (RadioButton)sender;
            if (s.IsChecked == true)
                s.IsChecked = false;
        }

        private void BrushComboBox_Selected(object sender, RoutedEventArgs e)
        {
            TextComboBox comboBox = (TextComboBox)sender;
            Image brushImage = (Image)this.FindName("SelectedBrushImage");

            ContentControl selectedItem = (ContentControl)comboBox.SelectedItem;
            brushImage.Source = ((Image)selectedItem.Content).Source;
        }


        private void LineComboBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedEventArgs e)
        {

        }

        private void LineComboBox_Selected(object sender, SelectionChangedEventArgs e)
        {

        }





        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = imageFilter;
            if (dialog.ShowDialog() == true && this.FindName("ImageToRedact") is Image openImage)
            {

                BitmapImage image = new BitmapImage();

                using (FileStream stream = File.OpenRead(dialog.FileName))
                {

                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }


                openImage.Source = image;
                if (Path.GetExtension(dialog.FileName) == ".png")
                {
                    imageFormat = FileFormat.PNG;
                }
                else
                {
                    imageFormat = FileFormat.JPEG;
                }

                imagePath = dialog.FileName;
            }
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CloseFile();
        }

        private void CloseFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = imageFilter;
            if (dialog.ShowDialog() == true && this.FindName("ImageToRedact") is Image openImage)
            {
                Uri imageUri = new Uri(dialog.FileName, UriKind.Absolute);
                BitmapImage image = new BitmapImage(imageUri);
                openImage.Source = image;

            }
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsImageLoad())
            {
                SaveFile();
            }
            else
            {
                MessageBox.Show("Нечего сохранять!", "Paint Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void SaveFile()
        {

            ImageSource source = ((Image)this.FindName("ImageToRedact")).Source as ImageSource;
            BitmapEncoder encoder = null;



            if (imageFormat == FileFormat.PNG)
            {
                encoder = new PngBitmapEncoder();
            }
            if (imageFormat == FileFormat.JPEG)
            {
                encoder = new JpegBitmapEncoder();
            }

            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)source));
            using (FileStream stream = new FileStream(imagePath, FileMode.Open))
                encoder.Save(stream);





        }


        private void SaveNewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsImageLoad())
            {
                SaveNewFile();
            }
            else
            {
                MessageBox.Show("Нечего сохранять!", "Paint Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveNewFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = imageFilter;
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Успешно сохранено.", "Paint Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }


        private bool IsImageLoad()
        {
            return ((Image)this.FindName("ImageToRedact")).Source is ImageSource;

        }


        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenHelp();
        }

        private void OpenHelp()
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string helpPath = Path.Combine(projectDirectory, "Help", "Paint Help.chm");
            System.Windows.Forms.Help.ShowHelp(null, helpPath);
        }

        private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsImageLoad())
            {
                PrintFile();
            }
            else
            {
                MessageBox.Show("Нечего печататать!", "Paint Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void PrintFile()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                if (this.FindName("ImageToRedact") is Image printImage && printImage.Source is ImageSource)
                {
                    printDialog.PrintVisual(printImage, "Отправка Ваших секретов на печать...");
                }

            }
        }


        private void CreateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CreateFile();
        }

        private void CreateFile()
        {
            MessageBox.Show("Кроме таланта, в искусство желательно вкладывать и деньги.", "Paint Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TextComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextComboBox comboBox = (TextComboBox)sender;
            Image brushImage = (Image)this.FindName("SelectedBrushImage");

            ContentControl selectedItem = (ContentControl)comboBox.SelectedItem;
            brushImage.Source = ((Image)selectedItem.Content).Source;
        }
    }
}
