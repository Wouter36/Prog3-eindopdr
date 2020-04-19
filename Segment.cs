using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class Segment
    {
        #region properties
        public int SegmentId { get; private set; }
        public Knoop BeginKnoop { get; set; }
        public Knoop EindKnoop { get; set; }
        public List<Punt> Punten { get; private set; }
        private static Dictionary<int, List<Segment>> straatSegmentenDict = new Dictionary<int, List<Segment>>();
        #endregion properties

        #region constructor
        private Segment(int segmentID, Knoop beginKnoop, Knoop eindKnoop, List<Punt> vertices)
        {
            this.SegmentId = segmentID;
            this.BeginKnoop = beginKnoop;
            this.EindKnoop = eindKnoop;
            this.Punten = vertices;
        }
        #endregion constructor

        #region methods
        private static void InitFullList()
        {
            var index = Config.indexwrdata;
            foreach (string[] strData in Config.strListWRData)
            {
                List<Punt> punten = new List<Punt>();

                string puntenString = strData[index["geo"]];
                puntenString = puntenString.Replace("LINESTRING (", "");
                puntenString = puntenString.Replace(")", "");
                string[] strPunten = puntenString.Split(',');

                foreach (string strPunt in strPunten)
                {
                    string[] xy = strPunt.Split(' ');
                    xy = xy.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    decimal x = Convert.ToDecimal(xy[0], CultureInfo.InvariantCulture);
                    decimal y = Convert.ToDecimal(xy[1], CultureInfo.InvariantCulture);
                    Punt punt = new Punt(x, y);
                    punten.Add(punt);
                }
                int segmentId = int.Parse(strData[index["wegsegmentid"]]);
                int beginknoopId = int.Parse(strData[index["beginwegknoopid"]]);
                Knoop beginKnoop = new Knoop(beginknoopId, punten.First());
                int eindknoopId = int.Parse(strData[index["eindwegknoopid"]]);
                Knoop eindKnoop = new Knoop(eindknoopId, punten.Last());

                Segment segment = new Segment(segmentId, beginKnoop, eindKnoop, punten);

                int straatId = int.Parse(strData[index["rechtsstraatnaamid"]]);
                if (straatSegmentenDict.ContainsKey(straatId))
                {
                    straatSegmentenDict[straatId].Add(segment);
                }
                else
                {
                    List<Segment> segmenten = new List<Segment>();
                    segmenten.Add(segment);
                    straatSegmentenDict.Add(straatId, segmenten);
                }
            }
        }

        public static List<Segment> GetSegmentList(int straatId)
        {
            if (straatSegmentenDict.Count == 0)
            {
                InitFullList();
            }


            if (straatSegmentenDict.ContainsKey(straatId))
            {
                return straatSegmentenDict[straatId];
            }
            else
            {
                return new List<Segment>(); // Todo aanpassen? lijkt niet elegant
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Config.SegmentLabel);
            sb.Append(Config.Separator);
            sb.Append(SegmentId);
            sb.Append(Environment.NewLine);
            sb.Append(BeginKnoop.ToString());
            sb.Append(EindKnoop.ToString());
            foreach (Punt punt in Punten)
            {
                sb.Append(punt.ToString());
            }
            return sb.ToString();
        }
        #endregion methods
    }
}
