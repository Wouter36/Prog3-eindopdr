using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class Straat
    {
        #region properties
        public int StraatId { get; private set; }
        public string StraatNaam { get; set; }
        public Graaf Graaf { get; set; }
        public Gemeente Gemeente { get; set; }
        #endregion properties

        #region constructor
        private Straat(int straatID, string naam, Gemeente gemeente)
        {
            this.StraatId = straatID;
            this.StraatNaam = naam;
            this.Gemeente = gemeente;
        }
        #endregion constructor

        public static List<Straat> GetStraatList(Gemeente gemeente)
        {
            var idindex = Utils.indexwrgemeenteid;
            List<string[]> straatGemeenteidsTemp = CSVReader.ReadInCSV(Utils.pathGemeenteStraatID, ';');
            straatGemeenteidsTemp.RemoveAt(0);
            IEnumerable<string[]> straatGemeenteIds = straatGemeenteidsTemp.
                Where(e => e[idindex["gemeentenaamid"]] == gemeente.GemeenteId.ToString());

            var index = Utils.indexwrstraatnamen;
            List<string[]> strListStraatnamen = CSVReader.ReadInCSV(Utils.pathStraatNamen, ';');
            List<Straat> straten = new List<Straat>();

            foreach (string[] straatGemeenteId in straatGemeenteIds)
            {
                string strStraatId = straatGemeenteId[idindex["straatnaamid"]];
                
                foreach (string[] straatNaamEnID in strListStraatnamen)
                {
                    if(strStraatId == straatNaamEnID[index["straatid"]])
                    {
                        int straatId = int.Parse(strStraatId);
                        string straatNaam = straatNaamEnID[index["straatnaam"]].Trim(); // bevat soms whitespace

                        Straat straat = new Straat(straatId, straatNaam, gemeente);
                        straat.Graaf = Graaf.GetInstance(straatId); ;
                        straten.Add(straat);

                        break;
                    }
                }
            }
            return straten;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[straat]*");
            sb.Append(StraatNaam);
            sb.Append("*");
            sb.Append(StraatId);
            sb.Append(Environment.NewLine);
            sb.Append(Graaf.ToString());
            return sb.ToString();
        }
    }
}
