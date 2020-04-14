using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class CSVReader
    {
        public static List<string[]> ReadCSV(string path, char separator)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(path);
            List<string[]> list = new List<string[]>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(separator);
                list.Add(values);
            }
            reader.Close();
            return list;
        }
    }
}
