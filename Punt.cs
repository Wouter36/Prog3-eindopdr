using System;
using System.Text;

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Config.PuntLabel);
            sb.Append(Config.Separator);
            sb.Append(X);
            sb.Append(Config.Separator);
            sb.Append(Y);
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
    }
}
