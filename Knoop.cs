using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class Knoop
    {
        private int KnoopID { get; set; }
        private Punt punt { get; set; }

        public Knoop(int knoopID, Punt punt)
        {
            this.KnoopID = knoopID;
            this.punt = punt;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Config.KnoopLabel);
            sb.Append(Config.Separator);
            sb.Append(KnoopID);
            sb.Append(Environment.NewLine);
            sb.Append(punt.ToString());

            return sb.ToString();
        }
    }
}
