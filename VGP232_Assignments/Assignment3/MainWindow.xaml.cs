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
    public partial class MainWindow : Window
    {
        private Spritesheet _spriteSheet = new Spritesheet();
        bool includeMetaData = false;

        string fullPath = string.Empty;
        bool fileWasOpened = false;
        List<Image> _images = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _spriteSheet.InitializeSheet();
        }

        private void MetadataCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            includeMetaData = true;
            _spriteSheet.IncludeMetaData = includeMetaData;
        }

        private void MetadataCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            includeMetaData = false;
            _spriteSheet.IncludeMetaData = includeMetaData;
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
                _spriteSheet.OutputDirectory = Path.GetDirectoryName(dlg.FileName);
                _spriteSheet.OutputFile = Path.GetFileNameWithoutExtension(dlg.FileName) + ".xml";
                tbOutputDir.Text = _spriteSheet.OutputDirectory;
                tbOutputFile.Text = Path.GetFileName(dlg.FileName);
                _spriteSheet.PNGOutputFile = tbOutputFile.Text;
                fileWasOpened = true;
                ProjectName.Text = _spriteSheet.OutputFile;
                fullPath = _spriteSheet.OutputDirectory + _spriteSheet.OutputFile;
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
                    _spriteSheet.InputPaths.Add(file);
                    Image image = new Image();

                    try
                    {
                        image.Source = new BitmapImage(new Uri(file));
                    }
                    catch (Exception i)
                    {
                        MessageBox.Show("Loading Image failed:" + i.Message);
                        continue;
                    }

                    _images.Add(image);
                }

                ImagesBox.DataContext = _images;
                ImagesBox.ItemsSource = _images;
                tbColumns.Text = ImagesBox.Items.Count.ToString();
                _spriteSheet.Columns = ImagesBox.Items.Count;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Remove();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(tbColumns.Text, out int columns);
            _spriteSheet.Columns = columns;
            _spriteSheet.OutputFile = tbOutputFile.Text;
            _spriteSheet.OutputDirectory = _spriteSheet.OutputDirectory;
            _spriteSheet.IncludeMetaData = includeMetaData;
            _spriteSheet.InputPaths = _spriteSheet.InputPaths;

            _spriteSheet.Generate(true);

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
                    process.StartInfo.FileName = @$"{_spriteSheet.OutputDirectory}";
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
            if (_spriteSheet.InputPaths.Count < 1)
                e.CanExecute = false;
            else if (_spriteSheet.InputPaths.Count >= 1)
                e.CanExecute = true;
        }

        private void RemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Remove();
        }

        private void RemoveAllCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_spriteSheet.InputPaths.Count < 1)
                e.CanExecute = false;
            else if (_spriteSheet.InputPaths.Count >= 1)
                e.CanExecute = true;
        }

        private void RemoveAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _spriteSheet.InputPaths.Clear();
            _images.Clear();
            ImagesBox.ItemsSource = null;
            ImagesBox.SelectedIndex = -1;
            tbColumns.Text = ImagesBox.Items.Count.ToString();
            _spriteSheet.Columns = ImagesBox.Items.Count;
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
            _spriteSheet.InputPaths.Clear();
            _spriteSheet.IncludeMetaData = false;
            _spriteSheet.PNGOutputFile = String.Empty;
            _spriteSheet.OutputFile = String.Empty;
            _spriteSheet.OutputDirectory = String.Empty;
            _spriteSheet.Columns = 0;
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
                        XmlSerializer xs = new XmlSerializer(typeof(Spritesheet));
                        xs.Serialize(fs, _spriteSheet);
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
                    _spriteSheet.OutputDirectory = Path.GetDirectoryName(dlg.FileName);
                    _spriteSheet.OutputFile = Path.GetFileName(dlg.FileName);
                    tbOutputDir.Text = _spriteSheet.OutputDirectory;
                    ProjectName.Text = _spriteSheet.OutputFile;

                    FileMode mode = FileMode.Create;

                    using (FileStream fs = new FileStream(fullPath, mode, FileAccess.ReadWrite))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(Spritesheet));
                        xs.Serialize(fs, _spriteSheet);
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
                    Spritesheet tempSheet = new Spritesheet();

                    try
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(Spritesheet));
                        tempSheet = (Spritesheet)xs.Deserialize(fileStream);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Loading file failed. Error: " + ex.Message);
                        return;
                    }

                    //TODO::FIX ALL THIS
                    _spriteSheet.OutputDirectory = tempSheet.OutputDirectory;
                    _spriteSheet.OutputFile = tempSheet.OutputFile;
                    _spriteSheet.IncludeMetaData = tempSheet.IncludeMetaData;
                    _spriteSheet.PNGOutputFile = tempSheet.PNGOutputFile;
                    tbOutputDir.Text = _spriteSheet.OutputDirectory;
                    tbOutputFile.Text = _spriteSheet.PNGOutputFile;
                    MetadataCheckbox.IsChecked = includeMetaData;
                    ProjectName.Text = _spriteSheet.OutputFile;

                    _spriteSheet.InputPaths.Clear();

                    foreach (var image in tempSheet.InputPaths)
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
                        Image newImage = new Image();

                        try
                        {
                            newImage.Source = new BitmapImage(new Uri(image));
                        }
                        catch (Exception i)
                        {
                            MessageBox.Show("Loading Image failed:" + i.Message);
                            continue;
                        }

                        _images.Add(newImage);
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

                    _spriteSheet.InputPaths = validImages;
                    ImagesBox.ItemsSource = _images;
                    fileWasOpened = true;
                    tbColumns.Text = ImagesBox.Items.Count.ToString();
                    _spriteSheet.Columns = ImagesBox.Items.Count;
                }
            }
        }

        private void Remove()
        {
            if (_spriteSheet.InputPaths.Count == 0)
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
            var selectedPath = _spriteSheet.InputPaths[ImagesBox.SelectedIndex];
            _spriteSheet.InputPaths.Remove(selectedPath);
            _images.Remove(selectedImage);
            ImagesBox.ItemsSource = _images;
            ImagesBox.SelectedIndex = -1;
            tbColumns.Text = ImagesBox.Items.Count.ToString();
            _spriteSheet.Columns = ImagesBox.Items.Count;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [XmlRoot("CustomCommand")]
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
}
