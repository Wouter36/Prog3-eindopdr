using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class Punt
    {
        public decimal X { get; private set; }
        public decimal Y { get; private set; }

        public Punt(decimal x, decimal y)
        {
            this.X = x;
            this.Y = y;
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

            // TODO: write your implementation of Equals() here
            throw new NotImplementedException();
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[punt]*");
            sb.Append(X);
            sb.Append("*");
            sb.Append(Y);
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
    }
}
