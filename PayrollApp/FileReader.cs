using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    class FileReader
    {
        public static string[] GetData(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            return File.ReadAllLines(path, Encoding.UTF8);
        }

        public static int GetSize(string path)
        {
            if (!File.Exists(path))
            {
                return 0;
            }

            return File.ReadAllLines(path).Length;
        }

        public static bool AddData(string data, string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine(data);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        public static bool AddData(List<string> data, string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                using (StreamWriter writer = File.AppendText(path))
                {
                    foreach (string row in data)
                    {
                        writer.WriteLine(row);
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
    }
}
