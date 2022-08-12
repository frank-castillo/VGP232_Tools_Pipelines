using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Win32;
using System.Xml.Serialization;

using TextureAtlasLib;

using Path = System.IO.Path;
using System.Collections.ObjectModel;

namespace Assignment3
{
    // Code taken from: https://www.wpf-tutorial.com/commands/implementing-custom-commands/
    [XmlRoot("SpritesheetXML")]
    public partial class MainWindow : Window
    {
        [XmlElement("OutputDirectory")] string OutputDirectory = String.Empty;
        [XmlElement("FileName")] string OutputFile = String.Empty;
        [XmlElement("PNGFileName")] string PNGOutputFile = String.Empty;
        [XmlElement("Images")] List<string> ImagePaths = new List<string>();
        [XmlElement("Metadata")] bool includeMetaData = false;

        [XmlIgnore] string fullPath = string.Empty;
        [XmlIgnore] bool fileWasOpened = false;
        [XmlIgnore] ObservableCollection<MyImage> _images = new ObservableCollection<MyImage>();
        [XmlIgnore] Spritesheet _spritesheet = new Spritesheet();
        public ObservableCollection<MyImage> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void MetadataCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            includeMetaData = true;
        }

        private void MetadataCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            includeMetaData = false;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "SpriteSheet"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG (.png)|.png";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                OutputDirectory = Path.GetDirectoryName(dlg.FileName);
                OutputFile = Path.GetFileNameWithoutExtension(dlg.FileName) + ".xml";
                tbOutputDir.Text = OutputDirectory;
                tbOutputFile.Text = Path.GetFileName(dlg.FileName);
                PNGOutputFile = tbOutputFile.Text;
                fileWasOpened = true;
                ProjectName.Text = OutputFile;
                fullPath = OutputDirectory + OutputFile;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PNG Files(*.png)|*.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Images Selector";
            Nullable<bool> result = openFileDialog.ShowDialog(); ;

            if (result == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    ImagePaths.Add(file);

                    BitmapImage bitmap = new BitmapImage();

                    try
                    {
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(file, UriKind.Absolute);
                        bitmap.EndInit();
                    }
                    catch (Exception i)
                    {
                        MessageBox.Show("Loading Image failed:" + i.Message);
                        continue;
                    }

                    _images.Add(new MyImage(bitmap, file));
                }

                ImagesBox.ItemsSource = _images;
                tbColumns.Text = ImagesBox.Items.Count.ToString();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Remove();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(tbColumns.Text, out int columns);
            _spritesheet.Columns = columns;
            _spritesheet.OutputFile = tbOutputFile.Text;
            _spritesheet.OutputDirectory = OutputDirectory;
            _spritesheet.IncludeMetaData = includeMetaData;
            _spritesheet.InputPaths = ImagePaths;

            _spritesheet.Generate(true);

            string message = (includeMetaData == true) ? "SpriteSheet and metadata file created. Open the SpriteSheet file?" : "SpriteSheet created. Open the SpriteSheet file?";
            string caption = "SpriteSheet created.";
            MessageBoxButton button = MessageBoxButton.YesNo;
            var result = MessageBox.Show(message, caption, button, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = @$"{OutputDirectory}";
                    process.Start();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        // Keyboard Commands
        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(fileWasOpened)
            {
                string message = "Would you like to save your changes?";
                string caption = "Open File detected.";
                MessageBoxButton button = MessageBoxButton.YesNo;
                var result = MessageBox.Show(message, caption, button, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.None:
                        break;
                    case MessageBoxResult.OK:
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        Save();
                        Reset();
                        break;
                    case MessageBoxResult.No:
                        Reset();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Reset();
            }
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (fileWasOpened)
            {
                string message = "Would you like to save your changes?";
                string caption = "Open File detected.";
                MessageBoxButton button = MessageBoxButton.YesNo;
                var result = MessageBox.Show(message, caption, button, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.None:
                        break;
                    case MessageBoxResult.OK:
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        Save();
                        Open();
                        break;
                    case MessageBoxResult.No:
                        Open();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Open();
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (fileWasOpened)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (fileWasOpened)
            {
                string message = "Would you like to save your changes?";
                string caption = "Open File detected.";
                MessageBoxButton button = MessageBoxButton.YesNo;
                var result = MessageBox.Show(message, caption, button, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.None:
                        break;
                    case MessageBoxResult.OK:
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        Save();
                        Application.Current.Shutdown();
                        break;
                    case MessageBoxResult.No:
                        Reset();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAs();
        }

        private void RemoveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Remove();
        }

        private void RemoveAllCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RemoveAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ImagePaths.Clear();
            _images.Clear();
            ImagesBox.ItemsSource = null;
            ImagesBox.SelectedIndex = -1;
            tbColumns.Text = ImagesBox.Items.Count.ToString();
        }

        private void Reset()
        {
            tbColumns.Text = String.Empty;
            tbOutputDir.Text = String.Empty;
            tbOutputFile.Text = String.Empty;
            includeMetaData = false;
            MetadataCheckbox.IsChecked = false;
            ImagesBox.ItemsSource = null;
            ImagesBox.SelectedIndex = -1;
            ImagePaths.Clear();
            _images.Clear();
            fileWasOpened = false;
            tbColumns.Text = ImagesBox.Items.Count.ToString();
        }

        private void Save()
        {
            try
            {
                FileMode mode = FileMode.Open;

                if (File.Exists((fullPath)))
                {
                    using (FileStream fs = new FileStream(fullPath, mode, FileAccess.ReadWrite))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(MainWindow));
                        xs.Serialize(fs, this);
                    }
                }
                else
                {
                    SaveAs();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Saving file as XML file failed.\nException: " + ex.Message);
                return;
            }
        }

        private void SaveAs()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "SpriteSheet"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML File (.xml)|.xml";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                try
                {
                    fullPath = dlg.FileName;
                    OutputDirectory = Path.GetDirectoryName(dlg.FileName);
                    OutputFile = Path.GetFileName(dlg.FileName);
                    tbOutputDir.Text = OutputDirectory;
                    ProjectName.Text = OutputFile;

                    FileMode mode = FileMode.Create;

                    using (FileStream fs = new FileStream(fullPath, mode, FileAccess.ReadWrite))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(Window));
                        xs.Serialize(fs, this);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Saving file as XML file failed.\nException: " + ex.Message);
                    return;
                }

                fileWasOpened = true;
            }
        }

        private void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files(*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    List<string> validImages = new List<string>();
                    List<string> missingImages = new List<string>();
                    var tempWindow = new MainWindow();
                    tempWindow.Close();

                    try
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(MainWindow));
                        tempWindow = (MainWindow)xs.Deserialize(fileStream);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Loading file failed. Error: " + ex.Message);
                        return;
                    }

                    OutputDirectory = tempWindow.OutputDirectory;
                    OutputFile = tempWindow.OutputFile;
                    includeMetaData = tempWindow.includeMetaData;
                    PNGOutputFile = tempWindow.PNGOutputFile;
                    tbOutputDir.Text = OutputDirectory;
                    tbOutputFile.Text = PNGOutputFile;
                    MetadataCheckbox.IsChecked = includeMetaData;
                    ProjectName.Text = OutputFile;

                    foreach (var image in tempWindow.ImagePaths)
                    {
                        if (!File.Exists(image))
                        {
                            missingImages.Add(image);
                        }
                        else
                        {
                            validImages.Add(image);
                        }
                    }

                    foreach (var image in validImages)
                    {
                        BitmapImage bitmap = new BitmapImage();

                        try
                        {
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(image, UriKind.Absolute);
                            bitmap.EndInit();
                        }
                        catch (Exception i)
                        {
                            MessageBox.Show("Loading Image failed:" + i.Message);
                            continue;
                        }

                        _images.Add(new MyImage(bitmap, image));
                    }

                    if (missingImages.Count > 0)
                    {
                        string missingItems = "Missing Images:\n";
                        for (int i = 0; i < missingImages.Count; i++)
                        {
                            missingItems += missingImages[i] + "\n";
                        }
                        MessageBox.Show(missingItems);
                    }

                    ImagePaths = validImages;
                    ImagesBox.ItemsSource = _images;
                    fileWasOpened = true;
                    tbColumns.Text = ImagesBox.Items.Count.ToString();
                }
            }
        }

        private void Remove()
        {
            if (ImagePaths.Count == 0)
            {
                MessageBox.Show("Your collection is empty. Nothing to remove.");
                return;
            }
            else if (ImagesBox.SelectedIndex == -1)
            {
                MessageBox.Show("No Image is currently selected. Please make a selection  before removing");
                return;
            }

            var selectedImage = _images[ImagesBox.SelectedIndex];
            var selectedPath = ImagePaths[ImagesBox.SelectedIndex];
            ImagePaths.Remove(selectedPath);
            _images.Remove(selectedImage);
            ImagesBox.ItemsSource = _images;
            ImagesBox.SelectedIndex = -1;
            tbColumns.Text = ImagesBox.Items.Count.ToString();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static class CustomCommands
    {
        // Code taken from: https://www.wpf-tutorial.com/commands/implementing-custom-commands/
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
                "Exit",
                "Exit",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );

        //Define more commands here, just like the one above

        public static readonly RoutedUICommand SaveAs = new RoutedUICommand
            (
                "Save As",
                "Save As",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.S, ModifierKeys.Alt)
                }
            );

        public static readonly RoutedUICommand Remove = new RoutedUICommand
            (
                "Remove",
                "Remove",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.X, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand RemoveAll = new RoutedUICommand
            (
                "Remove All",
                "Remove All",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.X, ModifierKeys.Alt)
                }
            );
    }

    public class MyImage
    {
        private ImageSource _image;
        private string _fileName;
        public ImageSource ImageSource
        {
            get { return _image; }
            set { _image = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        public MyImage(ImageSource image, string filename)
        {
            _image = image;
            _fileName = filename;
        }
    }
}
