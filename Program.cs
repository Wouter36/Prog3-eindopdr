﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prog3EindOpdracht
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Provincie> provincies = new List<Provincie>();

            provincies = Provincie.GetProvincieList();

            foreach (Provincie provincie in provincies)
            {
                System.IO.File.WriteAllText($"ProvincieFiles\\{provincie.Naam}.txt", provincie.ToString());
            }

            Console.Read();
        }
    }
}
