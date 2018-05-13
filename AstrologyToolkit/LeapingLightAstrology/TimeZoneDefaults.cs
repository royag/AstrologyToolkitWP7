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

namespace AstrologyToolkit.LeapingLightAstrology
{
    public class TimeZoneDefaults
    {


    static void ifCCeqCCaddZtoLISTandINC(string cc1,string cc2,string zone,ref IList<String> list, ref int i) {
        if (cc1.Equals(cc2, StringComparison.InvariantCultureIgnoreCase))
        {list.Add(zone);i++;}
    }



    public static void usa1(string countryCode, ref IList<String> list, ref int i)
    {
        // USA:
        ifCCeqCCaddZtoLISTandINC(countryCode, "USAK", "America/Anchorage", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNY", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USVA", "America/Louisville", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USAL", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USND", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USHI", "Pacific/Honolulu", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USRI", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USDE", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMD", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMN", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNE", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USCO", "America/Denver", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USME", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMO", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIA", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USWY", "America/Denver", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USKY", "America/Louisville", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USOH", "America/Detroit", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIL", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USAZ", "America/Phoenix", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USOR", "America/Los_Angeles", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNH", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USGA", "America/Louisville", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USTN", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USTX", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USSC", "America/Louisville", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", "America/Indiana/Indianapolis", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USID", "America/Boise", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USCT", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNJ", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USPA", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USFL", "America/Louisville", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMI", "America/Detroit", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMS", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USAR", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USOK", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USSD", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USWI", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMT", "America/Boise", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USVT", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USKS", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USCA", "America/Los_Angeles", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNV", "America/Los_Angeles", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USDC", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMA", "America/New_York", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USWV", "America/Louisville", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNM", "America/Denver", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USUT", "America/Boise", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNC", "America/Louisville", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USLA", "America/Chicago", ref list, ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode, "USWA", "America/Los_Angeles", ref list, ref i);
    }

        public const string US_ALASKA = "US/Alaska";
        public const string US_HAWAII = "US/Hawaii";
        public const string US_ARIZONA = "US/Arizona";

        public const string US_PACIFIC = "US/Pacific";
        public const string US_PST8PDT = "PST8PDT";
        public const string US_MOUNTAIN = "US/Mountain";
        public const string US_MST7MDT = "MST7MDT";
        public const string US_CENTRAL = "US/Central";
        public const string US_CST6CDT = "CST6CDT";
        public const string US_EASTERN = "US/Eastern";
        public const string US_EST5EDT = "EST5EDT";

    public static void usa2(string countryCode, ref IList<String> list, ref int i)
    {
        ifCCeqCCaddZtoLISTandINC(countryCode, "USAK", US_ALASKA, ref list, ref i);

        ifCCeqCCaddZtoLISTandINC(countryCode, "USNY", "America/New_York", ref list, ref i);

        ifCCeqCCaddZtoLISTandINC(countryCode, "USVA", US_EASTERN, ref list, ref i); // Virginia
        ifCCeqCCaddZtoLISTandINC(countryCode, "USAL", US_CENTRAL, ref list, ref i); // Alabama

        ifCCeqCCaddZtoLISTandINC(countryCode, "USND", US_CENTRAL, ref list, ref i); // North Dakota (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USND", US_MOUNTAIN, ref list, ref i); // North Dakota
        ifCCeqCCaddZtoLISTandINC(countryCode, "USND", "America/North_Dakota/New_Salem", ref list, ref i); // North Dakota (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USND", "America/North_Dakota/Center", ref list, ref i); // North Dakota (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USHI", US_HAWAII, ref list, ref i);

        ifCCeqCCaddZtoLISTandINC(countryCode, "USRI", US_EASTERN, ref list, ref i); // Rhode Island
        ifCCeqCCaddZtoLISTandINC(countryCode, "USDE", US_EASTERN, ref list, ref i); // Delaware
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMD", US_EASTERN, ref list, ref i); // Maryland
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMN", US_CENTRAL, ref list, ref i); // Minnesota

        ifCCeqCCaddZtoLISTandINC(countryCode, "USNE", US_CENTRAL, ref list, ref i); // Nebraska (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNE", US_MOUNTAIN, ref list, ref i); // Nebraska (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USCO", US_MOUNTAIN, ref list, ref i); // Colorado
        ifCCeqCCaddZtoLISTandINC(countryCode, "USME", US_EASTERN, ref list, ref i); // Maine
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMO", US_CENTRAL, ref list, ref i); // Missouri
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIA", US_CENTRAL, ref list, ref i); // Iowa
        ifCCeqCCaddZtoLISTandINC(countryCode, "USWY", US_MOUNTAIN, ref list, ref i); // Wyoming

        ifCCeqCCaddZtoLISTandINC(countryCode, "USKY", US_CENTRAL, ref list, ref i); // Kentucky (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USKY", US_EASTERN, ref list, ref i); // Kentucky (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USKY", "America/Kentucky/Louisville", ref list, ref i); // Kentucky (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USKY", "America/Kentucky/Monticello", ref list, ref i); // Kentucky (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USOH", US_EASTERN, ref list, ref i); // Ohio
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIL", US_CENTRAL, ref list, ref i); // Illinois

        ifCCeqCCaddZtoLISTandINC(countryCode, "USAZ", US_ARIZONA, ref list, ref i); // Arizona (2?Navajo)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USAZ", US_MOUNTAIN, ref list, ref i); // Arizona (2?Navajo)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USOR", US_PACIFIC, ref list, ref i); // Oregon (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USOR", US_MOUNTAIN, ref list, ref i); // Oregon (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USNH", US_EASTERN, ref list, ref i); // New Hampshire
        ifCCeqCCaddZtoLISTandINC(countryCode, "USGA", US_EASTERN, ref list, ref i); // Georgia

        ifCCeqCCaddZtoLISTandINC(countryCode, "USTN", US_CENTRAL, ref list, ref i); // Tennessee (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USTN", US_EASTERN, ref list, ref i); // Tennessee (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USTX", US_CENTRAL, ref list, ref i); // Texas (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USTX", US_MOUNTAIN, ref list, ref i); // Texas (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USSC", US_EASTERN, ref list, ref i); // South Carolina

        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", US_EASTERN, ref list, ref i); // Indiana (2++)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", US_CENTRAL, ref list, ref i); // Indiana (2++)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", "America/Indiana/Indianapolis", ref list, ref i); // Indiana (2++)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", "America/Indiana/Knox", ref list, ref i); // Indiana (2++)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", "America/Indiana/Marengo", ref list, ref i); // Indiana (2++)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", "America/Indiana/Petersburg", ref list, ref i); // Indiana (2++)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", "America/Indiana/Tell_City", ref list, ref i); // Indiana (2++)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", "America/Indiana/Vevay", ref list, ref i); // Indiana (2++)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", "America/Indiana/Vincennes", ref list, ref i); // Indiana (2++)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USIN", "America/Indiana/Winamac", ref list, ref i); // Indiana (2++)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USID", US_MOUNTAIN, ref list, ref i); // Idaho (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USID", US_PACIFIC, ref list, ref i); // Idaho (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USCT", US_EASTERN, ref list, ref i); // Connecticut
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNJ", US_EASTERN, ref list, ref i); // New Jersey
        ifCCeqCCaddZtoLISTandINC(countryCode, "USPA", US_EASTERN, ref list, ref i); // 	Pennsylvania

        ifCCeqCCaddZtoLISTandINC(countryCode, "USFL", US_CENTRAL, ref list, ref i); // Florida (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USFL", US_EASTERN, ref list, ref i); // Florida (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USMI", US_CENTRAL, ref list, ref i); // 	Michigan (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMI", US_EASTERN, ref list, ref i); // 	Michigan (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USMS", US_CENTRAL, ref list, ref i); // Mississippi
        ifCCeqCCaddZtoLISTandINC(countryCode, "USAR", US_CENTRAL, ref list, ref i); // Arkansas
        ifCCeqCCaddZtoLISTandINC(countryCode, "USOK", US_CENTRAL, ref list, ref i); // Oklahoma

        ifCCeqCCaddZtoLISTandINC(countryCode, "USSD", US_CENTRAL, ref list, ref i); // South Dakota (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USSD", US_MOUNTAIN, ref list, ref i); // South Dakota (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USWI", US_CENTRAL, ref list, ref i); // Wisconsin
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMT", US_MOUNTAIN, ref list, ref i); // Montana
        ifCCeqCCaddZtoLISTandINC(countryCode, "USVT", US_EASTERN, ref list, ref i); // Vermont

        ifCCeqCCaddZtoLISTandINC(countryCode, "USKS", US_CENTRAL, ref list, ref i); // Kansas (2)
        ifCCeqCCaddZtoLISTandINC(countryCode, "USKS", US_MOUNTAIN, ref list, ref i); // Kansas (2)

        ifCCeqCCaddZtoLISTandINC(countryCode, "USCA", US_PACIFIC, ref list, ref i); // California
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNV", US_PACIFIC, ref list, ref i); // Nevada
        ifCCeqCCaddZtoLISTandINC(countryCode, "USDC", US_EASTERN, ref list, ref i); // District of Columbia
        ifCCeqCCaddZtoLISTandINC(countryCode, "USMA", US_EASTERN, ref list, ref i); // Massachusetts
        ifCCeqCCaddZtoLISTandINC(countryCode, "USWV", US_EASTERN, ref list, ref i); // West Virginia
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNM", US_MOUNTAIN, ref list, ref i); // New Mexico
        ifCCeqCCaddZtoLISTandINC(countryCode, "USUT", US_MOUNTAIN, ref list, ref i); // Utah
        ifCCeqCCaddZtoLISTandINC(countryCode, "USNC", US_EASTERN, ref list, ref i); // North Carolina
        ifCCeqCCaddZtoLISTandINC(countryCode, "USLA", US_CENTRAL, ref list, ref i); // Louisiana
        ifCCeqCCaddZtoLISTandINC(countryCode, "USWA", US_PACIFIC, ref list, ref i); // Washington
    }



    public static int addDefaultZonesForCountry(string countryCode, ref IList<String> list) {
        int i = 0;

        //usa1(countryCode, ref list, ref i);
        usa2(countryCode, ref list, ref i);
        

        // REST OF WORLD:
        ifCCeqCCaddZtoLISTandINC(countryCode,"UK","Europe/London",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SI","Europe/Ljubljana",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"IT","Europe/Rome",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"GR","Europe/Athens",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CY","Europe/Nicosia",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AL","Europe/Tirane",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"JA","Asia/Tokyo",ref list,ref i);

        //multiple:
        // Russia (today?)
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Europe/Moscow",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Europe/Kaliningrad",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Yekaterinburg",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Omsk",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Krasnoyarsk",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Irkutsk",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Yakutsk",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Vladivostok",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Magadan",ref list,ref i);
        // Russia (additional/historic?)
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Europe/Samara",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Europe/Volgograd",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Anadyr",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Kamchatka",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Novosibirsk",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Novokuznetsk",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RS","Asia/Sakhalin",ref list,ref i);


        ifCCeqCCaddZtoLISTandINC(countryCode,"TU","Europe/Istanbul",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"NO","Europe/Oslo",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"MN","Europe/Monaco",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"MD","Europe/Chisinau",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"FR","Europe/Paris",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"EG","Africa/Cairo",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"HU","Europe/Budapest",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"GI","Europe/Gibraltar",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BO","Europe/Minsk",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AN","Europe/Andorra",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"LO","Europe/Bratislava",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"FI","Europe/Helsinki",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BE","Europe/Brussels",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SW","Europe/Stockholm",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"IC","Atlantic/Reykjavik",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"EI","Europe/Dublin",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"PK","Asia/Karachi",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"YI","Europe/Belgrade",ref list,ref i);

        //multiple:
        // UKRAINE:
        ifCCeqCCaddZtoLISTandINC(countryCode,"UP","Europe/Kiev",ref list,ref i);
        // UKRAINE - for historic purposes ? :
        ifCCeqCCaddZtoLISTandINC(countryCode,"UP","Europe/Simferopol",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"UP","Europe/Uzhgorod",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"UP","Europe/Zaporozhy",ref list,ref i);

        ifCCeqCCaddZtoLISTandINC(countryCode,"LG","Europe/Riga",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"IZ","Asia/Baghdad",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"PL","Europe/Warsaw",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"LH","Europe/Vilnius",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"GM","Europe/Berlin",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SZ","Europe/Zurich",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SP","Europe/Madrid",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RO","Europe/Bucharest",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"MT","Europe/Malta",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"LS","Europe/Vaduz",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"DA","Europe/Copenhagen",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"KS","Asia/Seoul",ref list,ref i);


        //multiple:
        // CHINA:
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Shanghai",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Harbin",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Chongqing",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Urumqi",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Kashgar",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Hong_Kong",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Ulaanbaatar",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Hovd",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Choibalsan",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Macau",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CH","Asia/Taipei",ref list,ref i);


        //multiple:
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/Whitehorse",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/Vancouver",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/Yellowknife",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/Edmonton",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/Regine",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/Winnipeg",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/Thunder_Bay",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/Montreal",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/Halifax",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CA","America/St_Johns",ref list,ref i);

        ifCCeqCCaddZtoLISTandINC(countryCode,"VT","Europe/Vatican",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"NL","Europe/Amsterdam",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"MK","Europe/Skopje",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BU","Europe/Sofia",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"PO","Europe/Lisbon",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"LU","Europe/Luxembourg",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"EN","Europe/Tallinn",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BK","Europe/Sarajevo",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AU","Europe/Vienna",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"HR","Europe/Zagreb",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"EZ","Europe/Prague",ref list,ref i);


        //// Multiple:
        // Brazil:
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Sao_Paulo",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Noronha",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Belem",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Fortaleza",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Recife",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Araguaina",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Maceio",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Bahia",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Campo_Grande",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Cuiaba",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Santarem",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Porto_Velho",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Boa_Vista",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Manaus",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Eirunepe",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BR","America/Rio_Branco",ref list,ref i);

/*
The tz database contains 16 zones for Brazil.coordinates	TZ	comments	UTC offset	DST	Notes
-0351-03225	America/Noronha	Atlantic islands	-02:00	-
-0127-04829	America/Belem	Amapa, E Para	-03:00	-
-0343-03830	America/Fortaleza	NE Brazil (MA, PI, CE, RN, PB)	-03:00	-
-0803-03454	America/Recife	Pernambuco	-03:00	-
-0712-04812	America/Araguaina	Tocantins	-03:00	-
-0940-03543	America/Maceio	Alagoas, Sergipe	-03:00	-
-1259-03831	America/Bahia	Bahia	-03:00	-
-2332-04637	America/Sao_Paulo	S & SE Brazil (GO, DF, MG, ES, RJ, SP, PR, SC, RS)	-03:00	-02:00
-2027-05437	America/Campo_Grande	Mato Grosso do Sul	-04:00	-03:00
-1535-05605	America/Cuiaba	Mato Grosso	-04:00	-03:00
-0226-05452	America/Santarem	W Para	-03:00	-
-0846-06354	America/Porto_Velho	Rondonia	-04:00	-
+0249-06040	America/Boa_Vista	Roraima	-04:00	-
-0308-06001	America/Manaus	E Amazonas	-04:00	-
-0640-06952	America/Eirunepe	W Amazonas	-04:00	-
-0958-06748	America/Rio_Branco	Acre	-04:00

 */


        //// Multiple:
        // ARGENTINA:
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/Buenos_Aires",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/Cordoba",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/Salta",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/Jujuy",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/Tucuman",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/Catamarca",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/La_Rioja",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/San_Juan",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/Mendoza",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/San_Luis",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/Rio_Gallegos",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AR","America/Argentina/Ushuaia",ref list,ref i);

        //// Multiple:
        // AUSTRALIA:
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Adelaide",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Brisbane",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Broken_Hill",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Darwin",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Hobart",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Lindeman",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Lord_Howe",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Melbourne",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Perth",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AS","Australia/Sydney",ref list,ref i);



        // Asia/Calcutta is same as IST (Indian Standard Time)
        ifCCeqCCaddZtoLISTandINC(countryCode,"IN","Asia/Calcutta",ref list,ref i);
        // Sri LAnka:
        ifCCeqCCaddZtoLISTandINC(countryCode,"CE","Asia/Calcutta",ref list,ref i);
        // Bangladesh:
        ifCCeqCCaddZtoLISTandINC(countryCode,"BG","Asia/Dhaka",ref list,ref i);


        // MISC:
        ifCCeqCCaddZtoLISTandINC(countryCode,"MO","Africa/Casablanca",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"TS","Africa/Tunis",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"LY","Africa/Tripoli",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SF","Africa/Johannesburg",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"NI","Africa/Lagos",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"TZ","Africa/Dar_es_Salaam",ref list,ref i);


        ifCCeqCCaddZtoLISTandINC(countryCode,"SY","Asia/Damascus",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"YM","Asia/Aden",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"KU","Asia/Kuwait",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"IR","Asia/Tehran",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"ID","Asia/Jakarta",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"HK","Asia/Hong_Kong",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CB","Asia/Phnom_Penh",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BA","Asia/Bahrain",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"VM","Asia/Ho_Chi_Minh",ref list,ref i);

        ifCCeqCCaddZtoLISTandINC(countryCode,"VE","America/Caracas",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"BL","America/La_Paz",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CU","America/Havana",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"CI","America/Santiago",ref list,ref i);

        ifCCeqCCaddZtoLISTandINC(countryCode,"IS","Asia/Jerusalem",ref list,ref i);

        ifCCeqCCaddZtoLISTandINC(countryCode,"MX","America/Mexico_City",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"MU","Asia/Muscat",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"PM","America/Panama",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"PA","America/Asuncion",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"RP","Asia/Manila",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"PE","America/Lima",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SN","Asia/Singapore",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SO","Africa/Mogadishu",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SU","Africa/Khartoum",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SV","Arctic/Longyearbyen",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"AE","Asia/Dubai",ref list,ref i);
        ifCCeqCCaddZtoLISTandINC(countryCode,"SA","Asia/Riyadh",ref list,ref i);


        /*

MX - MEXICO		America/Mexico_City
MU - OMAN		Asia/Muscat

PM - PANAMA		America/Panama
PA - PARAGUAY	America/Asuncion
RP - PHILIPPINES	Asia/Manila
??QATAR
PE - PERU		America/Lima
SN - SINGAPORE	Asia/Singapore
SO - SOMALIA		Africa/Mogadishu
SU - SUDAN		Africa/Khartoum
SV - SVALBARD	Arctic/Longyearbyen
AE - UNITED ARAB EMIRATES	Asia/Dubai
SA - SAUDI ARABIA	Asia/Riyadh
         */

        return i;
    }
}
}




/*


  #Russia:
  #Europe/Moscow
  #Europe/Kaliningrad
  #Europe/Samara
  #Am/Dawson_Creek: Canada
  #Am/Thunder_Bay: Canada/Ontario

  #Am/Los_Angeles: California (CA) (Western)

  #Am/Boise: Idaho (ID) (mountain (left))
  #Am/Phoenix: Arizona (AZ) (Mountain (left))
  #Am/Denver: Colorado (CO) (Mountain (right))

  #Am/Chicago: ILlinois (IL)(Central)

  #Am/Louisville: Kentucky (KY) (Eastern (very left))
  #Am/New_York: New York   (NY) (Eastern (very east))
  #Am/Detroit: Michigan (MI)(Eastern (very left))

  #USA:
  #WA,OR,NV,CA - Pacific time:  "America/Los_Angeles (CA)" ??
  #AZ,CO,ID,MT,NM,UT,WY(Wyoming?MISSING!) - Montain time
  #ND,SD,NE,KS,OK,TX,,MN,IA,MO,AR,LA,,WI,IL,IN,TN,MS,AL - Central time
  #MI,OH,KY,GA,,PA,MD,WV,VA,NC,SC,FL,,NY,NJ,DE,,ME,MA,NH,VT,RI,CT - Eastern
  # Alaska: Alaska-time# Hawaii: Hawaii-time

DefaultZonesUsa = {
    'WA' => 'America/Los_Angeles',
    'OR' => 'America/Los_Angeles',
    'NV' => 'America/Los_Angeles',
    'CA' => 'America/Los_Angeles', #CA - Western
    'AZ' => 'America/Phoenix', #AZ - Mountain
    'ID' => 'America/Boise',
    'UT' => 'America/Boise',
    'MT' => 'America/Boise', #ID - Mountain
    'WY' => 'America/Denver',
    'CO' => 'America/Denver',
    'NM' => 'America/Denver', #CO - Mountain
    'ND' => 'America/Chicago',
    'SD' => 'America/Chicago',
    'NE' => 'America/Chicago',
    'KS' => 'America/Chicago',
    'OK' => 'America/Chicago',
    'TX' => 'America/Chicago',
    'MN' => 'America/Chicago',
    'IA' => 'America/Chicago',
    'MO' => 'America/Chicago',
    'AR' => 'America/Chicago',
    'LA' => 'America/Chicago',
    'WI' => 'America/Chicago',
    'IL' => 'America/Chicago',
    'TN' => 'America/Chicago',
    'MS' => 'America/Chicago',
    'AL' => 'America/Chicago', #IL - Central
    'IN' => 'America/Indiana/Indianapolis', #IN - central(summer)/eastern(winter)
    'MI' => 'America/Detroit',
    'OH' => 'America/Detroit', #MI - Eastern
    'KY' => 'America/Louisville',
    'GA' => 'America/Louisville',
    'FL' => 'America/Louisville',
    'WV' => 'America/Louisville',
    'VA' => 'America/Louisville',
    'NC' => 'America/Louisville',
    'SC' => 'America/Louisville', #KY - Eastern
    'NY' => 'America/New_York',
    'PA' => 'America/New_York',
    'MD' => 'America/New_York',
    'DE' => 'America/New_York',
    'NJ' => 'America/New_York',
    'CT' => 'America/New_York',
    'RI' => 'America/New_York',
    'VT' => 'America/New_York',
    'MA' => 'America/New_York',
    'NH' => 'America/New_York',
    'ME' => 'America/New_York',
    'DC' => 'America/New_York', #NY
    'HI' => 'Pacific/Honolulu'   #HI - Hawaii
  }


  DefaultZones = {
    'AL' => 'Europe/Tirane',      #Albania
    'AN' => 'Europe/Andorra',     #Andorra
    'AU' => 'Europe/Vienna',      #Austria
    'BE' => 'Europe/Brussels',    #Belgium
    'BK' => 'Europe/Sarajevo',    #Bosnia and Herzegovina
    'BO' => 'Europe/Minsk',       #Belarus
    'BU' => 'Europe/Sofia',       #Bulgaria
    'CY' => 'Europe/Nicosia',     #Cyprus
    'DA' => 'Europe/Copenhagen',  #Denmark
    'EI' => 'Europe/Dublin',      #Ireland
    'EN' => 'Europe/Tallinn',     #Estonia
    'EZ' => 'Europe/Prague',      #Check Rebublic
    'FI' => 'Europe/Helsinki',    #Finland
    'FR' => 'Europe/Paris',       #France
    'GI' => 'Europe/Gibraltar',   #Gibraltar
    'GR' => 'Europe/Athens',      #Greece
    'GM' => 'Europe/Berlin',      #Germany
    'HR' => 'Europe/Zagreb',      #Croatia
    'HU' => 'Europe/Budapest',    #Hungary
    'IC' => 'Atlantic/Reykjavik', #Iceland
    'IT' => 'Europe/Rome',        #Italy
    'LG' => 'Europe/Riga',        #Latvia
    'LH' => 'Europe/Vilnius',     #Lithuania
    'LO' => 'Europe/Bratislava',  #Slovakia
    'LS' => 'Europe/Vaduz',       #Liechtenstein
    'LU' => 'Europe/Luxembourg',  #Luxembourg
    'MD' => 'Europe/Chisinau',    #Moldova
    'MK' => 'Europe/Skopje',      #Macedonia
    'MN' => 'Europe/Monaco',      #Monaco
    'MT' => 'Europe/Malta',       #Malta
    'NL' => 'Europe/Amsterdam',   #Netherlands
    'NO' => 'Europe/Oslo',        #Norway
    'PO' => 'Europe/Lisbon',      #Portugal
    'PL' => 'Europe/Warsaw',      #Poland
    'RO' => 'Europe/Bucharest',   #Romania
    'SI' => 'Europe/Ljubljana',   #Slovenia
    'SP' => 'Europe/Madrid',      #Spain
    'SW' => 'Europe/Stockholm',   #Sweden
    'SZ' => 'Europe/Zurich',      #Switzerland
    'TU' => 'Europe/Istanbul',      #Turkey
    'UK' => 'Europe/London',      #United Kingdom

    'UP' => ['Europe/Simferopol', #Ukraine
      'Europe/Uzhgorod',
      'Europe/Zaporozhy'],
    'RS' => ['Europe/Moscow',      #Russia
      'Europe/Kaliningrad',
      'Asia/Irkutsk',
      'Asia/Krasnoyarsk',
      'Asia/Sakhalin',
      'Asia/Vladivostok',
      'Asia/Yakutsk'],

    'VT' => 'Europe/Vatican',     #Vatican City
    'YI' => 'Europe/Belgrade',    #Serbia and Montenegro

    'CA' => ['America/Whitehorse', #Canada
      'America/Vancouver',
      'America/Yellowknife',
      'America/Edmonton',
      'America/Regine',
      'America/Winnipeg',
      'America/Thunder_Bay',
      'America/Montreal',
      'America/Halifax',
      'America/St_Johns'],

    'IZ' => 'Asia/Baghdad',       #Iraq
    'JA' => 'Asia/Tokyo',         #Japan
    'KS' => 'Asia/Seoul',         #South Korea
    'PK' => 'Asia/Karachi',       #Pakistan

    'EG' => 'Africa/Cairo'       #Egypt
  }


    }
}
*/