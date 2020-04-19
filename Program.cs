using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Prog3EindOpdracht
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.strListWRData = Config.InitStrData();
            foreach (Provincie provincie in Provincie.GetList())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(provincie.ToString());
                foreach (Gemeente gemeente in Gemeente.GetList(provincie))
                {
                    sb.Append(gemeente.ToString());
                    foreach (Straat straat in Straat.GetList(gemeente))
                    {
                        sb.Append(straat.ToString());
                    }
                }
                string text = sb.ToString();
                System.IO.File.WriteAllText($"ProvincieFiles\\{provincie.Naam}.txt", text);
            }
            Rapport.Create();

            Console.WriteLine("Klaar!");
            Console.Read();
        }
    }
}
