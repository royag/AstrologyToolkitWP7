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

namespace LeapingLight.Astrology
{
    public class HouseCalc
    {
        public const int HOUSES_NULL = 0;
        public const int HOUSES_EQUAL_ASC = 1;
        public const int HOUSES_WHOLE_VEDIC = 2;
        public const int HOUSES_PORPHYRY = 3;
        public const int HOUSES_PLACIDUS = 4;
        public const int HOUSES_KOCH = 5;

        public const int HOUSE_1 = 0;
        public const int HOUSE_2 = 1;
        public const int HOUSE_3 = 2;
        public const int HOUSE_4 = 3;
        public const int HOUSE_5 = 4;
        public const int HOUSE_6 = 5;
        public const int HOUSE_7 = 6;
        public const int HOUSE_8 = 7;
        public const int HOUSE_9 = 8;
        public const int HOUSE_10 = 9;
        public const int HOUSE_11 = 10;
        public const int HOUSE_12 = 11;

        public static readonly string[] HOUSE_NAMES = {
            "1st", "2nd", "3rd", "4th", "5th", "6th",
            "7th", "8th", "9th", "10th", "11th", "12th"
        };

        public static readonly string[] HOUSE_SYSTEM_NAMES = {
            "NULL", "Equal", "Whole", "Porphyry", "Placidus", "Koch"
        };



        static double degToRad(double deg)
        {
            return deg * Math.PI / 180.0;
        }
        static double cot(double n)
        {
            return Math.Cos(n) / Math.Sin(n);
        }
        static double acot(double n)
        {
            return Math.Atan(1 / n);
        }

        static double radToDeg(double rad)
        {
            return rad * 180.0 / Math.PI;
        }

        public static float eclipticObliquity()
        {
            return 23.448f; // Estimation of the ecliptic obliquity
        }

        public static bool mcAboveAsc(float mc, float asc)
        {
            float desc = AstroDefs.filter360(asc + 180.0f);
            if (desc > asc)
            {
                return (mc > desc) || (mc < asc);
            }
            else
            {
                return (mc > desc) && (mc < asc);
            }
        }

        static double dsin(double n)
        {
            return Math.Sin(degToRad(n));
        }

        static double dtan(double n)
        {
            return Math.Tan(degToRad(n));
        }

        static double dcot(double n)
        {
            return cot(degToRad(n));
        }

        static double dacot(double n)
        {
            return radToDeg(acot(n));
        }

        static double dcos(double n)
        {
            return Math.Cos(degToRad(n));
        }

        static double datan(double n)
        {
            return radToDeg(Math.Atan(n));
        }

        static double dasin(double n)
        {
            return radToDeg(Math.Asin(n));
        }

        /*
void placProc(double &D11, double &D12, double &D2, double &D3,
              double H11, double H12, double H2, double H3,
              double f, double e) {
    double F11 = 1.0/3;
    double F12 = 2.0/3;
    double F2 = 2.0/3;
    double F3 = 1.0/3;


    double dtf = dtan(f);
    double dt2 = dtan(D2);
    double s = dtf * dt2;
    double ds = dasin(s);

    double A11 = F11 *  dasin ( dtan (f) * dtan (D11) );
    double  A12 = F12 *  dasin ( dtan (f) * dtan (D12 ));
    double  A2 = F2 *  dasin ( dtan (f) * dtan (D2) ) ;
    double  A3 = F3 *  dasin ( dtan (f) * dtan (D3));

    //6. Compute the house cusp positions as follows:
    double M11 = datan ( dsin (A11) / ( dcos (H11) * dtan (D11)) ) ;
    double M12 = datan ( dsin (A12) / ( dcos (H12) * dtan (D12)) ) ;
    double M2 = datan ( dsin (A2) / ( dcos (H2) * dtan (D2)) ) ;
    double M3 = datan ( dsin (A3) / ( dcos (H3) * dtan (D3)) ) ;

    //7. Compute the intermediate house cusps:
    double R11 = datan ( ( dtan (H11) * dcos (M11) ) / dcos ( M11 + e) ) ;
    double R12 = datan ( ( dtan (H12) * dcos (M12) ) / dcos ( M12 + e) ) ;
    double R2 = datan ( ( dtan (H2 )* dcos (M2) ) / dcos ( M2 + e) ) ;
    double R3 = datan ( ( dtan (H3) * dcos (M3) ) / dcos ( M3 + e) );

    D11 =R11; // filter360(R11);
    D12 =R12; // filter360(R12);
    D2 = R2; //filter360(R2);
    D3 = R3; //filter360(R3);
}*/

        public static float ascendantFromSideralLatitude(float localSideral, float latitude)
        {
            double eh = degToRad(eclipticObliquity());

            double ramc = degToRad(localSideral * 15.0);
            double lati = degToRad(latitude);
            double teller = Math.Cos(ramc);
            double nevner = ((0.0 - Math.Sin(ramc)) * Math.Cos(eh) -
                    Math.Tan(lati) * Math.Sin(eh));

            double asc_rad = Math.Atan(teller / nevner);
            double asc = radToDeg(asc_rad);
            if (nevner < 0)
            {
                asc += 180.0;
            }
            return AstroDefs.filter360((float)asc);
        }

        public static float mediumCoeliFromSideral(float localSideral)
        {
            double eh = degToRad(eclipticObliquity());
            double ramc = degToRad(localSideral * 15.0);
            double mc_rad = Math.Atan(Math.Tan(ramc) / Math.Cos(eh));
            double mc = radToDeg(mc_rad);
            if (mc < 0)
            {
                mc += 180;
            }
            if (radToDeg(ramc) > 180)
            {
                mc += 180;
            }
            return AstroDefs.filter360((float)mc);
        }

        public static double cuspPlacidus(double deg, double FF, bool fNeg, float localSideral, float latitude)
        {
            double AA = degToRad(latitude);
            if (AA == 0) {
                AA = 0.0001;
            }
            double RA = degToRad(localSideral * 15.0);
            double OB = degToRad(eclipticObliquity());
            double LO;
            double R1;
            double XS;
            double X;
            int i;
            R1 = RA + radToDeg(deg);
            X = (fNeg ? 1.0 : -1.0);
            for (i = 1; i <= 10; i++) {

            /* This formula works except at 0 latitude (AA == 0.0). */

            XS = X*Math.Sin(R1)*Math.Tan(OB)*Math.Tan(AA == 0.0 ? 0.0001 : AA);
            XS = Math.Acos(XS);
            if (XS < 0.0)
                XS += Math.PI; //rPi;
                R1 = RA + (fNeg ? Math.PI/*rPi*/-(XS/FF) : (XS/FF));
            }
            LO = Math.Atan(Math.Tan(R1)/Math.Cos(OB));
            if (LO < 0.0) {
                 LO += Math.PI; //rPi;
            }
            if (Math.Sin(R1) < 0.0) {
                LO += Math.PI;//rPi;
            }
            return radToDeg(LO);
        }

        public static void calcHousesPlacidus(ref float[] houses, float asc, float mc, float localSideral, float latitude)
        {
            houses[1 - 1] = asc;
            houses[4 - 1] = mc + 180.0f;
            houses[5 - 1] = (float)cuspPlacidus(30.0, 3.0, false, localSideral, latitude) + 180.0f;
            houses[6 - 1] = (float)cuspPlacidus(60.0, 1.5, false, localSideral, latitude) + 180.0f;
            houses[2 - 1] = (float)cuspPlacidus(120.0, 1.5, true, localSideral, latitude);
            houses[3 - 1] = (float)cuspPlacidus(150.0, 3.0, true, localSideral, latitude);
            for (int i = 0; i < 12; i++) {
            if (i < 6)
                houses[i] = AstroDefs.filter360(houses[i]/*+is.rSid*/);
            else
                houses[i] = AstroDefs.filter360(houses[i - 6] + 180.0f); //rDegHalf);
            }
        }

        public static double Angle(double x, double y)
        {
          double a;

          if (x != 0.0) {
            if (y != 0.0)
              a = Math.Atan(y/x);
            else
              a = x < 0.0 ? Math.PI : 0.0;
          } else
            a = y < 0.0 ? -(Math.PI/2) : (Math.PI/2);
          if (a < 0.0)
            a += Math.PI;
          if (y < 0.0)
              a += Math.PI;
          return a;
        }


        public static void calcHousesKoch(ref float[] houses, float asc, float mc, float localSideral, float latitude)
        {
            double A1, A2, A3, KN, D, X;
            int i;
            double AA = degToRad(latitude);
            if (AA == 0) {
                AA = 0.0001;
            }
            double RA = degToRad(localSideral * 15.0);
            double OB = degToRad(eclipticObliquity());
            A1 = Math.Sin(RA) * Math.Tan(AA) * Math.Tan(OB);
            A1 = Math.Asin(A1);
            for (i = 1; i <= 12; i++) {
                D = AstroDefs.filter360(60.0f+30.0f*(float)i);
                A2 = D/90.0-1.0; KN = 1.0;
                if (D >= 180.0) {
                    KN = -1.0;
                    A2 = D/90.0-3.0;
                }
                A3 = degToRad(AstroDefs.filter360((float)(radToDeg(RA) + D + A2 * radToDeg(A1))));
                X = Angle(Math.Cos(A3) * Math.Cos(OB) - KN * Math.Tan(AA) * Math.Sin(OB), Math.Sin(A3));
                houses[i - 1] = AstroDefs.filter360((float)radToDeg(X));
            }
        }

        public static double calculateLocalSideral(double gmt0sideraltime, double newGmtTime, double longitude)
        {
            // 24hours in normal time gives 3 mins 56.6 seconds sideral time.
            // hms2hours(0,3,56.6) / 24) gives 0.00273843
            double hourLength = 0.00273843;
            double localSideral = newGmtTime + gmt0sideraltime +
                    (newGmtTime * hourLength) +
                    AstroDefs.hmsToHours(0, longitude * 4, 0);
            // [above:] the sun spends 4 minutes to pass each of the 360 degrees
            // (360*4=1440mins 1440/60=24hours):
            if (localSideral > 24)
            {
                localSideral -= 24;
            }
            return localSideral;
        }

        public static void calcAscAndMc(ref AstroDefs.planet_positions pos, float latitude, float longitude)
        {
            float localSideral = (float)calculateLocalSideral(pos.gmt0sideralTime, pos.gmtTime, longitude);
            float asc = ascendantFromSideralLatitude(localSideral, latitude);
            float mc = mediumCoeliFromSideral(localSideral);
            if (!mcAboveAsc(mc, asc))
            {
                if ((latitude > 66) || (latitude < -66))
                {
                    // We assume this is due to the weird behaviour of the
                    // ascendant in the far north (/south ?):
                    // We turn the ascendant around 180 degrees,
                    // some say we should also turn medium coeli
                    asc = AstroDefs.filter360(asc + 180);
                }
                else
                {
                    //throw new AstroException("Something wrong with ascendant calculation!");
                    // -----------------
                    // Hmmm for Hillary Clinton, turning the MC around seems to fix the deal...
                    mc = AstroDefs.filter360(mc + 180);
                    //return; // do nothing
                }
            }
            pos.ascendant = asc;
            pos.mediumCoeli = mc;
        }

        public static void calcHouses(ref float[] houses, AstroDefs.planet_positions positions, int houseSystem, float latitude, float longitude)
        {
            float localSideral = (float)calculateLocalSideral(positions.gmt0sideralTime, positions.gmtTime, longitude);
            switch (houseSystem)
            {
                case HOUSES_NULL:
                    calcHousesNULL(ref houses);
                    break;
                case HOUSES_EQUAL_ASC:
                    calcHousesEqualASC(ref houses, positions.ascendant);
                    break;
                case HOUSES_WHOLE_VEDIC:
                    calcHousesWholeVedic(ref houses, positions.ascendant);
                    break;
                case HOUSES_PORPHYRY:
                    calcHousesPorphyry(ref houses, positions.ascendant, positions.mediumCoeli);
                    break;
                case HOUSES_PLACIDUS:
                    calcHousesPlacidus(ref houses, positions.ascendant, positions.mediumCoeli, localSideral, latitude);
                    break;
                case HOUSES_KOCH:
                    calcHousesKoch(ref houses, positions.ascendant, positions.mediumCoeli, localSideral, latitude);
                    break;
                default:
                    calcHousesNULL(ref houses);
                    break;
            }
        }

        public static void calcHousesNULL(ref float[] houses)
        {
            for (int i = 0; i < 12; i++)
            {
                houses[i] = i * 30;
            }
        }

        public static void calcHousesEqualASC(ref float[] houses, float asc)
        {
            for (int i = 0; i < 12; i++)
            {
                if (i == 0)
                {
                    houses[i] = asc;
                }
                else
                {
                    houses[i] = AstroDefs.filter360(houses[i - 1] + 30);
                }
            }
        }

        public static void calcHousesWholeVedic(ref float[] houses, float asc)
        {
            for (int i = 0; i < 12; i++)
            {
                if (i == 0)
                {
                    houses[i] = AstroDefs.ephem_signForPosition(asc) * 30.0f;
                }
                else
                {
                    houses[i] = AstroDefs.filter360(houses[i - 1] + 30);
                }
            }
        }

        public static void calcHousesPorphyry(ref float[] houses, float asc, float mc)
        {
            float fourthQuadSize = AstroDefs.calcDistance(mc, asc);
            float signSizeQ4 = fourthQuadSize / 3.0f;
            float firstQuadSize = 180 - fourthQuadSize;
            float signSizeQ1 = firstQuadSize / 3.0f;
            houses[0] = asc;
            houses[1] = AstroDefs.filter360(asc + signSizeQ1);
            houses[2] = AstroDefs.filter360(asc + signSizeQ1 * 2);
            float ic = houses[3] = AstroDefs.filter360(mc + 180.0f); // IC/4th
            houses[4] = AstroDefs.filter360(ic + signSizeQ4);
            houses[5] = AstroDefs.filter360(ic + signSizeQ4 * 2);
            float dec = houses[6] = AstroDefs.filter360(asc + 180.0f); // Descendant/7th
            houses[7] = AstroDefs.filter360(dec + signSizeQ1);
            houses[8] = AstroDefs.filter360(dec + signSizeQ1 * 2);
            houses[9] = mc; // MC/10th
            houses[10] = AstroDefs.filter360(mc + signSizeQ4);
            houses[11] = AstroDefs.filter360(mc + signSizeQ4 * 2);
        }

        public static int houses_houseOfPosition(float pos, float[] houses)
        {
            for (int i = HOUSE_1; i <= HOUSE_12; i++)
            {
                float cur = houses[i];
                float next = (i == HOUSE_12 ? houses[HOUSE_1] : houses[i + 1]);
                if (cur < next)
                {
                    if (pos >= cur && pos < next)
                    {
                        return i;
                    }
                }
                else
                {
                    if (pos >= cur || pos < next)
                    {
                        return i;
                    }
                }
            }
            return -1; // shouldn't happen
        }



    }
}
