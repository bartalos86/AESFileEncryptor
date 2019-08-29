using System;
using System.IO;
using System.Security;
using System.Text;

public static class FolderCryptor
{
    public static string CreateFileSafe(string path, SecureString password, string safeName)
    {
        string[] files = Directory.GetFiles(path);
        StreamWriter dataWriter = new StreamWriter(safeName + ".safe");

        foreach (var file in files)
        {
            //File name encryption
            byte[] fileNameBytes = Encoding.Unicode.GetBytes(Path.GetFileName(file));
            string fileName =  AESEncryptor.Encrypt(UniqueHash.SecureHashString("1npr0gramsecretKey"), fileNameBytes);

            System.Console.WriteLine(Path.GetFileName(file));

            byte[] fileData = File.ReadAllBytes(file);
            string encryptedData = AESEncryptor.Encrypt(password, fileData);

            dataWriter.WriteLine(fileName);
            dataWriter.WriteLine(encryptedData);

            dataWriter.Flush();


        }
        System.Console.WriteLine("Befejezve");
        dataWriter.Dispose();

        return "Sucess";
    }

    public static string UnlockFileSafe(string safePath, SecureString password)
    {
        StreamReader dataReader = new StreamReader(safePath);

        string unlockDirectory = Path.GetFileNameWithoutExtension(safePath);



        while (!dataReader.EndOfStream)
        {

            string fileName = dataReader.ReadLine();
            //File name decryption
            string decryptedName = Encoding.Unicode.GetString(AESEncryptor.Decrypt(UniqueHash.SecureHashString("1npr0gramsecretKey"), fileName));
            string data = dataReader.ReadLine();



            string path = unlockDirectory + "\\" + decryptedName.TrimEnd();
            // path = "sample.png";
            try
            {
                byte[] decryptedData = AESEncryptor.Decrypt(password, data);

                if (!Directory.Exists(unlockDirectory))
                    Directory.CreateDirectory(unlockDirectory);

                File.Create(path).Close();
                File.WriteAllBytes(path, decryptedData);
            }
            catch (WrongPasswordException)
            {
                System.Console.WriteLine("A jelszo helytelen");
                return "Failed";
            }

            System.Console.WriteLine("Jelenleg: " + decryptedName);




        }
        System.Console.WriteLine("Befejezve");
        dataReader.Dispose();

        return "Sucbess";
    }
}