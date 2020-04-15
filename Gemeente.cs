using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prog3EindOpdracht
{
    class Gemeente
    {
        #region props
        private Provincie Provincie { get; set; }
        public int GemeenteId { get; set; }
        private string GemeenteNaam { get; set; }
        private List<Straat> straten = new List<Straat>();
        #endregion props

        #region constructor
        private Gemeente(int gemeenteId, Provincie provincie, string gemeenteNaam)
        {
            this.GemeenteId = gemeenteId;
            this.Provincie = provincie;
            this.GemeenteNaam = gemeenteNaam;
        }
        #endregion constructor

        #region methods
        public static List<Gemeente> GetGemeenteList(Provincie provincie)
        {
            var provincieindex = Config.indexprovincieinfo;
            var gemeenteindex = Config.indexwrgemeentenaam;
            List<string[]> strListGemeentes = Config.strListGemeentes;
            List<string[]> Provincies = Config.Provincies;

            List<Gemeente> gemeentes = new List<Gemeente>();

            foreach (string[] strGemeente in strListGemeentes)
            {
                bool isInProvincie = Provincies.Any(p => 
                    p[provincieindex["gemeenteid"]] == strGemeente[gemeenteindex["gemeenteid"]] &&
                    p[provincieindex["provincieid"]] == provincie.ProvincieID.ToString());

                if (strGemeente[2] == Config.TaalCode && isInProvincie)
                {
                    int gemeenteId = int.Parse(strGemeente[gemeenteindex["gemeenteid"]]);
                    string gemeenteNaam = strGemeente[gemeenteindex["gemeentenaam"]];
                    gemeenteNaam = gemeenteNaam.Trim(); // kan soms whitespace bevatten
                    Gemeente gemeente = new Gemeente(gemeenteId, provincie, gemeenteNaam);
                    gemeentes.Add(gemeente);
                }
            }
            foreach (Gemeente gemeente in gemeentes)
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
        #endregion methods
    }
}
