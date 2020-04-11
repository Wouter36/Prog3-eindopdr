using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class Graaf
    {
        #region properties
        public int GraafId { get; private set; }
        public Dictionary<Knoop, List<Segment>> KnoopSegmenten { get; set; }
        private static int HighestID;
        private static List<string[]> strListData;
        #endregion properties

        #region constructor
        private Graaf(int graafID, Dictionary<Knoop, List<Segment>> knoopSegmenten)
        {
            this.GraafId = graafID;
            this.KnoopSegmenten = knoopSegmenten;
        }
        #endregion constructor

        public static Graaf GetInstance(int straatId)
        {
            var index = Utils.indexwrdata;

            if (strListData == null)
            {
                strListData = CSVReader.ReadInCSV(Utils.pathWRData, ';');
                strListData = strListData.Where(line => 
                    line[index["linksstraatnaamid"]] != "-9" && line[index["rechtsstraatnaamid"]] != "-9").ToList();
                strListData.RemoveAt(0);
            }

            List<Segment> ontvangenSegmenten = Segment.GetInstanceList(strListData, straatId);
            Dictionary<Knoop, List<Segment>> tempKnoopSegmenten = new Dictionary<Knoop, List<Segment>>();

            foreach(Segment segment in ontvangenSegmenten)
            {
                if (tempKnoopSegmenten.ContainsKey(segment.BeginKnoop))
                {
                    tempKnoopSegmenten[segment.BeginKnoop].Add(segment);
                }
                else
                {
                    List<Segment> segmenten = new List<Segment>();
                    segmenten.Add(segment);
                    tempKnoopSegmenten.Add(segment.BeginKnoop, segmenten);
                }
            }

            HighestID++;
            Graaf graaf = new Graaf(HighestID, tempKnoopSegmenten);
            
            return graaf;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[graaf]*");
            sb.Append(GraafId);
            sb.Append(Environment.NewLine);

            foreach (KeyValuePair<Knoop, List<Segment>> kvp in KnoopSegmenten)
            {

                foreach (Segment segment in kvp.Value)
                {
                    sb.Append(segment.ToString());
                }

            }
            return sb.ToString();
        }
    }
}
