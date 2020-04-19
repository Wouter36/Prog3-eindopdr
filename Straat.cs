using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prog3EindOpdracht
{
    class Straat
    {
        #region properties
        private int StraatId { get; set; }
        public string StraatNaam { get; private set; }
        public Graaf Graaf { get; private set; }
        public Gemeente Gemeente { get; set; }
        private static Dictionary<Gemeente, List<Straat>> gemeentestraten = new Dictionary<Gemeente, List<Straat>>();
        #endregion properties

        #region constructor
        private Straat(int straatID, string naam, Gemeente gemeente)
        {
            this.StraatId = straatID;
            this.StraatNaam = naam;
            this.Gemeente = gemeente;
        }
        #endregion constructor

        #region methods
        public static List<Straat> GetList(Gemeente gemeente)
        {
            if (!gemeentestraten.ContainsKey(gemeente))
            {

                var idindex = Config.indexwrgemeenteid;
                List<string[]> straatGemeenteidsTemp = Config.strListStraatGemeenteIds;
                straatGemeenteidsTemp.RemoveAt(0);
                IEnumerable<string[]> straatGemeenteIds = straatGemeenteidsTemp.
                    Where(e => e[idindex["gemeentenaamid"]] == gemeente.GemeenteId.ToString());

                var index = Config.indexwrstraatnamen;
                List<string[]> strListStraatnamen = CSVReader.ReadCSV(Config.pathStraatNamen, ';');
                List<Straat> straten = new List<Straat>();

                foreach (string[] straatGemeenteId in straatGemeenteIds)
                {
                    string strStraatId = straatGemeenteId[idindex["straatnaamid"]];

                    foreach (string[] straatNaamEnID in strListStraatnamen)
                    {
                        if (strStraatId == straatNaamEnID[index["straatid"]])
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
                gemeentestraten.Add(gemeente, straten);
            }
            return gemeentestraten[gemeente];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Config.StraatLabel);
            sb.Append(Config.Separator);
            sb.Append(StraatNaam);
            sb.Append(Config.Separator);
            sb.Append(StraatId);
            sb.Append(Environment.NewLine);
            sb.Append(Graaf.ToString());
            return sb.ToString();
        }
        #endregion methods
    }
}