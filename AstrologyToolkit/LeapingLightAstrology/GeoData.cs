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
using System.Collections.Generic;
using System.IO;
using System.Windows.Resources;

namespace LeapingLight.Astrology
{
    public class GeoData
    {
        public class GeoCountryInfo
        {
            public string countryCode
            {
                get;
                set;
            }
            public string countryName
            {
                get;
                set;
            }
        }


        public class GeoPlaceInfo
        {
            public string placeName
            {
                get;
                set;
            }
            public int latitudeCents
            {
                get;
                set;
            }
            public int longitudeCents
            {
                get;
                set;
            }
        }

        public static IList<GeoCountryInfo> loadCountryInfo()
        {
            IList<GeoPlaceInfo> dummy = new List<GeoPlaceInfo>();
            IList<GeoCountryInfo> ret = new List<GeoCountryInfo>();
            load(true, ref ret, ref dummy);
            return ret;
        }

        public static IList<GeoPlaceInfo> loadPlaceInfoForCountryCode(string code)
        {
            IList<GeoPlaceInfo> ret = new List<GeoPlaceInfo>();
            IList<GeoCountryInfo> dummy = new List<GeoCountryInfo>();
            load(false, ref dummy, ref ret, true, code);
            return ret;
        }

        public static string readStringOfSize(BinaryReader r, int size)
        {
            string ret = "";
            for (int i = 0; i < size; i++)
            {
                char c = (char)r.ReadByte();
                ret += c;
            }
            return ret;
        }

        public static string coordinatesToString(float latitude, float longitude)
        {
            return coordinatesToString((int)(latitude * 100.0f), (int)(longitude * 100.0f));
        }

        public static string coordinatesToString(int latitudeCents, int longitudeCents)
        {
            string latS = "N";
            if (latitudeCents < 0)
            {
                latS = "S";
                latitudeCents = -latitudeCents;
            }
            string longS = "E";
            if (longitudeCents < 0)
            {
                longS = "W";
                longitudeCents = -longitudeCents;
            }

            int latFull = latitudeCents / 100;
            int longFull = longitudeCents / 100;
            int latCent = latitudeCents - (100 * latFull);
            int longCent = longitudeCents - (100 * longFull);

            return latFull.ToString() + latS + (latCent < 10 ? "0" : "") + latCent.ToString() +
                    " " +
                    longFull.ToString() + longS + (longCent < 10 ? "0" : "") + longCent.ToString();
        }


        private static void load(bool loadCountries,
            ref IList<GeoCountryInfo> countryTarget,
            ref IList<GeoPlaceInfo> placeTarget,
            bool loadPlaces = false,
            string wantedCountryCode = "tst")
        {
            StreamResourceInfo ef = Application.GetResourceStream(new Uri("geodata/geodata.dat", UriKind.Relative));

            Stream resourceStream = ef.Stream;
            using (BinaryReader reader = new BinaryReader(resourceStream))
            {
                //reader.
                byte countryCodeSize;
                string countryCode;
                byte countryNameSize;
                string countryName;
                UInt16 countryDataSize;

                byte placeNameSize;
                string placeName;
                Int16 latitude;
                Int16 longitude;

                while (true)
                {
                    try
                    {
                        countryCodeSize = reader.ReadByte();
                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }
                    countryCode = readStringOfSize(reader, countryCodeSize);
                    countryNameSize = reader.ReadByte();
                    countryName = readStringOfSize(reader, countryNameSize);
                    countryDataSize = reader.ReadUInt16();

                    if (loadCountries)
                    {
                        GeoCountryInfo gci = new GeoCountryInfo();
                        gci.countryCode = countryCode;
                        gci.countryName = countryName;
                        countryTarget.Add(gci);
                    }


                    if (loadPlaces && (wantedCountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)))
                    {
                        int bytesRead = 0;
                        while (bytesRead < countryDataSize)
                        {
                            placeNameSize = reader.ReadByte();
                            placeName = readStringOfSize(reader, placeNameSize);
                            latitude = reader.ReadInt16();
                            longitude = reader.ReadInt16();

                            bytesRead += 1 + placeNameSize + 2 + 2;

                            GeoPlaceInfo gpi = new GeoPlaceInfo();
                            gpi.placeName = placeName;
                            gpi.latitudeCents = latitude;
                            gpi.longitudeCents = longitude;
                            placeTarget.Add(gpi);
                        }
                        if (!loadCountries)
                        {
                            break; // while
                        }
                    }
                    else
                    {
                        reader.BaseStream.Seek(countryDataSize, SeekOrigin.Current);
                        //in.skipRawData(countryDataSize);
                    }
                }
            }
        }
    }
}
