using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

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

        private static void GeefAlleStraatNamenAlfabetisch()
        {
            Console.WriteLine("Geef de gemeentenaam");
            string gemeenteNaam = Console.ReadLine();

            string connectionString = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string gemeenteSql = $"SELECT GemeenteID FROM gemeente WHERE GemeenteNaam = '{gemeenteNaam}';";
                int gemeenteID;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(gemeenteSql, conn))
                {
                    gemeenteID = (int)cmd.ExecuteScalar();
                    cmd.CommandText = $"SELECT StraatNaam FROM straat WHERE GemeenteID = '{gemeenteID}' ORDER BY StraatNaam ASC;";
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
                int gemeenteID;
                int provincieID;
                String provincieNaam;
                int straatID;
                int graafID;
                List<int> segmentIDs = new List<int>();
                conn.Open();
                // Alle data ophalen
                try
                {
                    string gemeenteSql = $"SELECT GemeenteID, ProvincieID FROM gemeente WHERE GemeenteNaam = '{gemeenteNaam}';";
                    SqlCommand cmd = new SqlCommand(gemeenteSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    gemeenteID = reader.GetInt32(0);
                    provincieID = reader.GetInt32(1);
                    reader.Close();
                    cmd.CommandText = $"SELECT ProvincieNaam FROM provincie WHERE ProvincieID = '{provincieID}';";
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    provincieNaam = reader.GetString(0);
                    reader.Close();
                    cmd.CommandText = $"SELECT StraatID FROM straat WHERE StraatNaam = '{straatNaam}' AND GemeenteID = '{gemeenteID}' ;";
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    straatID = reader.GetInt32(0);
                    reader.Close();
                    cmd.CommandText = $"SELECT GraafID FROM graaf WHERE StraatID = '{straatID}';";
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    graafID = reader.GetInt32(0);
                    reader.Close();
                    cmd.CommandText = $"SELECT SegmentID FROM segment WHERE GraafID = '{graafID}';";
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        segmentIDs.Add(reader.GetInt32(0));
                    }
                    reader.Close();

                    // starten met output o.a. van herhalende elementen
                    Console.WriteLine("provincie: " + provincieID + " " + provincieNaam);
                    Console.WriteLine("gemeente:" + gemeenteID + " " + gemeenteNaam);
                    Console.WriteLine("straat: " + straatID + " " + straatNaam);
                    Console.WriteLine("graaf: " + graafID);
                    foreach (int segid in segmentIDs)
                    {
                        Console.WriteLine("segment: " + segid);
                        cmd.CommandText = $"SELECT X, Y, KnoopID FROM punt WHERE SegmentID = '{segid}';";
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(2))
                            {
                                Console.WriteLine("knoop: " + reader.GetInt32(2));
                            }
                            decimal x = reader.GetDecimal(0);
                            decimal y = reader.GetDecimal(1);

                            Console.WriteLine("punt: " + x + " " + y);
                        }
                        reader.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Er is een probleem opgetreden.");
                    Console.WriteLine("Ben je zeker dat je alles juist hebt ingevoerd");
                    Console.WriteLine("Fout: " + e.Message);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private static void GeefStraatIDsVanGemeenteNaam()
        {
            Console.WriteLine("Geef de stadsnaam");
            string gemeenteNaam = Console.ReadLine();

            string connectionString = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string gemeenteSql = $"SELECT GemeenteID FROM gemeente WHERE GemeenteNaam = '{gemeenteNaam}';";
                int gemeenteID;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(gemeenteSql, conn))
                {
                    gemeenteID = (int)cmd.ExecuteScalar();
                    cmd.CommandText = $"SELECT StraatID FROM straat WHERE GemeenteID = '{gemeenteID}';";
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
        }
    }
}
