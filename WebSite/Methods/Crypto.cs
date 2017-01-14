using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebSite.Methods
{
    public class Crypto
    {
        public string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        public string CryptoOrDecrypto(string text, bool encrypt)
        {
            dynamic Cl = new ExpandoObject();
            Cl.PasswordHash = "P@@Sw0rd";
            Cl.SaltKey = "s!lR2ed";
            Cl.VIKey = "@^11xd%^&#3cdaawsr";
            Cl.EncryptionKey = "Asd123";
            Cl.Encrypt = new Func<string, string>(
                (string EncryptStr) =>
                {

                    byte[] clearBytes = Encoding.Unicode.GetBytes(EncryptStr);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Cl.EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(clearBytes, 0, clearBytes.Length);
                                cs.Close();
                            }
                            EncryptStr = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                    return EncryptStr;


                });
            Cl.Decrypt = new Func<string, string>(

                (string DecryptString) =>
                {
                    DecryptString = DecryptString.Replace(" ", "+");
                    byte[] cipherBytes = Convert.FromBase64String(DecryptString);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Cl.EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            DecryptString = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                    return DecryptString;
                });

            if (encrypt == false)
            {
                return Cl.Decrypt(text);
            }
            else
            {
                return Cl.Encrypt(text);
            }
        }
    }
}