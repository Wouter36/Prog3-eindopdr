using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class Rapport
    {
        public static void Create()
        {
           
            foreach(Provincie provincie in Provincie.GetList())
            {
                StringBuilder sb = new StringBuilder();
                int stratenInProvincie = 0;
                foreach (Gemeente gemeente in Gemeente.GetList(provincie))
                {
                    int stratenInGemeente = Straat.GetList(gemeente).Count();
                    sb.Append(gemeente.GemeenteNaam);
                    sb.Append(" ");
                    sb.Append(stratenInGemeente);
                    sb.Append(Environment.NewLine);
                    stratenInProvincie += stratenInGemeente;
                }

                string fileInhoud = provincie.Naam + " " + stratenInProvincie + Environment.NewLine + sb.ToString();
                System.IO.File.WriteAllText($"ProvincieFiles\\rapport_{provincie.Naam}.txt", fileInhoud);
            }
        }
    }
}
