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
using System.IO;
using System.Windows.Resources;

namespace LeapingLight.Astrology
{
    public class AstroDefs
    {

        public struct EPHEM_REC //EF2_DAY
        {
            public UInt16 dummy;
            public UInt16 weekday;
            public UInt16 sid_h;
            public UInt16 sid_m;
            public UInt16 sid_s;
            public UInt16 gmt;
            public UInt16 day;
            public UInt16 month;
            public UInt32 year;
            public UInt16 sun;
            public UInt16 moon;
            public UInt16 mercury;
            public UInt16 venus;
            public UInt16 mars;
            public UInt16 jupiter;
            public UInt16 saturn;
            public UInt16 uranus;
            public UInt16 neptune;
            public UInt16 pluto;
            public UInt16 node;
        }

        public struct planet_positions
        {
            public int weekday;
            public float sideralTime;
            public float gmt0sideralTime;
            public float gmtTime;
            public float sun;
            public float moon;
            public float mercury;
            public float venus;
            public float mars;
            public float jupiter;
            public float saturn;
            public float uranus;
            public float neptune;
            public float pluto;
            public float node;

            public float ascendant;
            public float mediumCoeli;
        }

        public static planet_positions Duplicate(planet_positions pp) 
        {
            planet_positions ret = new planet_positions();
            ret.weekday = pp.weekday;
            ret.sideralTime = pp.sideralTime;
            ret.gmt0sideralTime = pp.gmt0sideralTime;
            ret.gmtTime = pp.gmtTime;
            ret.sun = pp.sun;
            ret.moon = pp.moon;
            ret.mercury = pp.mercury;
            ret.venus = pp.venus;
            ret.mars = pp.mars;
            ret.jupiter = pp.jupiter;
            ret.saturn = pp.saturn;
            ret.uranus = pp.uranus;
            ret.neptune = pp.neptune;
            ret.pluto = pp.pluto;
            ret.node = pp.node;
            ret.ascendant = pp.ascendant;
            ret.mediumCoeli = pp.mediumCoeli;
            return ret;
        }

        public const int SUN = 0;
        public const int MOON = 1;
        public const int MERCURY = 2;
        public const int VENUS = 3;
        public const int MARS = 4;
        public const int JUPITER = 5;
        public const int SATURN = 6;
        public const int URANUS = 7;
        public const int NEPTUNE = 8;
        public const int PLUTO = 9;
        public const int NODE = 10;

        public const int ASCENDANT = 11;
        public const int MEDIUM_COELI = 12;

        public static readonly int[] PLANETS = {
            SUN,MOON,MERCURY,VENUS,MARS,JUPITER,SATURN,URANUS,NEPTUNE,PLUTO,NODE};

        public const int ARIES = 0;
        public const int TAURUS = 1;
        public const int GEMINI = 2;
        public const int CANCER = 3;
        public const int LEO = 4;
        public const int VIRGO = 5;
        public const int LIBRA = 6;
        public const int SCORPIO = 7;
        public const int SAGGITARIUS = 8;
        public const int CAPRICORN = 9;
        public const int AQUARUIS = 10;
        public const int PISCES = 11;


        public static readonly int[] SIGNS = {
            ARIES,TAURUS,GEMINI,CANCER,LEO,VIRGO,LIBRA,SCORPIO,SAGGITARIUS,CAPRICORN,AQUARUIS,PISCES};


        public const int NUM_ASPECTS = 10;
        public const int NUM_SIGNS = 12;
        public const int NUM_PLANETS = 11;

        public static readonly int[] ASPECTS = {
            0,30,45,60,72,90,120,135,150,180
        };

        public static readonly string[] ASPECT_NAMES = {
            "Conjunction", "Semisextile", "Semisquare", "Sextile", "Quintile", "Square",
            "Trine", "Sesquisquare", "Quincunx", "Opposition"
        };


        public static readonly float[] DEFAULT_ORBS = {
            9.0f, // Conjunction
            2.0f, // Semisextile
            2.0f, // Semisquare
            4.0f, // Sextile
            1.0f, // Quintile
            9.0f, // Square
            9.0f, // Trine
            2.0f, // Sesquisquare
            2.0f, // Quincunx
            9.0f  // Opposition
        };

        public static readonly float[] DEFAULT_TRANSIT_ORBS = {
            4.0f, // Conjunction
            0.5f, // Semisextile
            0.5f, // Semisquare
            1.0f, // Sextile
            0.5f, // Quintile
            2.0f, // Square
            2.0f, // Trine
            0.5f, // Sesquisquare
            0.5f, // Quincunx
            3.0f  // Opposition
        };

        public struct chart_rgb
        {
            public readonly float r;
            public readonly float g;
            public readonly float b;
            public chart_rgb(float r, float g, float b)
            {
                this.r = r; this.g = g; this.b = b;
            }
        };

        public static readonly chart_rgb chart_rgb_red = new chart_rgb(0f, 1.0f, 0f);
        public static readonly chart_rgb chart_rgb_green = new chart_rgb(0f, 1.0f, 0f);
        public static readonly chart_rgb chart_rgb_blue = new chart_rgb(0f, 0f, 1.0f);
        public static readonly chart_rgb chart_rgb_black = new chart_rgb(0f, 0f, 1.0f);

        public static readonly chart_rgb[] colors = {
            chart_rgb_blue, // 0
            chart_rgb_blue, // 20
            chart_rgb_red,  // 45
            chart_rgb_blue, // 60
            chart_rgb_black,    // 72
            chart_rgb_red,  // 90
            chart_rgb_blue, // 120
            chart_rgb_green,    // 135
            chart_rgb_green,    // 150
            chart_rgb_red   // 180
        };

        public const int ELEM_FIRE = 0;
        public const int ELEM_AIR = 1;
        public const int ELEM_EARTH = 2;
        public const int ELEM_WATER = 3;

        public static readonly int[] SIGN_AND_HOUSE_ELEMENTS = {
            ELEM_FIRE, ELEM_EARTH, ELEM_AIR, ELEM_WATER,
            ELEM_FIRE, ELEM_EARTH, ELEM_AIR, ELEM_WATER,
            ELEM_FIRE, ELEM_EARTH, ELEM_AIR, ELEM_WATER,
            ELEM_FIRE, ELEM_EARTH, ELEM_AIR, ELEM_WATER,
        };


        public static readonly int[] EPHEM_DAYS_PER_MONTH = {
            31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
        };

        public static readonly string[] SIGN_NAMES = {
            "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Saggitarius", "Capricorn", "Aquarius", "Pisces"
        };

        public static readonly string[] SIGN_SHORT_NAMES = {
            "Ar","Ta","Gm","Cn","Le","Vi","Li","Sc","Sg","Cp","Aq","Pi"
        };

        public static readonly string[] PLANET_NAMES = {
            "Sun", "Moon", "Mercury", "Venus", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto", "Node", "Ascendant", "MC"
        };

        public static readonly string[] PLANET_FULL_NAMES = {
            "Sun", "Moon", "Mercury", "Venus", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto", "North Node", "Ascendant", "Medium Coeli"
        };

        public class EPHEM_YEAR
        {
            public EPHEM_REC[] days = new EPHEM_REC[366];
        };

        public static EPHEM_REC readDay(BinaryReader s)
        {
            EPHEM_REC ret = new EPHEM_REC();
            ret.dummy = s.ReadUInt16();
            ret.weekday = s.ReadUInt16();
            ret.sid_h = s.ReadUInt16();
            ret.sid_m = s.ReadUInt16();
            ret.sid_s = s.ReadUInt16();
            ret.gmt = s.ReadUInt16();
            ret.day = s.ReadUInt16();
            ret.month = s.ReadUInt16();
            ret.year = s.ReadUInt32();
            ret.sun = s.ReadUInt16();
            ret.moon = s.ReadUInt16();
            ret.mercury = s.ReadUInt16();
            ret.venus = s.ReadUInt16();
            ret.mars = s.ReadUInt16();
            ret.jupiter = s.ReadUInt16();
            ret.saturn = s.ReadUInt16();
            ret.uranus = s.ReadUInt16();
            ret.neptune = s.ReadUInt16();
            ret.pluto = s.ReadUInt16();
            ret.node = s.ReadUInt16();
            return ret;
        }

        public static EPHEM_YEAR readYear(BinaryReader s)
        {
            EPHEM_YEAR ret = new EPHEM_YEAR();
            for (int i = 0; i < 366; i++)
            {
                ret.days[i] = readDay(s);
            }
            return ret;
        }

        public static EPHEM_YEAR ephem_readYear(int year)
        {
            StreamResourceInfo ef = Application.GetResourceStream(new Uri("ef2/" + year.ToString() + ".ef2", UriKind.Relative));

            Stream resourceStream = ef.Stream;
            using (BinaryReader reader = new BinaryReader(resourceStream))
            {
                return readYear(reader);
            }
        }

        ///////



        public static string ephem_nameOfAspect(int aspect)
        {
            for (int i = 0; i < 10; i++)
            {
                if (ASPECTS[i] == aspect)
                {
                    return ASPECT_NAMES[i];
                }
            }
            return "";
        }

        public static int ephem_signForPosition(float pos)
        {
            return (int)Math.Floor(pos / 30.0);
        }

        public static float ephem_signPosForPosition(float pos)
        {
            return pos - (ephem_signForPosition(pos) * 30f);
        }

        public static int ephem_signDegForPosition(float pos)
        {
            float sp = ephem_signPosForPosition(pos);
            return (int)Math.Floor(sp);
        }

        public static int ephem_signCDegForPosition(float pos)
        {
            float sp = ephem_signPosForPosition(pos);
            int sd = ephem_signDegForPosition(pos);
            return (int)Math.Floor((sp - sd) * 100);
        }

        public static string ephem_positionToChar6(float pos)
        {
            string degChar = "";
            int deg = ephem_signDegForPosition(pos);
            if (deg < 10)
            {
                degChar += "0" + deg.ToString();
            }
            else
            {
                degChar += deg.ToString();
            }

            string cdegChar = "";
            int cdeg = ephem_signCDegForPosition(pos);
            if (cdeg < 10)
            {
                cdegChar += "0" + cdeg.ToString();
            }
            else
            {
                cdegChar += cdeg.ToString();
            }

            int sign = ephem_signForPosition(pos);

            return degChar + SIGN_SHORT_NAMES[sign] + cdegChar;
        }

        public static float ephem_positionOfPlanet(EPHEM_REC rec, int planet)
        {
            int pos = 0;
            switch (planet)
            {
                case SUN: pos = rec.sun; break;
                case MOON: pos = rec.moon; break;
                case MERCURY: pos = rec.mercury; break;
                case VENUS: pos = rec.venus; break;
                case MARS: pos = rec.mars; break;
                case JUPITER: pos = rec.jupiter; break;
                case SATURN: pos = rec.saturn; break;
                case URANUS: pos = rec.uranus; break;
                case NEPTUNE: pos = rec.neptune; break;
                case PLUTO: pos = rec.pluto; break;
                case NODE: pos = rec.node; break;
            }
            return pos / 100.0f;
        }

        public static void ephem_setPosition(ref planet_positions rec, int planet, float pos)
        {
            switch (planet)
            {
                case SUN: rec.sun = pos; break;
                case MOON: rec.moon = pos; break;
                case MERCURY: rec.mercury = pos; break;
                case VENUS: rec.venus = pos; break;
                case MARS: rec.mars = pos; break;
                case JUPITER: rec.jupiter = pos; break;
                case SATURN: rec.saturn = pos; break;
                case URANUS: rec.uranus = pos; break;
                case NEPTUNE: rec.neptune = pos; break;
                case PLUTO: rec.pluto = pos; break;
                case NODE: rec.node = pos; break;
                case ASCENDANT: rec.ascendant = pos; break;
                case MEDIUM_COELI: rec.mediumCoeli = pos; break;
            }
        }

        public static float ephem_positionOfPlanet(planet_positions rec, int planet)
        {
            float pos = 0;
            switch (planet)
            {
                case SUN: pos = rec.sun; break;
                case MOON: pos = rec.moon; break;
                case MERCURY: pos = rec.mercury; break;
                case VENUS: pos = rec.venus; break;
                case MARS: pos = rec.mars; break;
                case JUPITER: pos = rec.jupiter; break;
                case SATURN: pos = rec.saturn; break;
                case URANUS: pos = rec.uranus; break;
                case NEPTUNE: pos = rec.neptune; break;
                case PLUTO: pos = rec.pluto; break;
                case NODE: pos = rec.node; break;
                case ASCENDANT: pos = rec.ascendant; break;
                case MEDIUM_COELI: pos = rec.mediumCoeli; break;
            }
            return pos;
        }

        public static float hmsToHours(double h, double m, double s)
        {
            return (float)(h + (m / 60.0f) + (s / (60.0f * 60.0f)));
        }

        public static float filter360(float n)
        {
            if (n >= 360.0)
            {
                return n - 360.0f;
            }
            if (n < 0.0)
            {
                return n + 360.0f;
            }
            return n;
        }






        public static int dayNoFromMonthDay(int month, int day)
        {
            if (month == 1)
            {
                return day - 1;
            }
            else if (month > 1 && month <= 12)
            {
                int daysInPassedMonths = 0;
                for (int i = 0; i < month - 1; i++)
                {
                    daysInPassedMonths += EPHEM_DAYS_PER_MONTH[i];
                }
                return daysInPassedMonths + day - 1;
            }
            else
            {
                return -1;
            }
        }

        public static float calculateMeanFloat(float a, float b, float percentAfterA)
        {
            if (a > (b + 180.0)) // Kind of a hack..!.!
            {
                b += 360.0f;
            }
            float res = a + (((b - a) * percentAfterA) / 100.0f);
            if (res >= 360.0f)
            {
                res -= 360.0f;
            }
            return res;
        }

        public static planet_positions positionsForEphemRec(EPHEM_REC rec)
        {
            planet_positions pos = new planet_positions();
            pos.sideralTime = hmsToHours(rec.sid_h, rec.sid_m, rec.sid_s);
            pos.gmt0sideralTime = pos.sideralTime;
            pos.gmtTime = 0.0f;
            pos.weekday = rec.weekday;
            pos.sun = rec.sun / 100.0f;
            pos.moon = rec.moon / 100.0f;
            pos.mercury = rec.mercury / 100.0f;
            pos.venus = rec.venus / 100.0f;
            pos.mars = rec.mars / 100.0f;
            pos.jupiter = rec.jupiter / 100.0f;
            pos.saturn = rec.saturn / 100.0f;
            pos.uranus = rec.uranus / 100.0f;
            pos.neptune = rec.neptune / 100.0f;
            pos.pluto = rec.pluto / 100.0f;
            pos.node = rec.node / 100.0f;
            pos.ascendant = 0.0f;
            pos.mediumCoeli = 270.0f;
            return pos;
        }

        public static planet_positions NULLpositions()
        {
            planet_positions pos = new planet_positions();
            pos.sideralTime = 0;
            pos.gmtTime = 0.0f;
            pos.weekday = 0;
            pos.sun = 0;
            pos.moon = 0;
            pos.mercury = 0;
            pos.venus = 0;
            pos.mars = 0;
            pos.jupiter = 0;
            pos.saturn = 0;
            pos.uranus = 0;
            pos.neptune = 0;
            pos.pluto = 0;
            pos.node = 0;
            pos.ascendant = 0.0f;
            pos.mediumCoeli = 0.0f;
            return pos;
        }

        public static planet_positions calculateMean(planet_positions rec1, planet_positions rec2, float percentAfterRec1)
        {
            planet_positions res = new planet_positions();
            res.ascendant = 0.0f; // NullHouses - Houses corresponds to signs
            res.mediumCoeli = 270.0f;
            res.gmt0sideralTime = rec1.gmt0sideralTime;

            res.weekday = rec1.weekday;

            if (rec1.sideralTime > rec2.sideralTime)
            {
                float sidTime = calculateMeanFloat(rec1.sideralTime, rec2.sideralTime + 24.0f, percentAfterRec1);
                if (sidTime >= 24.0)
                {
                    res.sideralTime = sidTime - 24;
                }
                else
                {
                    res.sideralTime = sidTime;
                }
            }
            else
            {
                res.sideralTime = calculateMeanFloat(rec1.sideralTime, rec2.sideralTime, percentAfterRec1);
            }

            /*if ((rec1.Gmt != 0) || (rec2.Gmt != 0))
            {
                    throw new AstroException("Cannot calculate from records not based on GMT 0:00");
            }*/
            /*res.Day = rec1.Day;
            res.Month = rec1.Month;
            res.Year = rec1.Year;*/


            res.sun = calculateMeanFloat(rec1.sun, rec2.sun, percentAfterRec1);
            res.moon = calculateMeanFloat(rec1.moon, rec2.moon, percentAfterRec1);
            res.mercury = calculateMeanFloat(rec1.mercury, rec2.mercury, percentAfterRec1);
            res.venus = calculateMeanFloat(rec1.venus, rec2.venus, percentAfterRec1);
            res.mars = calculateMeanFloat(rec1.mars, rec2.mars, percentAfterRec1);
            res.jupiter = calculateMeanFloat(rec1.jupiter, rec2.jupiter, percentAfterRec1);
            res.saturn = calculateMeanFloat(rec1.saturn, rec2.saturn, percentAfterRec1);
            res.uranus = calculateMeanFloat(rec1.uranus, rec2.uranus, percentAfterRec1);
            res.neptune = calculateMeanFloat(rec1.neptune, rec2.neptune, percentAfterRec1);
            res.pluto = calculateMeanFloat(rec1.pluto, rec2.pluto, percentAfterRec1);
            res.node = calculateMeanFloat(rec1.node, rec2.node, percentAfterRec1);

            return res;
        }

        public static planet_positions ephem_calculateRecForGmt(int year, int dayNo, float gmtHour, ref EPHEM_YEAR preLoadedYear,
                                                  int verifyMonth = 0, int verifyDay = 0)
        {
            EPHEM_YEAR y1;
            EPHEM_REC rec1;
            EPHEM_REC rec2;
            if (preLoadedYear != null && preLoadedYear.days[0].year == year)
            {
                y1 = preLoadedYear;
            }
            else
            {
                y1 = ephem_readYear(year);
                preLoadedYear = y1;
            }
            if (dayNo == 365)
            {
                EPHEM_YEAR y2;
                y2 = ephem_readYear(year + 1);
                rec1 = y1.days[365];
                rec2 = y2.days[0];
            }
            else
            {
                rec1 = y1.days[dayNo];
                rec2 = y1.days[dayNo + 1];
            }
            if (verifyMonth != 0 && rec1.month != verifyMonth)
            {
                return NULLpositions();
            }
            if (verifyDay != 0 && rec1.day != verifyDay)
            {
                return NULLpositions();
            }
            planet_positions ret = calculateMean(positionsForEphemRec(rec1), positionsForEphemRec(rec2), (gmtHour * 100) / 24);
            ret.gmtTime = gmtHour;
            return ret;
        }

        public static planet_positions ephem_calculateRecForGmt(int year, int month, int day, int h, int m, int s, ref EPHEM_YEAR preLoadedYear)
        {
            return ephem_calculateRecForGmt(year,
                                            dayNoFromMonthDay(month, day),
                                            hmsToHours(h, m, s), ref preLoadedYear, month, day);
        }

        /// aspects etc:

        public static float calcDistance(float planetPos1, float planetPos2)
        {
            float d = -1;
            if (planetPos1 > planetPos2)
            {
                d = planetPos1 - planetPos2;
            }
            else
            {
                d = planetPos2 - planetPos1;
            }
            if (d > 180)
            {
                d = 360 - d;
            }
            return d;
        }

        public static int getAspect(float pos1, float pos2, float[] orbs)
        {
            float dist = calcDistance(pos1, pos2);
            if (dist < 0)
            {
                return -1;
            }
            for (int i = 0; i < NUM_ASPECTS; i++)
            {
                /*float asp = ASPECTS[i];
                float orb = orbs[i];
                float lower = asp - orb;
                float upper = asp + orbs;*/
                if (dist >= (ASPECTS[i] - orbs[i]) &&
                    (dist <= (ASPECTS[i] + orbs[i])))
                {
                    return ASPECTS[i];
                }
            }
            return -1;
        }

        public static double calcGmtSideralTime(int year, int month, int day, int hour, int minute) {
            DateTime gmt2000 = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            DateTime gmtBirth = new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc);
            double secs = gmtBirth.Subtract(gmt2000).TotalSeconds;
            //double secs = gmt2000.secsTo(gmtBirth);
            double days = secs / (double)86400;
            double GMST = 18.697374558 + 24.06570982441908 * days;

            double ST = GMST;
            if (ST < 0) ST = -ST;
            double twentyFours = Math.Floor(ST / 24.0);
            ST = ST - (twentyFours * 24);
            while (ST >= 24) {
                ST -= 24.0;
            }
            if (GMST < 0) {
                ST = -ST;
            }
            //double gmstLong = (double)(GMST * 10000);
            //double hoursTimes10000 = gmstLong % (24 * 10000);
            double gmtSideralTime = ST; //(double)hoursTimes10000 / (double)10000;

            if (gmtSideralTime < 0) {
                gmtSideralTime = 24 + gmtSideralTime;
            }
            return gmtSideralTime;
        }
    }
}
