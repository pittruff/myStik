using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace MyStik
{
    public class Cryptor
    {
        byte[] key = new byte[] { 12, 34, 23, 56, 23, 45, 12, 65 };
        byte[] key2 = new byte[] { 13, 32, 129, 251, 233, 145, 182, 65 };


        public string Decrypt(string text)
        {
            byte[] textBytes = StringToBytes(text);
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            MemoryStream writeCryp = new MemoryStream();
            CryptoStream crypto = new CryptoStream(writeCryp, desProvider.CreateDecryptor(key, key2), CryptoStreamMode.Write);


            crypto.Write(textBytes, 0, textBytes.Length);
            crypto.FlushFinalBlock();

            byte[] resultBytes = writeCryp.GetBuffer();
            int len = Convert.ToInt32(writeCryp.Length);
            writeCryp.Close();
            crypto.Close();
            try
            {
                crypto.Dispose();
            }
            catch { }


            string result = "";
            for (int i = 0; i < len; i++)
                result = result.Insert(result.Length, Convert.ToChar(resultBytes[i]).ToString());
            return result;



        }
        public string Encrypt(string text)
        {
            byte[] textBytes = StringToBytes(text);
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            MemoryStream writeClear = new MemoryStream();
            CryptoStream crypto = new CryptoStream(writeClear, desProvider.CreateEncryptor(key, key2), CryptoStreamMode.Write);


            crypto.Write(textBytes, 0, textBytes.Length);
            crypto.FlushFinalBlock();

            byte[] resultBytes = writeClear.GetBuffer();
            int len = Convert.ToInt32(writeClear.Length);
            writeClear.Close();
            try{
            crypto.Dispose();
            }
            catch { }

            string result = "";
            for (int i = 0; i < len; i++)
                result = result.Insert(result.Length, Convert.ToChar(resultBytes[i]).ToString());
            return result;
        }

        private byte[] StringToBytes(string text)
        {
            byte[] result = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
                result[i] = Convert.ToByte(text[i]);
            return result;
        }
    }
}