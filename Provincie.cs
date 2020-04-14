using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class Provincie
    {
        #region properties
        public int ProvincieID { get; set; }
        private string Naam { get; set; }
        private List<Gemeente> Gemeentes = new List<Gemeente>();
        #endregion properties

        #region constructor
        private Provincie(int provincieID, string naam)
        {
            this.ProvincieID = provincieID;
            this.Naam = naam;
        }
        #endregion constructor


        public static List<Provincie> GetProvincieList()
        {
            var index = Config.indexprovincieinfo;
            string[] ProvincieIdsVlaams = Config.vlaamseProvincieIds;
            List<string[]> strArrayProvincies = Config.Provincies;
            List<Provincie> provincies = new List<Provincie>();

            foreach (string stringId in ProvincieIdsVlaams)
            {
                string provincieNaam = 
                    strArrayProvincies.Where(p => p[index["taalcode"]] == Config.TaalCode && 
                    p[index["provincieid"]] == stringId).Select(p => p[index["provincienaam"]]).First();

                int provincieId = int.Parse(stringId);
                Provincie provincie = new Provincie(provincieId, provincieNaam);
                provincies.Add(provincie);
            }

            foreach (Provincie p in provincies)
            {
                p.Gemeentes = Gemeente.GetGemeenteList(p);
            }

            return provincies;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[provincie]*");
            sb.Append(Naam);
            sb.Append("*");
            sb.Append(ProvincieID);
            sb.Append(Environment.NewLine);
            foreach (Gemeente gemeente in Gemeentes)
            {
                sb.Append(gemeente.ToString());
            }
            return sb.ToString();
        }
    }
}
