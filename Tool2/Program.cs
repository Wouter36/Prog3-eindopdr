using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tool2
{
    class Program
    {
        static void Main(string[] args)
        {
            DbAdd dbadd = new DbAdd();
            dbadd.ReadProvincies();
            Console.WriteLine("Klaar!");
            Console.Read();
        }
    }
}
