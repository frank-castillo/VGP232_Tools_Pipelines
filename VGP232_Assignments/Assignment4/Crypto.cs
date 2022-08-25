using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Input;
using System.Linq;

namespace Assignment4
{
    public sealed class Crypto
    {
        public enum CryptoAlgorithm { RSA, AES, None }

        private AesCryptoServiceProvider _aes = null;
        private RSACryptoServiceProvider _rsa = null;
        private CryptoAlgorithm _mode = CryptoAlgorithm.None;
        private static Crypto _instance;
        private string _load_save_K1Path = string.Empty;
        private string _load_save_K2Path = string.Empty;
        private string _privateKey = string.Empty;
        private string _publicKey = string.Empty;
        private bool _usePublicKey = false;

        public CryptoAlgorithm Mode { get => _mode; set => _mode = value; }

        public static Crypto Instance { get => InstanceCheck(); }

        private static Crypto InstanceCheck()
        {
            if (_instance == null)
            {
                _instance = new Crypto();
            }

            return _instance;
        }

        public void Initialize(CryptoAlgorithm mode)
        {
            _mode = mode;

            switch (_mode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    _rsa = new RSACryptoServiceProvider();
                    _privateKey = _rsa.ToXmlString(true);
                    _publicKey = _rsa.ToXmlString(false);
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    _aes = new AesCryptoServiceProvider();
                    break;
            }
        }

        public void SaveK1(string path)
        {
            switch (_mode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    try
                    {
                        _privateKey = _rsa.ToXmlString(true);
                        File.WriteAllText(path, _privateKey);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Saving RSA Private Key String", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    try
                    {
                        File.WriteAllBytes(path, _aes.Key);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Saving AES Shared Key String", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    break;
            }

            _load_save_K1Path = path;
        }

        public void SaveK2(string path)
        {
            switch (_mode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    try
                    {
                        _publicKey = _rsa.ToXmlString(false);
                        File.WriteAllText(path, _publicKey);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Saving RSA Private Key String", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    try
                    {
                        File.WriteAllBytes(path, _aes.IV);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Saving AES Shared Key String", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    break;
            }

            _load_save_K2Path = path;
        }

        public void LoadK1(string path)
        {
            switch (_mode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    try
                    {
                        var keyString = File.ReadAllText(path);
                        _rsa.FromXmlString(keyString);
                        _privateKey = _rsa.ToXmlString(true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Loading Private Key file", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    try
                    {
                        _aes.Key = File.ReadAllBytes(path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Loading AES Key file", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    break;
            }

            _load_save_K1Path = path;
        }

        public void LoadK2(string path)
        {
            switch (_mode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    try
                    {
                        var keyString = File.ReadAllText(path);
                        _rsa.FromXmlString(keyString);
                        _publicKey = _rsa.ToXmlString(false);
                        _usePublicKey = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Loading Private Key file", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    try
                    {
                        _aes.IV = File.ReadAllBytes(path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Loading AES Initialization Vector file", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    break;
            }

            _load_save_K2Path = path;
        }

        public byte[] Encrypt(string plainText)
        {
            byte[] returnByte = null;

            switch (_mode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    returnByte = EncryptRSA(plainText);
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    returnByte = EncryptAes(plainText);
                    break;
            }

            return returnByte;
        }

        public byte[] Decrypt(byte[] input)
        {
            byte[] returnByte = null;

            switch (_mode)
            {
                case Crypto.CryptoAlgorithm.RSA:
                    returnByte = DecryptRSA(input);
                    break;
                case Crypto.CryptoAlgorithm.AES:
                    returnByte = DecryptAES(input);
                    break;
            }

            return returnByte;
        }

        private byte[] EncryptRSA(string plainText)
        {
            byte[] cipherBytes;

            if (_usePublicKey)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.FromXmlString(_publicKey);
                    var plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                    cipherBytes = _rsa.Encrypt(plainBytes, false);
                }
            }
            else
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.FromXmlString(_privateKey);
                    var plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                    cipherBytes = _rsa.Encrypt(plainBytes, false);
                }
            }

            return cipherBytes;
        }

        private byte[] DecryptRSA(byte[] input)
        {
            byte[] plain;

            if(_usePublicKey)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.FromXmlString(_publicKey);
                    plain = rsa.Decrypt(input, false);
                }
            }
            else
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.FromXmlString(_privateKey);
                    plain = rsa.Decrypt(input, false);
                }
            }

            return plain;
        }

        private byte[] EncryptAes(string plainText)
        {
            if (_aes.Key == null || _aes.Key.Length <= 0)
            {
                MessageBox.Show("No key available! Please revise the correct key is in use", "Exception thrown: Encrypt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            if (_aes.IV == null || _aes.IV.Length <= 0)
            {
                MessageBox.Show("No Initialization Vector available! Please revise the correct IV is in use", "Exception thrown: Encrypt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            byte[] encrypted;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _aes.Key;
                aesAlg.IV = _aes.IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private byte[] DecryptAES(byte[] input)
        {
            if (input == null || input.Length <= 0)
            {
                MessageBox.Show("Input is empty! Please revise the input byte[]", "Exception thrown: Decrypt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (_aes.Key == null || _aes.Key.Length <= 0)
            {
                MessageBox.Show("No key available! Please revise the correct key is in use", "Exception thrown: Decrypt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (_aes.IV == null || _aes.IV.Length <= 0)
            {
                MessageBox.Show("No IV available! Please revise the correct IV is in use", "Exception thrown: Decrypt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _aes.Key;
                aesAlg.IV = _aes.IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(input))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        var decrypted = new byte[input.Length];
                        var bytesRead = csDecrypt.Read(decrypted, 0, input.Length);
                        return decrypted.Take(bytesRead).ToArray();

                        //csDecrypt.Write(input, 0, input.Length);
                        //csDecrypt.FlushFinalBlock();
                        //var returnArray = msDecrypt.ToArray();
                        //return returnArray;
                    }
                }
            }
        }
    }
}
