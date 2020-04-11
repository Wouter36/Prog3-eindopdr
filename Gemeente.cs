using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prog3EindOpdracht
{
    class Gemeente
    {
        #region props
        List<Straat> straten = new List<Straat>();
        public int GemeenteId { get; private set; }
        public Provincie Provincie { get; set; }
        public string GemeenteNaam { get; set; }
        #endregion props

        #region constructor
        private Gemeente(int gemeenteId, Provincie provincie, string gemeenteNaam)
        {
            this.GemeenteId = gemeenteId;
            this.Provincie = provincie;
            this.GemeenteNaam = gemeenteNaam;
        }
        #endregion constructor

        public static List<Gemeente> GetGemeenteList(Provincie provincie)
        {
            var provincieindex = Utils.indexprovincieinfo;
            var gemeenteindex = Utils.indexwrgemeentenaam;
            List<string[]> strListGemeentes = Utils.strListGemeentes;
            List<string[]> Provincies = Utils.Provincies;

            List<Gemeente> gemeentes = new List<Gemeente>();

            foreach (string[] strGemeente in strListGemeentes)
            {
                bool isInProvincie = 
                    Provincies.Any(p => p[provincieindex["gemeenteid"]] == strGemeente[1] &&
                    p[provincieindex["provincieid"]] == provincie.ProvincieID.ToString());

                if (strGemeente[2] == Utils.TaalCode && isInProvincie)
                {
                    int gemeenteId = int.Parse(strGemeente[gemeenteindex["gemeenteid"]]);
                    string gemeenteNaam = strGemeente[gemeenteindex["gemeentenaam"]];
                    gemeenteNaam = gemeenteNaam.Trim(); // kan soms whitespace bevatten
                    Gemeente gemeente = new Gemeente(gemeenteId, provincie, gemeenteNaam);
                    gemeentes.Add(gemeente);
                }
            }

            foreach(Gemeente gemeente in gemeentes)
            {
                gemeente.straten = Straat.GetStraatList(gemeente);
            }

            return gemeentes;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[gemeente]*");
            sb.Append(GemeenteNaam);
            sb.Append("*");
            sb.Append(GemeenteId);

            sb.Append(Environment.NewLine);
            foreach (Straat straat in straten)
            {
                sb.Append(straat.ToString());
            }

            return sb.ToString();
        }
    }
}
