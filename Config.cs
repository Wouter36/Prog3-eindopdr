using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog3EindOpdracht
{
    class Config
    {
        public static string[] vlaamseProvincieIds = CSVReader.ReadCSV(pathVlaamseProvincies, ',').First();
        public static List<string[]> Provincies = CSVReader.ReadCSV(pathProvincieInfo, ';');
        public static List<string[]> strListGemeentes = CSVReader.ReadCSV(pathGemeentenaam, ';');
        public static List<string[]> strListStraatGemeenteIds = CSVReader.ReadCSV(pathGemeenteStraatID, ';');

        public const string TaalCode = "nl";

        public const string pathWRData = @"WRdata\WRdata-master\WRdata.csv";
        public static Dictionary<string, int> indexwrdata = new Dictionary<string, int>()
        {
            { "wegsegmentid",0},
            { "geo",1},
            {"morfologie", 2},
            {"status", 3},
            {"beginwegknoopid", 4},
            {"eindwegknoopid", 5},
            {"linksstraatnaamid", 6},
            {"rechtsstraatnaamid", 7}

        };
        public const string pathStraatNamen = @"WRdata\WRdata-master\WRstraatnamen.csv";
        public static Dictionary<string, int> indexwrstraatnamen = new Dictionary<string, int>()
        {
            { "straatid",0},
            { "straatnaam",1}
        };
        public const string pathGemeenteStraatID = @"WRdata\WRdata-master\WRGemeenteID.csv";
        public static Dictionary<string, int> indexwrgemeenteid = new Dictionary<string, int>()
        {
            { "straatnaamid",0},
            { "gemeentenaamid",1}
        };
        public const string pathGemeentenaam = @"WRdata\WRdata-master\WRGemeentenaam.csv";
        public static Dictionary<string, int> indexwrgemeentenaam = new Dictionary<string, int>()
        {
            { "gemeentenaamid",0},
            { "gemeenteid",1},
            { "taalcode",2},
            { "gemeentenaam",3}
        };
        public const string pathProvincieInfo = @"WRdata\WRdata-master\ProvincieInfo.csv";
        public static Dictionary<string, int> indexprovincieinfo = new Dictionary<string, int>()
        {
            { "gemeenteid",0},
            { "provincieid",1},
            { "taalcode",2},
            { "provincienaam",3}
        };
        public const string pathVlaamseProvincies = @"WRdata\WRdata-master\ProvincieIDsVlaanderen.csv";
    }
}
