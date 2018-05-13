using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LeapingLight.Astrology;
using System.Collections.Generic;

namespace AstrologyToolkit.LeapingLightAstrology
{

    /*
     * int latitudeCents = 5128;
    int longitudeCents = 0;
    QString countryCode = "UK";
    QString country = "United Kingdom";
    QString placeName = "Greenwich";
     * */
    public class ChartData
    {
        public static int DefaultHouseSystem = HouseCalc.HOUSES_PLACIDUS;
        public const string TimeZoneDeviceLocal = "DeviceLocal";
        public string TimeZone = TimeZoneDeviceLocal;
        public float Latitude = 51.28f;
        public float Longitude = 0.0f;
        public int Year = 1978;
        public int Month = 4;
        public int Day = 12;
        public int Hour = 19;
        public int Minute = 20;
        private int privHouseSystem = DefaultHouseSystem;
        public int HouseSystem {
            set
            {
                privHouseSystem = value;
            }
            get
            {
                if (privHouseSystem != HouseCalc.HOUSES_NULL)
                {
                    return DefaultHouseSystem;
                }
                return privHouseSystem;
            }
        }
        public string TimeString
        {
            get
            {
                return Year.ToString() + "/" +
                Month.ToString().PadLeft(2, '0') + "/" +
                Day.ToString().PadLeft(2, '0') + " " +
                Hour.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');
            }
        }
        public string CountryCode
        {
            get;
            set;
        }
        public string CountryName
        {
            get;
            set;
        }
        public string PlaceName
        {
            get;
            set;
        }
        public string PlaceDescription {
            get
            {
                return CountryName + "/" + PlaceName;
            }
        }
        public string PersonName
        {
            get;
            set;
        }
        public string PersonDescription
        {
            get;
            set;
        }
        private TZInfo.SimpleTime utcTime;
        public TZInfo.SimpleTime CalculatedUtcTime
        {
            get
            {
                return utcTime;
            }
        }

        public string FileName
        {
            set;
            get;
        }
        public string FileNameAsName
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(FileName).Replace('_', ' ');
            }
        }

        public static ChartData CurrentTransits()
        {
            ChartData ret = new ChartData();
            DateTime dt = DateTime.Now;
            ret.TimeZone = TimeZoneDeviceLocal;
            ret.Year = dt.Year;
            ret.Month = dt.Month;
            ret.Day = dt.Day;
            ret.Hour = dt.Hour;
            ret.Minute = dt.Minute;
            ret.HouseSystem = HouseCalc.HOUSES_NULL;
            return ret;
        }

        public void adjustTime(int years, int months, int days, int hours, int minutes)
        {
            DateTime dt = new DateTime(Year, Month, Day, Hour, Minute, 0);
            if (years != 0) dt = dt.AddYears(years);
            if (months != 0) dt = dt.AddMonths(months);
            if (days != 0) dt = dt.AddDays(days);
            if (hours != 0) dt = dt.AddHours(hours);
            if (minutes != 0) dt = dt.AddMinutes(minutes);
            Year = dt.Year;
            Month = dt.Month;
            Day = dt.Day;
            Hour = dt.Hour;
            Minute = dt.Minute;
        }

        public ChartData()
        {
            Latitude = 51.28f;
            Longitude = 0.0f;
            CountryCode = "";
            CountryName = "United Kingdom";
            PlaceName = "Greenwich";
            TimeZone = "DeviceLocal";
            PersonName = "";
            PersonDescription = "";
            DateTime localNow = DateTime.Now;
            Year = localNow.Year;
            Month = localNow.Month;
            Day = localNow.Day;
            Hour = localNow.Hour;
            Minute = localNow.Minute;
        }
        public ChartData(string data) : this()
        {
            updateFromSerialized(data);
        }

        public string serialize()
        {
            return
                CountryCode + "\n" +
                CountryName + "\n" +
                PlaceName + "\n" +
                (int)(Latitude * 100.0) + "\n" +
                (int)(Longitude * 100.0) + "\n" +
                TimeZone + "\n" +
                Year + "\n" +
                Month + "\n" +
                Day + "\n" +
                Hour + "\n" +
                Minute + "\n" +
                PersonName + "\n" +
                PersonDescription + "\n" +
                HouseSystem.ToString(); 
            //+"\n" +
              //   PlaceDescription;
        }
        public void updateFromSerialized(string data)
        {
            string[] d = data.Split(new char[] { '\n' });
            for (int i = 0; i < d.Length; i++)
            {
                string s = d[i];
                switch (i) {
                    case 0: CountryCode = s.Trim(); break;
                    case 1: CountryName = s.Trim(); break;
                    case 2: PlaceName = s.Trim(); break;
                    case 3: float.TryParse(s, out Latitude); Latitude = Latitude / 100.0f; break;
                    case 4: float.TryParse(s, out Longitude); Longitude = Longitude / 100.0f; break;
                    case 5: TimeZone = s.Trim(); break;
                    case 6: int.TryParse(s, out Year); break;
                    case 7: int.TryParse(s, out Month); break;
                    case 8: int.TryParse(s, out Day); break;
                    case 9: int.TryParse(s, out Hour); break;
                    case 10: int.TryParse(s, out Minute); break;
                    case 11: PersonName = s.Trim(); break;
                    case 12: PersonDescription = s.Trim(); break;
                    case 13:
                        if (s == null || s.Trim().Length == 0)
                        {
                            HouseSystem = DefaultHouseSystem;
                        }
                        else
                        {
                            int h = -1;
                            int.TryParse(s.Trim(), out h);
                            HouseSystem = h;
                            if (HouseSystem < 0)
                            {
                                HouseSystem = h;
                            }
                        }
                        break;
                    //case 14: PlaceDescription = s; break;
                }
            }
            //HouseSystem = DefaultHouseSystem; // TRALALAL
        }

        public ChartData Duplicate()
        {
            ChartData ret = new ChartData();
            ret.TimeZone = TimeZone;
            ret.Latitude = Latitude;
            ret.Longitude = Longitude;
            ret.Year = Year;
            ret.Month = Month;
            ret.Day = Day;
            ret.Hour = Hour;
            ret.Minute = Minute;
            ret.HouseSystem = HouseSystem;
            ret.CountryCode = CountryCode;
            ret.CountryName = CountryName;
            ret.PlaceName = PlaceName;
            //ret.PlaceDescription = PlaceDescription;
            return ret;
        }

        static AstroDefs.EPHEM_YEAR preloadedYear = null;
        static AstroDefs.EPHEM_YEAR preloadedYear2 = null;
        static int lastUsed = -1;
        static IDictionary<string, TZInfo> zoneCache = new Dictionary<string, TZInfo>();

        public AstroDefs.planet_positions CalculatePositions()
        {
            return CalculatePositions(false);
        }
        
        public AstroDefs.planet_positions CalculatePositions(bool noPlanets)
        {
            //AstroDefs.EPHEM_YEAR preloadedYear = null;
            //TZInfo.SimpleTime st;
            if (TimeZone != TimeZoneDeviceLocal)
            {
                TZInfo tz;
                if (zoneCache.ContainsKey(TimeZone.Trim()))
                {
                    tz = zoneCache[TimeZone.Trim()];
                }
                else
                {
                    tz = new TZInfo(TimeZone.Trim());
                    zoneCache[TimeZone.Trim()] = tz;
                }
                //TZInfo tz = new TZInfo(TimeZone);
                utcTime = tz.toUTC(Year, Month, Day, Hour, Minute);
            }
            else
            {
                DateTime dt = new DateTime(Year, Month, Day, Hour, Minute, 0, DateTimeKind.Local);
                DateTime utc = dt.ToUniversalTime();
                utcTime = new TZInfo.SimpleTime();
                utcTime.year = utc.Year;
                utcTime.month = utc.Month;
                utcTime.day = utc.Day;
                utcTime.hour = utc.Hour;
                utcTime.minute = utc.Minute;
            }

            AstroDefs.planet_positions pp;
            if (noPlanets)
            {
                pp = AstroDefs.NULLpositions();
                pp.gmtTime = AstroDefs.hmsToHours(utcTime.hour, utcTime.minute, 0);
                pp.gmt0sideralTime = (float)AstroDefs.calcGmtSideralTime(utcTime.year, utcTime.month, utcTime.day, 0, 0);
            }
            else
            {

                //AstroDefs.EPHEM_YEAR yearCache = null;
                if (preloadedYear != null && preloadedYear.days[0].year == utcTime.year)
                {
                    lastUsed = 1;
                    //yearCache = preloadedYear;
                }
                else if (preloadedYear2 != null && preloadedYear2.days[0].year == utcTime.year)
                {
                    lastUsed = 2;
                    //yearCache = preloadedYear2;
                }
                else
                {
                    if (lastUsed == 1)
                    {
                        lastUsed = 2;
                        //yearCache = preloadedYear2;
                    }
                    else
                    {
                        lastUsed = 1;
                        //yearCache = preloadedYear;
                    }
                }

                if (lastUsed == 1)
                {
                    pp = AstroDefs.ephem_calculateRecForGmt(
                        utcTime.year, utcTime.month, utcTime.day, utcTime.hour, utcTime.minute, 0,
                        ref preloadedYear);
                }
                else
                {
                    pp = AstroDefs.ephem_calculateRecForGmt(
                        utcTime.year, utcTime.month, utcTime.day, utcTime.hour, utcTime.minute, 0,
                        ref preloadedYear2);
                }

            }

            if (HouseSystem != HouseCalc.HOUSES_NULL)
            {
                HouseCalc.calcAscAndMc(ref pp, Latitude, Longitude);
            }
            else
            {
                float[] houses = CalculateHouses(pp);
                pp.ascendant = houses[0];
                pp.mediumCoeli = houses[9];
            }
            return pp;
        }

        public float[] CalculateHouses(AstroDefs.planet_positions pp) {
            float[] houses = new float[12];
            HouseCalc.calcHouses(ref houses, pp, HouseSystem, Latitude, Longitude);
            return houses;
        }
    }
}
