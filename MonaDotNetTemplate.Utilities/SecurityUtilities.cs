using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Utilities
{
    public class SecurityUtilities
    {

        public static string EncodeString(string input, string key)
        {
            // Convert the input and key to byte arrays
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Create a byte array to store the output
            byte[] outputBytes = new byte[inputBytes.Length];

            // Loop through the input bytes
            for (int i = 0; i < inputBytes.Length; i++)
            {
                // XOR the input byte with the corresponding key byte
                outputBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            // Convert the output bytes to a base64 string
            string output = Convert.ToBase64String(outputBytes);

            // Return the output string
            return output;
        }

        // A function to decode a string with a given key
        public static string DecodeString(string input, string key)
        {
            // Convert the input and key to byte arrays
            byte[] inputBytes = Convert.FromBase64String(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Create a byte array to store the output
            byte[] outputBytes = new byte[inputBytes.Length];

            // Loop through the input bytes
            for (int i = 0; i < inputBytes.Length; i++)
            {
                // XOR the input byte with the corresponding key byte
                outputBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            // Convert the output bytes to a UTF8 string
            string output = Encoding.UTF8.GetString(outputBytes);

            // Return the output string
            return output;
        }

        public static string Encode(string value, string key)
        {
            var sha1 = SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return MD5Hash(sb.ToString(), key);
        }

        private static string MD5Hash(string value, string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            value += key;
            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}
