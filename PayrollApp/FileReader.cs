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
                    File.Create(path).Close();
                }

                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine(data);
                }

                return true;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }

        public static bool UpdateData(string data, string path, int lineNumber)
        {
            try
            {
                if (!File.Exists(path))
                {
                    // If the file doesn't exist for this call, then this is the wrong method to use.
                    return FileReader.AddData(data, path);
                }

                string[] dataCollection = File.ReadAllLines(path);
                if (dataCollection.Length < lineNumber)
                {
                    // There are no rows with this ID, so we can just add it
                    return FileReader.AddData(data, path);
                }

                dataCollection[lineNumber] = data;
                File.WriteAllLines(path, dataCollection);
                return true;
            }
            catch(UnauthorizedAccessException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }

        public static bool RemoveData(string path, int lineNumber)
        {
            try
            {
                if (!File.Exists(path))
                {
                    // There is nothing to delete
                    return true;
                }

                List<string> dataCollection = File.ReadAllLines(path).ToList<string>();
                if (dataCollection.Count < lineNumber)
                {
                    // There is nothing to delete
                    return true;
                }

                dataCollection.RemoveAt(lineNumber);
                File.WriteAllLines(path, dataCollection);
                return true;
            }
            catch(UnauthorizedAccessException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }

        public static bool AddData(List<string> data, string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }

                using (StreamWriter writer = File.AppendText(path))
                {
                    foreach (string row in data)
                    {
                        writer.WriteLine(row);
                    }
                }

                return true;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }
    }
}
