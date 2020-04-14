using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;

namespace tool2
{
    class DbAdd
    {
        private DataSet Ds { get; set; }

        public DbAdd()
        {
            Ds = new DataSet();
            Ds.CaseSensitive = false;

            DataTable provTbl = new DataTable("provincie");
            provTbl.Columns.Add(new DataColumn("ProvincieID", typeof(int)));
            provTbl.Columns.Add(new DataColumn("ProvincieNaam", typeof(string)));
            Ds.Tables.Add(provTbl);

            DataTable gemeenteTbl = new DataTable("gemeente");
            gemeenteTbl.Columns.Add(new DataColumn("GemeenteID", typeof(int)));
            gemeenteTbl.Columns.Add(new DataColumn("GemeenteNaam", typeof(string)));
            gemeenteTbl.Columns.Add(new DataColumn("ProvincieID", typeof(int)));
            Ds.Tables.Add(gemeenteTbl);

            DataTable straatTbl = new DataTable("straat");
            straatTbl.Columns.Add(new DataColumn("StraatID", typeof(int)));
            straatTbl.Columns.Add(new DataColumn("StraatNaam", typeof(string)));
            straatTbl.Columns.Add(new DataColumn("GemeenteID", typeof(int)));
            Ds.Tables.Add(straatTbl);

            DataTable graafTbl = new DataTable("graaf");
            graafTbl.Columns.Add(new DataColumn("GraafID", typeof(int)));
            graafTbl.Columns.Add(new DataColumn("StraatID", typeof(int)));
            Ds.Tables.Add(graafTbl);

            DataTable SegmentTbl = new DataTable("segment");
            SegmentTbl.Columns.Add(new DataColumn("SegmentID", typeof(int)));
            SegmentTbl.Columns.Add(new DataColumn("GraafID", typeof(int)));
            Ds.Tables.Add(SegmentTbl);

            DataTable KnoopTbl = new DataTable("knoop");
            KnoopTbl.Columns.Add(new DataColumn("KnoopID", typeof(int)));
            KnoopTbl.Columns.Add(new DataColumn("SegmentID", typeof(int)));
            KnoopTbl.PrimaryKey = new DataColumn[] { KnoopTbl.Columns["KnoopID"] };
            Ds.Tables.Add(KnoopTbl);

            DataTable PuntTbl = new DataTable("punt");
            PuntTbl.Columns.Add(new DataColumn("X", typeof(decimal)));
            PuntTbl.Columns.Add(new DataColumn("Y", typeof(decimal)));
            PuntTbl.Columns.Add(new DataColumn("SegmentID", typeof(int)));
            PuntTbl.Columns.Add(new DataColumn("KnoopID", typeof(int)));
            Ds.Tables.Add(PuntTbl);
        }

        public void readProvincies()
        {
            DirectoryInfo d = new DirectoryInfo(@"ProvincieFiles");
            FileInfo[] Files = d.GetFiles("*.txt");

            foreach (FileInfo file in Files)
            {
                readProvincie(file.FullName);
            }
            ToDb();
        }
        
        private void readProvincie(string path)
        {
            int werkProvincieId = -1;
            int werkGemeenteId = -1;
            int werkStraatId = -1;
            int werkGraafId = -1;
            int werkSegmentId = -1;
            int werkKnoopId = -1;

            System.IO.StreamReader sReader = new System.IO.StreamReader(path);
            string line;

            while ((line = sReader.ReadLine()) != null)
            {
                string[] elemnts = line.Split('*');
                elemnts = elemnts.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                if (elemnts[0].StartsWith("[provincie]"))
                {
                    string provincieNaam = elemnts[1];
                    int provincieID = int.Parse(elemnts[2]);
                    Ds.Tables["provincie"].Rows.Add(provincieID, provincieNaam);
                    werkProvincieId = provincieID;
                }
                else if (elemnts[0].StartsWith("[gemeente]"))
                {
                    string gemeenteNaam = elemnts[1];
                    int gemeenteID = int.Parse(elemnts[2]);
                    Ds.Tables["gemeente"].Rows.Add(gemeenteID, gemeenteNaam, werkProvincieId);
                    werkGemeenteId = gemeenteID;
                }
                else if (elemnts[0].StartsWith("[straat]"))
                {
                    string straatNaam = elemnts[1];
                    int straatID = int.Parse(elemnts[2]);
                    Ds.Tables["straat"].Rows.Add(straatID, straatNaam, werkGemeenteId);
                    werkStraatId = straatID;
                }
                else if (elemnts[0].StartsWith("[graaf]"))
                {
                    int graafID = int.Parse(elemnts[1]);
                    Ds.Tables["graaf"].Rows.Add(graafID, werkStraatId);
                    werkGraafId = graafID;
                }
                else if (elemnts[0].StartsWith("[segment]"))
                {
                    int segmentID = int.Parse(elemnts[1]);
                    Ds.Tables["segment"].Rows.Add(segmentID, werkGraafId);
                    werkSegmentId = segmentID;
                }
                else if (elemnts[0].StartsWith("[knoop]"))
                {
                    int knoopID = int.Parse(elemnts[1]);
                    bool isNietAanwezig = (Ds.Tables["knoop"].Select($"KnoopID = {knoopID}").Length == 0);
                    if (isNietAanwezig)
                    {
                        Ds.Tables["knoop"].Rows.Add(knoopID, werkSegmentId);
                        werkKnoopId = knoopID;
                    }
                }
                else if (elemnts[0].StartsWith("[punt]"))
                {
                    decimal x = Convert.ToDecimal(elemnts[1], CultureInfo.InvariantCulture);
                    decimal y = Convert.ToDecimal(elemnts[2], CultureInfo.InvariantCulture);
                    if(werkKnoopId >= 0)
                    {
                        Ds.Tables["punt"].Rows.Add(x, y, werkSegmentId, werkKnoopId);
                        werkKnoopId = -1;
                    }
                    else
                    {
                        Ds.Tables["punt"].Rows.Add(x, y, werkSegmentId, null);
                    }
                }
            }
        }

        private void ToDb()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
            using (SqlBulkCopy sqlbc = new SqlBulkCopy(connectionString))
            {
                try
                {
                    foreach (DataTable d in Ds.Tables)
                    {
                        foreach (DataColumn c in d.Columns)
                        sqlbc.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                        sqlbc.DestinationTableName = d.TableName;
                        sqlbc.BulkCopyTimeout = 1800;
                        sqlbc.WriteToServer(d);
                        sqlbc.ColumnMappings.Clear();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
