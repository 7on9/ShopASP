using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ShopASP.Models.Utility
{
    public class Utility
    {
        public static string PATH_IMG_PRODUCT = "/Images/products/";
        public static string PATH_IMG_PRODUCTS = "/Images/customers/";
        public static string PATH_IMG_EMPLOYEE = "/Images/employees/";

        public static string CorrectPath(string path)
        {
            int start = -1;
            string s = "";
            for (int i = 0; i < path.Length; i++)
            {
                if (start == -1)
                {
                    try
                    {
                        if (path[i] == '\\' && path[i + 1] == 'I' && path[i + 2] == 'm' && path[i + 3] == 'a')
                            start = i;
                    }
                    catch (Exception e)
                    {
                    }
                }
                if (i >= start && start != -1)
                    {
                        if (path[i] == '\\')
                            s += '/';
                        else
                            s += path[i];
                    }
                
            }
            return s;
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}