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
    public class TZInfo
    {
        public struct TZType
        {
            public string name;
            public int offset;
            public bool dst;
            public TZType(string name, int offset, bool dst)
            {
                this.name = name; this.offset = offset; this.dst = dst;
            }
        }

        public struct SimpleTime
        {
            public int year;
            public int month;
            public int day;
            public int hour;
            public int minute;
        }

        private IList<TZType> tztypes;
        private TZType normalTZ;

        private Int32[] transTimes;	// transition times
        private SByte[] transTypes;	// timezone description for each transition
        public const long secsPerThreeMonths = 60 * 60 * 24 * 30 * 3;
        private int timecnt;
        private int typecnt;


        public TZInfo(string zoneName)
        {
            this.tztypes = new List<TZType>();
            this.loadZoneFromAll(zoneName);
        }

        public TZType getTZ(long clock)
        {
            if (timecnt > 0 && clock >= transTimes[0])
            {
                int i = 1;
                for (; i < timecnt; ++i)
                {
                    if (clock < transTimes[i])
                    {
                        break;
                    }
                }
                return tztypes[transTypes[i - 1]];
            }
            return normalTZ;
        }

        public static long toSecsSinceEpoch(int year, int month, int day, int h, int m)
        {

            DateTime dt = new DateTime(year, month, day, h, m, 0, DateTimeKind.Utc);
            DateTime epoch1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = new TimeSpan(dt.Ticks - epoch1970.Ticks);
            long secsSince1970 = (long)ts.TotalSeconds;
            return secsSince1970;
        }
        public static long currentSecsSinceEpoch()
        {

            DateTime dt = DateTime.Now; // new DateTime(year, month, day, h, m, 0, DateTimeKind.Utc);
            DateTime epoch1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = new TimeSpan(dt.Ticks - epoch1970.Ticks);
            long secsSince1970 = (long)ts.TotalSeconds;
            return secsSince1970;
        }

        public SimpleTime toUTC(int year, int month, int day, int h, int m)
        {
            DateTime dt = new DateTime(year, month, day, h, m, 0, DateTimeKind.Utc);
            long s = toSecsSinceEpoch(year, month, day, h, m);
            TZType t = getTZ(s);
            dt = dt.AddSeconds(-t.offset);
            SimpleTime ret = new SimpleTime();
            ret.year = dt.Year;
            ret.month = dt.Month;
            ret.day = dt.Day;
            ret.hour = dt.Hour;
            ret.minute = dt.Minute;
            return ret;
        }
        public static IList<string> readZones()
        {
            IList<string> ret = new List<string>();
            readZones(ref ret);
            return ret;
        }

        public static int addDefaultZonesForCountry(string countryCode, IList<string> list)
        {
            return 0;
        }

        /*protected void loadFile(string filename) {
        }*/
        protected void loadFile(BinaryReader ins)
        {
            //QDataStream::ByteOrder origByteOrder = in->byteOrder();
            //in->setByteOrder(QDataStream::BigEndian);
            ins.BaseStream.Seek(28, SeekOrigin.Current); //->skipRawData(28);
            int leapcnt;


            int charcnt;
            leapcnt = swapBytes32(ins.ReadInt32());
            timecnt = swapBytes32(ins.ReadInt32());
            typecnt = swapBytes32(ins.ReadInt32());

            charcnt = swapBytes32(ins.ReadInt32());

            //qint32* transTimes;	// transition times
            //qint8* transTypes;	// timezone description for each transition

            transTimes = new int[timecnt]; //](qint32*)malloc(sizeof(qint32) * timecnt);
            for (int i = 0; i < timecnt; i++)
            {
                transTimes[i] = swapBytes32(ins.ReadInt32());
            }



            transTypes = new sbyte[timecnt]; //(qint8*)malloc(sizeof(qint8) * timecnt);
            for (int i = 0; i < timecnt; i++)
            {
                transTypes[i] = ins.ReadSByte();
            }

            Int32[] offset = new Int32[typecnt];
            sbyte[] dst = new sbyte[typecnt];
            sbyte[] idx = new sbyte[typecnt];

            //Int32 tmp32;
            //sbyte tmp8;
            for (int i = 0; i < typecnt; i++)
            {

                offset[i] = swapBytes32(ins.ReadInt32());
                dst[i] = ins.ReadSByte();
                idx[i] = ins.ReadSByte();

                //tmp32 = offset[i];
                //tmp8 = dst[i];
                //tmp8 = idx[i];
            }
            sbyte[] str = new sbyte[charcnt];
            for (int i = 0; i < charcnt; i++)
            {
                str[i] = ins.ReadSByte();
            }

            //tz = new TZType[typecnt];
            for (int i = 0; i < typecnt; ++i)
            {
                // find string
                int pos = idx[i];
                int end = pos;
                while (str[end] != 0) ++end;
                //QString tmp = QString(str[pos], end-pos);
                string name = "";
                for (int p = pos; p < end; p++)
                {
                    name += (char)str[p];
                }
                //char ctmp[100];
                //memcpy(&ctmp, &str[pos], end-pos);
                //ctmp[end-pos] = 0;

                tztypes.Add(new TZType(name, offset[i], dst[i] != 0));

            }

            Int32[] leapSecs = new Int32[leapcnt * 2];
            for (int i = 0; leapcnt > 0; --leapcnt)
            {
                leapSecs[i++] = swapBytes32(ins.ReadInt32());
                leapSecs[i++] = swapBytes32(ins.ReadInt32());
            }

            //f.close();
            //in->setByteOrder(origByteOrder);

            // Set default timezone (normaltz).
            // First, set default to first non-DST rule.
            int n = 0;
            while (tztypes[n].dst && n < tztypes.Count)
                ++n;
            normalTZ = tztypes[n];

            // When setting "normaltz" (the default timezone) in the constructor,
            // we originally took the first non-DST rule for the current TZ.
            // But this produces nonsensical results for areas where historical
            // non-integer time zones were used, e.g. if GMT-2:33 was used until 1918.

            // This loop, based on a suggestion by Ophir Bleibergh, tries to find a
            // non-DST rule close to the current time. This is somewhat of a hack, but
            // much better than the previous behavior in this case.

            // Tricky: we need to get either the next or previous non-dst TZ
            // We shall take the future non-dst value, by trying to add 3 months at a
            // time to the current date and searching.
            // (QT 4.7 only) qint64 ts = QDateTime::currentMSecsSinceEpoch() / 1000;
            long ts = currentSecsSinceEpoch(); //::currentDateTime().toTime_t();
            //final long ts = System.currentTimeMillis() / 1000;
            for (int i = 0; i < 9; i++)
            {
                TZType currTz = getTZ(ts + secsPerThreeMonths * i);
                if (!currTz.dst)
                {
                    normalTZ = currTz;
                    break;
                }
            }
        }

        private static Stream getStream()
        {
            StreamResourceInfo ef = Application.GetResourceStream(new Uri("tz/tzall.dat", UriKind.Relative));
            return ef.Stream;
        }

        public static int readZones(ref IList<string> target)
        {
            int i = 0;
            Stream resourceStream = getStream();
            using (BinaryReader reader = new BinaryReader(resourceStream))
            {
                UInt16 nameLength;
                string name;
                UInt16 dataLength;
                while (true)
                {
                    try
                    {
                        nameLength = reader.ReadUInt16();
                        name = GeoData.readStringOfSize(reader, nameLength);
                        dataLength = reader.ReadUInt16();
                        reader.BaseStream.Seek(dataLength, SeekOrigin.Current);
                        target.Add(name);
                        i++;
                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }

                }
            }
            return i;
        }


        protected void loadZoneFromAll(string zoneName)
        {
            Stream resourceStream = getStream();
            using (BinaryReader reader = new BinaryReader(resourceStream))
            {
                UInt16 nameLength;
                string name;
                UInt16 dataLength;
                while (true)
                {
                    try
                    {
                        nameLength = reader.ReadUInt16();
                        name = GeoData.readStringOfSize(reader, nameLength);
                        dataLength = reader.ReadUInt16();
                        if (name.Trim().Equals(zoneName.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            loadFile(reader);
                            break;
                        }
                        else
                        {
                            reader.BaseStream.Seek(dataLength, SeekOrigin.Current);
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }

                }
            }
        }

        public static Int64 swapBytes64(Int64 value)
        {
            UInt64 uvalue = (UInt64)value;
            UInt64 swapped = ((0x00000000000000FF) & (uvalue >> 56)
                              | (0x000000000000FF00) & (uvalue >> 40)
                              | (0x0000000000FF0000) & (uvalue >> 24)
                              | (0x00000000FF000000) & (uvalue >> 8)
                              | (0x000000FF00000000) & (uvalue << 8)
                              | (0x0000FF0000000000) & (uvalue << 24)
                              | (0x00FF000000000000) & (uvalue << 40)
                              | (0xFF00000000000000) & (uvalue << 56));
            return (Int64)swapped;
        }
        public static Int32 swapBytes32(Int32 value)
        {
            UInt32 uvalue = (UInt32)value;
            UInt32 swapped = ((0x000000FF) & (uvalue >> 24)
                              | (0x0000FF00) & (uvalue >> 8)
                              | (0x00FF0000) & (uvalue << 8)
                              | (0xFF000000) & (uvalue << 24));
            return (Int32)swapped;
        }
        public static Int16 swapBytes16(Int16 value)
        {
            UInt16 uvalue = (UInt16)value;
            UInt16 swapped = (UInt16)((0x00FF) & (uvalue >> 8)
                              | (0xFF00) & (uvalue << 8));
            return (Int16)swapped;
        }

    }
}
