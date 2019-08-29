using System;
using System.Security;
using System.Text;
using System.Diagnostics;
namespace Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSpeed();

            System.Console.WriteLine("Udv a SafeMaker 2000-ben");
            System.Console.WriteLine("1. Safe keszitese");
            System.Console.WriteLine("2. Safe kibontasa");

            string resp = Console.ReadLine();
            
            switch (resp)
            {
                case "1":
                    Console.Clear();
                    Console.Write("Fajlok helye (ami megy a safebe): ");
                    string path = Console.ReadLine();
                    Console.Write("Safe neve: ");
                    string name = Console.ReadLine();
                    Console.Write("Safe kodja: ");
                    SecureString pass = ReadPassword();
                    FolderCryptor.CreateFileSafe(path, pass, name);
                    pass.Dispose();
                    break;

                case "2":
                    Console.Clear();
                    Console.Write("Safe helye: ");
                    string safePath = Console.ReadLine();
                    Console.Write("Safe kodja: ");
                    SecureString safePass = ReadPassword();
                    FolderCryptor.UnlockFileSafe(safePath, safePass);
                    safePass.Dispose();
                    break;
            }

            // Task.Factory.StartNew(() => FolderCryptor.CreateFileSafe("test","pass","Titkos")).Wait();
            //Task.Factory.StartNew(() => FolderCryptor.UnlockFileSafe("Titkos.safe","pass")).Wait();
        }

        public static SecureString ReadPassword()
        {
            ConsoleKeyInfo info = Console.ReadKey(true);
            SecureString pass = new SecureString();

            while (info.Key != ConsoleKey.Enter)
            {

                if (info.Key == ConsoleKey.Backspace)
                {
                    if (pass.Length > 0)
                    {
                        pass.RemoveAt(pass.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }

                }
                else
                {
                    Console.Write("*");
                    pass.AppendChar(info.KeyChar);
                }

                info = Console.ReadKey(true);
            }
            System.Console.WriteLine();
            return pass;

        }


        private static void TestSpeed()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            string path = "C:\\Users\\barta\\Desktop\\Camera";
            string safeName = "Camera";
            SecureString password = UniqueHash.SecureHashString("verysecurepass");
            FolderCryptor.CreateFileSafe(path, password, safeName);
            FolderCryptor.UnlockFileSafe("Camera.safe",password);
            TimeSpan ts = stopwatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            System.Console.WriteLine(elapsedTime);
        }


    }
}
