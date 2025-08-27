using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TBBEgitim.Helpers
{
    public class SHA256HashHelper
    {
        public static byte[] ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }
    }
}