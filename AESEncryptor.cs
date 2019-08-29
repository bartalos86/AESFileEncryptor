using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

public static class AESEncryptor
    {

      //  private static readonly string IV = "remenykugdteaeneh";//16 char
                                                              
        private static byte[] salt = Encoding.ASCII.GetBytes("lkygtstleszasoeskeszlhfgbrhfydsa");//32

        public static string Encrypt(SecureString password, byte[] textToEncrypt)
        {
            SecureString Key = password;

            for (int i = 0; i < 64 - password.Length; i++)
                Key.AppendChar('i');

            byte[] plaintextbytes = textToEncrypt;
            IntPtr pwPointer = Marshal.SecureStringToBSTR(Key);

            var key = new Rfc2898DeriveBytes(Marshal.PtrToStringBSTR(pwPointer), salt);
            Marshal.ZeroFreeBSTR(pwPointer);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            
            aes.BlockSize = 128;
            aes.KeySize = 256;

            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);


            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            ICryptoTransform cryptot = aes.CreateEncryptor(aes.Key, aes.IV);

            byte[] encryptedbytes = cryptot.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);

            cryptot.Dispose();
            aes.Clear();
            key.Dispose();

            //return encryptedbytes;
           return Convert.ToBase64String(encryptedbytes);

        }

        public static byte[] Decrypt(SecureString password, string encryptedText)
        {

            SecureString Key = password;

            for (int i = 0; i < 64 - password.Length; i++)
                Key.AppendChar('i');

            byte[] encbytes = Convert.FromBase64String(encryptedText);

            IntPtr pwPointer = Marshal.SecureStringToBSTR(Key);

            var key = new Rfc2898DeriveBytes(Marshal.PtrToStringBSTR(pwPointer), salt);
            Marshal.ZeroFreeBSTR(pwPointer);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

            aes.BlockSize = 128;
            aes.KeySize = 256;

            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform cryptot = aes.CreateDecryptor(aes.Key, aes.IV);


             byte[] decryptedbytes = null;
            try{
                 decryptedbytes = cryptot.TransformFinalBlock(encbytes, 0, encbytes.Length);
            }catch(CryptographicException){
                    throw new WrongPasswordException();
            }finally{
                cryptot.Dispose();
                aes.Clear();
                key.Dispose();
            }
            
            
            return decryptedbytes;
        }

        

    }