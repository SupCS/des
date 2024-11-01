using Avalonia.Controls;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DESApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

private void EncryptButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
{
    try
    {
        string algorithm = AlgorithmComboBox.SelectedItem?.ToString();
        string modeText = ModeComboBox.SelectedItem?.ToString();
        CipherMode mode = modeText == "CBC" ? CipherMode.CBC : CipherMode.ECB;
        string plainText = InputTextBox.Text;

        ICryptoSystem cryptoSystem = CryptoSystemFactory.CreateCryptoSystem(algorithm);
        byte[] encryptedData = cryptoSystem.Encrypt(plainText, mode);
        string encryptedText = Convert.ToBase64String(encryptedData);

        // Виводимо зашифрований текст у ResultTextBox
        ResultTextBox.Text = encryptedText;
    }
    catch (Exception ex)
    {
        ResultTextBox.Text = $"Error: {ex.Message}";
    }
}

private void DecryptButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
{
    try
    {
        string algorithm = AlgorithmComboBox.SelectedItem?.ToString();
        string modeText = ModeComboBox.SelectedItem?.ToString();
        CipherMode mode = modeText == "CBC" ? CipherMode.CBC : CipherMode.ECB;

        // Беремо зашифрований текст із InputTextBox для розшифрування
        byte[] encryptedData = Convert.FromBase64String(InputTextBox.Text);
        ICryptoSystem cryptoSystem = CryptoSystemFactory.CreateCryptoSystem(algorithm);
        string decryptedText = cryptoSystem.Decrypt(encryptedData, mode);

        // Виводимо розшифрований текст у ResultTextBox
        ResultTextBox.Text = decryptedText;
    }
    catch (Exception ex)
    {
        ResultTextBox.Text = $"Error: {ex.Message}";
    }
}



    }

    public interface ICryptoSystem
    {
        byte[] Encrypt(string plainText, CipherMode mode);
        string Decrypt(byte[] encryptedData, CipherMode mode);
    }

    public class DESCryptoSystem : ICryptoSystem
    {
        private readonly byte[] key = Encoding.ASCII.GetBytes("ABCDEFGH");
        private readonly byte[] iv = Encoding.ASCII.GetBytes("ABCDEFGH");

        public byte[] Encrypt(string plainText, CipherMode mode)
        {
            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                desProvider.Key = key;
                desProvider.IV = iv;
                desProvider.Mode = mode;

                byte[] data = Encoding.ASCII.GetBytes(plainText);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public string Decrypt(byte[] encryptedData, CipherMode mode)
        {
            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                desProvider.Key = key;
                desProvider.IV = iv;
                desProvider.Mode = mode;

                using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    public class TripleDESCryptoSystem : ICryptoSystem
    {
        private readonly byte[] key = Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWX");
        private readonly byte[] iv = Encoding.ASCII.GetBytes("ABCDEFGH");

        public byte[] Encrypt(string plainText, CipherMode mode)
        {
            using (TripleDESCryptoServiceProvider tripleDesProvider = new TripleDESCryptoServiceProvider())
            {
                tripleDesProvider.Key = key;
                tripleDesProvider.IV = iv;
                tripleDesProvider.Mode = mode;

                byte[] data = Encoding.ASCII.GetBytes(plainText);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesProvider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public string Decrypt(byte[] encryptedData, CipherMode mode)
        {
            using (TripleDESCryptoServiceProvider tripleDesProvider = new TripleDESCryptoServiceProvider())
            {
                tripleDesProvider.Key = key;
                tripleDesProvider.IV = iv;
                tripleDesProvider.Mode = mode;

                using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesProvider.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    public class AESCryptoSystem : ICryptoSystem
    {
        private readonly byte[] key = Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOP");
        private readonly byte[] iv = Encoding.ASCII.GetBytes("1234567890123456");

        public byte[] Encrypt(string plainText, CipherMode mode)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = key;
                aesProvider.IV = iv;
                aesProvider.Mode = mode;

                byte[] data = Encoding.ASCII.GetBytes(plainText);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesProvider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public string Decrypt(byte[] encryptedData, CipherMode mode)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = key;
                aesProvider.IV = iv;
                aesProvider.Mode = mode;

                using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesProvider.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    public static class CryptoSystemFactory
    {
        public static ICryptoSystem CreateCryptoSystem(string algorithm)
        {
            return algorithm switch
            {
                "TripleDES" => new TripleDESCryptoSystem(),
                "AES" => new AESCryptoSystem(),
                _ => new DESCryptoSystem(),
            };
        }
    }
}
