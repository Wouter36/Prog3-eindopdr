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
        public int GemeenteId { get; private set; }
        public string GemeenteNaam { get; private set; }
        private static Dictionary<Provincie, List<Gemeente>> gemeentes = new Dictionary<Provincie, List<Gemeente>>();

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
        public static List<Gemeente> GetList(Provincie provincie)
        {
            if(!gemeentes.ContainsKey(provincie))
            {
                var provincieindex = Config.indexprovincieinfo;
                var gemeenteindex = Config.indexwrgemeentenaam;
                List<string[]> strListGemeentes = Config.strListGemeentes;
                List<string[]> Provincies = Config.Provincies;

                List<Gemeente> gemeenteList = new List<Gemeente>();

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
                        gemeenteList.Add(gemeente);
                    }
                }
                gemeentes.Add(provincie, gemeenteList);
            }

            return gemeentes[provincie];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Config.GemeenteLabel);
            sb.Append(Config.Separator);
            sb.Append(GemeenteNaam);
            sb.Append(Config.Separator);
            sb.Append(GemeenteId);
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
        #endregion methods
    }
}