using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class Knoop
    {
        public int KnoopID { get; private set; }
        public Punt punt { get; set; }

        public Knoop(int knoopID, Punt punt)
        {
            this.KnoopID = knoopID;
            this.punt = punt;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Knoop knoop = obj as Knoop;
            // TODO: write your implementation of Equals() here
            if (this.KnoopID == knoop.KnoopID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return KnoopID.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[knoop]*");
            sb.Append(KnoopID);
            sb.Append(Environment.NewLine);
            sb.Append(punt.ToString());

            return sb.ToString();
        }
    }
}
