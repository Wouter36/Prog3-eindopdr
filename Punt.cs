using System;
using System.Text;

namespace Prog3EindOpdracht
{
    class Punt
    {
        private decimal X { get; set; }
        private decimal Y { get; set; }

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
