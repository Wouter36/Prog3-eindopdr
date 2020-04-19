using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prog3EindOpdracht
{
    class Provincie
    {
        #region properties
        public int ProvincieID { get; set; }
        public string Naam { get; set; }
        public static List<Provincie> provincies = new List<Provincie>();
        #endregion properties

        #region constructor
        private Provincie(int provincieID, string naam)
        {
            this.ProvincieID = provincieID;
            this.Naam = naam;
        }
        #endregion constructor

        #region methods
        internal static List<Provincie> GetList()
        {
            if (provincies.Count == 0)
            {
                var index = Config.indexprovincieinfo;
                string[] ProvincieIdsVlaams = Config.vlaamseProvincieIds;
                List<string[]> strArrayProvincies = Config.Provincies;
                List<Provincie> provincieList = new List<Provincie>();

                foreach (string stringId in ProvincieIdsVlaams)
                {
                    string provincieNaam =
                        strArrayProvincies.Where(p => p[index["taalcode"]] == Config.TaalCode &&
                        p[index["provincieid"]] == stringId).Select(p => p[index["provincienaam"]]).First();

                    int provincieId = int.Parse(stringId);
                    Provincie provincie = new Provincie(provincieId, provincieNaam);
                    provincieList.Add(provincie);
                }

                provincies = provincieList;
            }
            return provincies;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Config.ProvincieLabel);
            sb.Append(Config.Separator);
            sb.Append(Naam);
            sb.Append(Config.Separator);
            sb.Append(ProvincieID);
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
        #endregion methods
    }
}