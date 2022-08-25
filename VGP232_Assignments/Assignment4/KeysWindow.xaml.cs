using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assignment4
{
    /// <summary>
    /// Interaction logic for KeysWindow.xaml
    /// </summary>
    public partial class KeysWindow : Window
    {
        private Crypto.CryptoAlgorithm _algoMode = Crypto.CryptoAlgorithm.None;

        public void SetUpWindow()
        {
            _algoMode = Crypto.Instance.Mode;

            switch (_algoMode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    ImportLeftButton.Content = "Import Private Key";
                    ImportRightButton.Content = "Import Public Key";
                    ExportLeftButton.Content = "Export Private Key";
                    ExportRightButton.Content = "Export Public Key";
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    ImportLeftButton.Content = "Import Shared Key";
                    ImportRightButton.Content = "Import IV";
                    ExportLeftButton.Content = "Export shared Key";
                    ExportRightButton.Content = "Export IV";
                    break;
            }
        }

        public KeysWindow()
        {
            InitializeComponent();
        }

        private void ImportLeftButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_algoMode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    RSAImportPrivateKey();
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    AESImportSharedKey();
                    break;
            }
        }

        private void ImportRightButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_algoMode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    RSAImportPublicKey();
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    AESImportIV();
                    break;
            }
        }

        private void ExportLeftButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_algoMode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    RSAExportPrivateKey();
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    AESExportSharedKey();
                    break;
            }
        }

        private void ExportRightButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_algoMode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    RSAExportPublicKey();
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    AESExportIV();
                    break;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            EncryptionToolWindow _encryption = new EncryptionToolWindow();
            _encryption.SetUpWindow();
            _encryption.Show();
            this.Close();
        }

        private void AESExportSharedKey()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "SharedKey"; // Default file name
            dlg.DefaultExt = ".bin"; // Default file extension
            dlg.Filter = "BIN FILE (.bin)|.bin";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                Crypto.Instance.SaveK1(dlg.FileName);
            }
        }

        private void AESExportIV()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "InitializationVector"; // Default file name
            dlg.DefaultExt = ".bin"; // Default file extension
            dlg.Filter = "BIN FILE (.bin)|.bin";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                Crypto.Instance.SaveK2(dlg.FileName);
            }
        }

        private void RSAExportPrivateKey()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "PrivateKey";
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML (.xml)|.xml";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                Crypto.Instance.SaveK1(dlg.FileName);
            }
        }

        private void RSAExportPublicKey()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "PublicKey";
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML (.xml)|.xml";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                Crypto.Instance.SaveK2(dlg.FileName);
            }
        }

        private void AESImportSharedKey()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BINARY Files(*.bin)|*.bin";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Title = "Shared Key Selector";
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                Crypto.Instance.LoadK1(openFileDialog.FileName);
            }
        }

        private void AESImportIV()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BINARY Files(*.bin)|*.bin";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Title = "Initialization Vector Selector";
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                Crypto.Instance.LoadK2(openFileDialog.FileName);
            }
        }

        private void RSAImportPrivateKey()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files(*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Private Key Selector";
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                Crypto.Instance.LoadK1(openFileDialog.FileName);
            }
        }

        private void RSAImportPublicKey()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files(*.xml)|*.xml"; ;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Title = "Public Key Selector";
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                Crypto.Instance.LoadK2(openFileDialog.FileName);
            }
        }
    }
}
