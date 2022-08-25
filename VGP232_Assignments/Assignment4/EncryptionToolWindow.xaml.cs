using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for EncryptionToolWindow.xaml
    /// </summary>
    public partial class EncryptionToolWindow : Window
    {
        private byte[] _encryptedString = null;
        private byte[] _cipherByteArray = null;
        private byte[] _decryptByteArray = null;

        public void SetUpWindow()
        {
            StringToEncrypt.Text = string.Empty;
            StringEncrypted.Text = string.Empty;
            CipherString.Text = string.Empty;
            CipherResultString.Text = string.Empty;
        }

        public EncryptionToolWindow()
        {
            InitializeComponent();
        }

        private void LoadTextButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files(*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Load Text";
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        StringToEncrypt.Text = sr.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Loading Text file", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            _encryptedString = Crypto.Instance.Encrypt(StringToEncrypt.Text);
            string converter = Convert.ToBase64String(_encryptedString);
            StringEncrypted.Text = converter;
        }

        private void LoadCipherButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Binary Files(*.bin)|*.bin";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Title = "Load Cipher Binary File";
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    _cipherByteArray = File.ReadAllBytes(openFileDialog.FileName);
                    string converter = Convert.ToBase64String(_cipherByteArray);
                    CipherString.Text = converter;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Loading Text file", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(Crypto.Instance.Mode == Crypto.CryptoAlgorithm.RSA)
                {
                    var checker = Convert.ToBase64String(_cipherByteArray);
                    var s = checker.Trim().Replace(" ", "+");
                    if (s.Length % 4 > 0)
                        s = s.PadRight(s.Length + 4 - s.Length % 4, '=');
                    byte[] encodedCipherText = Convert.FromBase64String(s);
                    _decryptByteArray = Crypto.Instance.Decrypt(encodedCipherText);
                    CipherResultString.Text = Encoding.UTF8.GetString(_decryptByteArray);
                }
                else
                {
                    _decryptByteArray = Crypto.Instance.Decrypt(_cipherByteArray);
                    CipherResultString.Text = Encoding.UTF8.GetString(_decryptByteArray);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while Decrypting", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
        private void SaveToBinaryFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "EncryptedText";
            dlg.DefaultExt = ".bin";
            dlg.Filter = "Binary file (.bin)|.bin";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                try
                {
                    File.WriteAllBytes(dlg.FileName, _encryptedString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Saving Encrypted String", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }

        private void SaveResultButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Result Text";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text file (.txt)|.txt";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                try
                {
                    FileStream fs = null;

                    // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
                    if (File.Exists((dlg.FileName)))
                    {
                        File.Delete(dlg.FileName);
                        fs = File.Create(dlg.FileName);
                    }

                    using (StreamWriter wr = new StreamWriter(fs))
                    {
                        wr.WriteLine(CipherResultString);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Saving Encrypted String", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }
    }
}