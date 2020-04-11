using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool3
{
    class Ui
    {
        public static void ToonHoofdMenu()
        {
            Console.WriteLine("Wat wil je doen?");

            Console.WriteLine("1. Geeft alle straatIDs op basis van een stadsnaam");
            Console.WriteLine("2. Geeft de straatID van een straatnaam");
            Console.WriteLine("3. Geeft een straat op basis van straat en gemeentenaam");
            Console.WriteLine("4. Geeft alle straatnamen die bij een gemeentenaam horen");

            Console.WriteLine("Typ je keuzenummer en druk op enter");

            string strKeuzeNr = Console.ReadLine();

            if (strKeuzeNr == "1")
            {
                GeefStraatIDsVanGemeenteNaam();
            }
            else if (strKeuzeNr == "2")
            {
                GeefStraatIDVanStraatNaam();
            }
            else if (strKeuzeNr == "3")
            {
                GeefStraatVanStraatEnGemeentenaam();
            }
            else if (strKeuzeNr == "4")
            {
                GeefAlleStraatNamenAlfabetisch();
            }
            else
            {
                Console.WriteLine("Dit is niet geldig als keuzenummer");
            }
            ToonHoofdMenu();
        }

        private static void GeefStraatVanStraatEnGemeentenaam()
        {
            Console.WriteLine("Geef de gemeentenaam");
            string gemeenteNaam = Console.ReadLine();

            Console.WriteLine("Geef de straatnaam");
            string straatNaam = Console.ReadLine();

            string connectionString = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Alle data ophalen
                string gemeenteSql = $"SELECT GemeenteID, ProvincieID FROM gemeente WHERE GemeenteNaam = '{gemeenteNaam}';";
                int gemeenteID;
                int provincieID;
                using (SqlCommand cmd = new SqlCommand(gemeenteSql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    gemeenteID = reader.GetInt32(0);
                    provincieID = reader.GetInt32(1);
                    reader.Close();
                };
                string provincieSql = $"SELECT ProvincieNaam FROM provincie WHERE ProvincieID = '{provincieID}';";
                String provincieNaam;
                using (SqlCommand cmd = new SqlCommand(provincieSql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    provincieNaam = reader.GetString(0);
                    reader.Close();
                };
                string straatSql = $"SELECT StraatID FROM straat WHERE" +
                    $" StraatNaam = '{straatNaam}' AND GemeenteID = '{gemeenteID}' ;";
                int straatID;
                using (SqlCommand cmd = new SqlCommand(straatSql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    straatID = reader.GetInt32(0);
                    reader.Close();
                };
                string graafSql = $"SELECT GraafID FROM graaf WHERE StraatID = '{straatID}';";
                int graafID;
                using (SqlCommand cmd = new SqlCommand(graafSql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    graafID = reader.GetInt32(0);
                    reader.Close();
                };
                string segmentSql = $"SELECT SegmentID FROM segment WHERE GraafID = '{graafID}';";
                int segmentID;
                using (SqlCommand cmd = new SqlCommand(segmentSql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    segmentID = reader.GetInt32(0);
                    reader.Close();
                };
                //string puntSql = $"SELECT X, Y FROM punt WHERE SegmentID = '{segmentID}';";
                //decimal x;
                //decimal y;
                //using (SqlCommand cmd = new SqlCommand(puntSql, conn))
                //{
                //    SqlDataReader reader = cmd.ExecuteReader();
                //    Console.WriteLine(reader.HasRows);
                //    reader.Read();
                //    x = reader.GetDecimal(0);
                //    y = reader.GetDecimal(1);
                //    reader.Close();
                //};
                Console.WriteLine(provincieID + " " + provincieNaam);
                Console.WriteLine(gemeenteID + " " + gemeenteNaam);
                Console.WriteLine(straatID + " " + straatNaam);
                Console.WriteLine(graafID);
                Console.WriteLine(segmentID);
                
                conn.Close();
            }
        }

        private static void GeefAlleStraatNamenAlfabetisch()
        {
            Console.WriteLine("Geef de gemeentenaam");
            string gemeenteNaam = Console.ReadLine();

            string connectionString = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string gemeenteSql = $"SELECT GemeenteID FROM gemeente WHERE GemeenteNaam = '{gemeenteNaam}';";
                int gemeenteID;
                using (SqlCommand cmd = new SqlCommand(gemeenteSql, conn))
                {
                    gemeenteID = (int)cmd.ExecuteScalar();
                };

                string sql = $"SELECT StraatNaam FROM straat WHERE GemeenteID = '{gemeenteID}' ORDER BY StraatNaam ASC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetString(0));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Niets gevonden");
                    }
                }
                conn.Close();
            }
            Console.WriteLine("Druk op een toets");
            Console.Read();
        }

        private static void GeefStraatIDVanStraatNaam()
        {
            Console.WriteLine("Geef de straatnaam");
            string straatNaam = Console.ReadLine();

            string connectionString = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = $"SELECT StraatID FROM straat WHERE StraatNaam = '{straatNaam}';";
                int straatID;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    straatID = (int)cmd.ExecuteScalar();
                };

                Console.WriteLine(straatID);
            }
            Console.WriteLine("Druk op een toets");
            Console.Read();
        }

        private static void GeefStraatIDsVanGemeenteNaam()
        {
            Console.WriteLine("Geef de stadsnaam");
            string gemeenteNaam = Console.ReadLine();

            string connectionString = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string gemeenteSql = $"SELECT GemeenteID FROM gemeente WHERE GemeenteNaam = '{gemeenteNaam}';";
                int gemeenteID;
                using (SqlCommand cmd = new SqlCommand(gemeenteSql, conn))
                {
                    gemeenteID = (int)cmd.ExecuteScalar();
                };

                string sql = $"SELECT StraatID FROM straat WHERE GemeenteID = '{gemeenteID}';";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetInt32(0));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Niets gevonden");
                    }
                }
                conn.Close();
            }
            Console.WriteLine("Druk op een toets");
            Console.Read();
        }
    }
}
