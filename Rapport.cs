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
                    sb.Append(GetLangsteStraat(gemeente));
                    sb.Append(Environment.NewLine);
                    stratenInProvincie += stratenInGemeente;
                }

                string fileInhoud = provincie.Naam + " " + stratenInProvincie + Environment.NewLine + sb.ToString();
                System.IO.File.WriteAllText($"ProvincieFiles\\rapport_{provincie.Naam}.txt", fileInhoud);
            }
        }

        private static string GetLangsteStraat(Gemeente gemeente)
        {
            decimal lengteLangsteStraat = -1;
            string naamLangsteStraat = "";

            decimal lengteKortsteStraat = -1;
            string naamKortsteStraat = "";

            foreach (Straat straat in Straat.GetList(gemeente))
            {
                decimal lengte = 0;
                foreach (List<Segment> segmenten in straat.Graaf.KnoopSegmenten.Values)
                {
                    foreach (Segment segment in segmenten)
                    {
                        List<Punt> punten = segment.Punten;
                        decimal x1 = punten[0].X;
                        decimal y1 = punten[0].Y;
                        decimal x2;
                        decimal y2;
                        for(int i = 1; i < punten.Count; i++)
                        {
                            x2 = punten[i].X;
                            y2 = punten[i].Y;
                            lengte += ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)) * ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));
                        }
                    }
                }
                if(lengteLangsteStraat < lengte)
                {
                    lengteLangsteStraat = lengte;
                    naamLangsteStraat = straat.StraatNaam;
                }
                if(lengteKortsteStraat == -1 && lengte > 0)
                {
                    lengteKortsteStraat = lengte;
                    naamKortsteStraat = straat.StraatNaam;
                }
                else if (lengteKortsteStraat > lengte && lengte > 0)
                {
                    lengteKortsteStraat = lengte;
                    naamKortsteStraat = straat.StraatNaam;
                }
            }
            string langsteStraatStr = $"Langste straat: {naamLangsteStraat} Lengte: {lengteLangsteStraat}";
            string kortsteStraatStr = $"Kortste straat: {naamKortsteStraat} {lengteKortsteStraat}";
            return langsteStraatStr + Environment.NewLine + kortsteStraatStr;
        }
    }
}
