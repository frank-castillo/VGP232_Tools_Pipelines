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
    public partial class SelectionWindow : Window
    {
        public SelectionWindow()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if(AESButton.IsChecked != true && RSAButton.IsChecked != true)
            {
                MessageBox.Show("No encryption method selected", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (RSAButton.IsChecked == true)
                    Crypto.Instance.Initialize(Crypto.CryptoAlgorithm.RSA);
                else if (AESButton.IsChecked == true)
                    Crypto.Instance.Initialize(Crypto.CryptoAlgorithm.AES);

                KeysWindow _keysWindow = new KeysWindow();
                this.Close();
                _keysWindow.SetUpWindow();
                _keysWindow.Show();
            }
        }
    }
}