using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class MassiveInfoReader
    {
        private static readonly string workingpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "massiveInfo");
        const string firstNamesPath = "firstnames.txt";
        const string lastNamesPath = "lastnames.txt";
        const string streetNamePath = "streetnames.txt";
        const Int32 Buffersize = 128;
        const int MaxLines = 100;
        private static Random Random = new Random();

        private static string GetRandom(string path)
        {
            List<string> list = new List<string>();
            string fullPath = Path.Combine(workingpath, path);
            int lineCount = File.ReadLines(fullPath).Count();
            int skipLines = Random.Next(0, lineCount - MaxLines);
            using (var fileStream = File.OpenRead(fullPath))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, Buffersize))
                {
                    string line;
                    for (int i = 0; i < skipLines; i++) 
                    {
                        if (streamReader.ReadLine() == null) break;
                    }
                    for (int i = 0; i <= MaxLines; i++)
                    {
                        line = streamReader.ReadLine();
                        if (line == null) break;
                        list.Add(line);
                    }
                }
            }
            return list[Random.Next(list.Count - 1)];
        }

        public static string GetRandomFirstName()
        {
            return GetRandom(firstNamesPath);
        }

        public static string GetRandomLastName() 
        {
            return GetRandom(lastNamesPath);
        }

        public static string GetRandomStreetName() 
        { 
            return GetRandom(streetNamePath); 
        }
    }
}
