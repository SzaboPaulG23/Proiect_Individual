using CatalogOnline2.Data;
using ClosedXML.Excel;
using System.Security.Cryptography;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace CatalogOnline2.Utils
{
    public class Utils
    {
        private readonly CatalogOnline2Context _context;


        public Utils(CatalogOnline2Context context)
        {
            _context = context;
        }

        public static string[,] readExcel()
        {

            string fileName = "C:\\Users\\Paul\\Desktop\\upload\\TEST.xlsx";
            var workbook = new XLWorkbook(fileName);
            var ws1 = workbook.Worksheet(1);
            string value = "start";
            int i = 1;
            string[,] output = new string[100, 5];
            while (!string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var row = ws1.Row(i);
                    var cell = row.Cell(1);
                    output[i,0] = cell.GetValue<string>();
                    cell = row.Cell(2);
                    output[i,1] = cell.GetValue<string>(); value = cell.GetValue<string>();
                    cell = row.Cell(3);
                    output[i,2] = cell.GetValue<string>();
                    cell = row.Cell(4);
                    output[i, 3] = cell.GetValue<string>();
                    cell = row.Cell(5);
                    output[i, 4] = cell.GetValue<string>();
                    i++;
                    value = cell.GetValue<string>();
                }
            }

            return output;
        }

        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

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

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static string passwordGen(ref string password)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                Random random = new Random();
                string key = "jI5N0dqT6y6LfoP5tY9Fpg==";
                string IV = "EHTXEj7z2pQ5Myzc6i31zQ==";
                aesAlgorithm.Key = Convert.FromBase64String(key);
                aesAlgorithm.IV = Convert.FromBase64String(IV);
                char[] vocale = { 'a', 'e', 'i', 'o', 'u' };
                char[] consoane = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v' };
                string parola;
                int seed;
                seed = random.Next(consoane.Length);
                parola = consoane[seed].ToString();
                for (int i = 0; i < 9; i++)
                {
                    if (i % 2 == 0)
                    {
                        seed = random.Next(vocale.Length);
                        parola = parola + vocale[seed];
                    }
                    else
                    {
                        seed = random.Next(consoane.Length);
                        parola = parola + consoane[seed];
                    }
                }
                seed = random.Next(0, 10);
                parola = parola + seed;
                seed = random.Next(0, 10);
                parola = parola + seed;
                password = parola;

                byte[] encrypted = null;
                encrypted = Utils.EncryptStringToBytes_Aes(parola, aesAlgorithm.Key, aesAlgorithm.IV);
                parola = Convert.ToBase64String(encrypted);
                return parola;
            }
        }
        public static void testMail(string emailText, string emailReceiver)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("CatalogOnline", "szabopaulg@gmail.com"));
            email.To.Add(new MailboxAddress("PaulSzabo", $"{emailReceiver}"));
            email.Subject = "Your CatalogOnline Account";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailText
            };


            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("szabopaulg@gmail.com", "mcqyekplyslxorll");

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }


    }
}
