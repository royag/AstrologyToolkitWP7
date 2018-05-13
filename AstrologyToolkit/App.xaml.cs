using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AstrologyToolkit.LeapingLightAstrology;
using System.IO.IsolatedStorage;
using System.IO;
using LeapingLight.Astrology;

namespace AstrologyToolkit
{
    public partial class App : Application
    {
        public static bool IsTrial
        {
            get;
            // setting the IsTrial property from outside is not allowed 
            private set;
        }

        public const int VERSION_1_0 = 100;
        public const int VERSION_1_1 = 110;
        public const int CURRENT_VERISON = VERSION_1_1;
        public static int[] VERSIONS = { VERSION_1_0, VERSION_1_1 };

        public string GetUpdateInfoAndUpdateVersion(int lastVersion)
        {
            if (lastVersion >= CURRENT_VERISON)
            {
                return null;
            }
            string info = "";
            if (lastVersion < VERSION_1_1)
            {
                info += "New in version 1.1:\n" +
                "House system choice and disabling/enabling aspects added under the 'settings' menu.\n" +
                    "Manually select latitude/longitude location by choosing '[CUSTOM LOCATION]' for 'Country' under 'edit chart'.";

            }
            setVersionSettings(CURRENT_VERISON);
            return info;
        }

        private void DetermineIsTrail()
        {
#if TRIAL 
            // return true if debugging with trial enabled (DebugTrial configuration is active) 
            IsTrial = true; 
#else
            var license = new Microsoft.Phone.Marketplace.LicenseInformation();
            IsTrial = license.IsTrial();
#endif
        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        public ChartData tempChartData { get; set; }
        private const string TEMP_CHART_DATA = "TEMP_CHART_DATA";
        private const string CURRENT_CHART_DATA = "CURRENT_CHART_DATA";
        private const string CURRENT_COMPARE_CHART_DATA = "CURRENT_COMPARE_CHART_DATA";
        private const string IS_COMPARE_CHART = "IS_COMPARE_CHART";
        private const string IS_TRANSIT_CHART = "IS_TRANSIT_CHART";
        private const string SELECTED_PLANET_NO = "SELECTED_PLANET_NO";
        private const string SELECTED_PLANET_IS_COMP = "SELECTED_PLANET_IS_COMP";
        private const string INFO_PANEL_VISIBLE = "INFO_PANEL_VISIBLE";
        private const string INFO_PANEL_SELECTED_IDX = "INFO_PANEL_SELECTED_IDX";
        private const string CHART_FILE_NAME = "CHART_FILE_NAME";
        private const string CHART_COMPARE_FILE_NAME = "CHART_COMPARE_FILE_NAME";
        private const string TEMP_SUGGESTED_TIMEZONES = "TEMP_SUGGESTED_TIMEZONES";
        private const string AUTOCHART_HERE_NOW = "AUTOCHART_HERE_NOW";
        private const string PREFERRED_HOUSE_SYSTEM = "PREFERRED_HOUSE_SYSTEM";
        private const string TEMP_LATITUDE = "TEMP_LATITUDE";
        private const string TEMP_LONGITUDE = "TEMP_LONGITUDE";
        private const string ASPECTS_ENABLED = "ASPECTS_ENABLED";
        public ChartData currentChartData { get; set; }
        public ChartData currentCompareChartData { get; set; }
        public bool IsCompareChart = false;
        public bool IsTransitChart = false;
        public int SelectedPlanetNo = -1;
        public bool SelectedPlanetIsComparison = false;
        public bool InfoPanelVisible = false;
        public int InfoPanelSelectedIndex = -1;
        public string chartFileName = null;
        public string chartCompareFileName = null;
        public string tempSuggestedTimezones = null;
        public bool AutoChartHereNow = true;
        public int PreferredHouseSystem = HouseCalc.HOUSES_PLACIDUS;
        public int TempLongitude = 0;
        public int TempLatitude = 0;
        public int[] AspectsEnabled = AstroDefs.ASPECTS;

        public const int GEO_SETTINGS_NOT_SET = -1;
        public const int GEO_SETTINGS_ALLOW = 1;
        public const int GEO_SETTINGS_DONOTALLOW = 0;

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            tempChartData = new ChartData();
            currentChartData = new ChartData();
            currentCompareChartData = null;
            IsCompareChart = false;
            IsTransitChart = false;
            DetermineIsTrail();
            AutoChartHereNow = true;
            PreferredHouseSystem = loadPreferredHouseSystem();
            ChartData.DefaultHouseSystem = PreferredHouseSystem;
            AspectsEnabled = loadAspectsEnabled();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            DetermineIsTrail();
            reloadCharts();
            ChartData.DefaultHouseSystem = PreferredHouseSystem;
            AspectsEnabled = loadAspectsEnabled();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            saveCharts();
        }

        private void saveCharts()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (tempChartData != null)
            {
                settings[TEMP_CHART_DATA] = tempChartData.serialize();
            }
            if (currentChartData != null)
            {
                settings[CURRENT_CHART_DATA] = currentChartData.serialize();
            }
            if (currentCompareChartData != null)
            {
                settings[CURRENT_COMPARE_CHART_DATA] = currentCompareChartData.serialize();
            }
            settings[IS_COMPARE_CHART] = IsCompareChart;
            settings[IS_TRANSIT_CHART] = IsTransitChart;
            settings[SELECTED_PLANET_NO] = SelectedPlanetNo;
            settings[SELECTED_PLANET_IS_COMP] = SelectedPlanetIsComparison;
            settings[INFO_PANEL_VISIBLE] = InfoPanelVisible;
            settings[INFO_PANEL_SELECTED_IDX] = InfoPanelSelectedIndex;
            settings[CHART_FILE_NAME] = chartFileName;
            settings[CHART_COMPARE_FILE_NAME] = chartCompareFileName;
            settings[TEMP_SUGGESTED_TIMEZONES] = tempSuggestedTimezones;
            settings[AUTOCHART_HERE_NOW] = AutoChartHereNow;
            settings[PREFERRED_HOUSE_SYSTEM] = PreferredHouseSystem;
            settings[TEMP_LATITUDE] = TempLatitude;
            settings[TEMP_LONGITUDE] = TempLongitude;

            string asps = "";
            bool first = true;
            foreach (int a in AspectsEnabled)
            {
                if (!first)
                {
                    asps += ";";
                }
                asps += a.ToString();
                first = false;
            }
            settings[ASPECTS_ENABLED] = asps;
        }

        private void reloadCharts()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            try
            {
                if (settings[TEMP_CHART_DATA] != null)
                {
                    tempChartData = new ChartData((string)settings[TEMP_CHART_DATA]);
                }
            }
            catch (KeyNotFoundException)
            {
            }
            try
            {
                if (settings[CURRENT_CHART_DATA] != null)
                {
                    currentChartData = new ChartData((string)settings[CURRENT_CHART_DATA]);
                }
            }
            catch (KeyNotFoundException)
            {
            }
            try
            {
                if (settings[CURRENT_COMPARE_CHART_DATA] != null)
                {
                    currentCompareChartData = new ChartData((string)settings[CURRENT_COMPARE_CHART_DATA]);
                }
            }
            catch (KeyNotFoundException)
            {
            }
            try
            {
                IsCompareChart = (bool)settings[IS_COMPARE_CHART];
                IsTransitChart = (bool)settings[IS_TRANSIT_CHART];
                SelectedPlanetNo = (int)settings[SELECTED_PLANET_NO];
                SelectedPlanetIsComparison = (bool)settings[SELECTED_PLANET_IS_COMP];
                InfoPanelVisible = (bool)settings[INFO_PANEL_VISIBLE];
                InfoPanelSelectedIndex = (int)settings[INFO_PANEL_SELECTED_IDX];
                chartFileName = (string)settings[CHART_FILE_NAME];
                chartCompareFileName = (string)settings[CHART_COMPARE_FILE_NAME];
                tempSuggestedTimezones = (string)settings[TEMP_SUGGESTED_TIMEZONES];
                AutoChartHereNow = (bool)settings[AUTOCHART_HERE_NOW];
                PreferredHouseSystem = (int)settings[PREFERRED_HOUSE_SYSTEM];
                TempLatitude = (int)settings[TEMP_LATITUDE];
                TempLongitude = (int)settings[TEMP_LONGITUDE];
                string aspsEn = (string)settings[ASPECTS_ENABLED];
                if (aspsEn != null)
                {
                    string[] asps = aspsEn.Split(new char[] { ';' });
                    int[] aa = new int[asps.Length];
                    for (int i = 0; i < asps.Length; i++)
                    {
                        int.TryParse(asps[i], out aa[i]);
                    }
                    AspectsEnabled = aa;
                }
                else
                {
                    AspectsEnabled = AstroDefs.ASPECTS;
                }
            }
            catch (KeyNotFoundException)
            {
            }
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        public int getGeoSettings()
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {

                    using (StreamReader r = new StreamReader(store.OpenFile("settings.dat", System.IO.FileMode.Open)))
                    {
                        string stringdata = r.ReadToEnd();
                        string[] settings = stringdata.Split(new char[] { '\n' });
                        int num = -1;
                        int.TryParse(settings[0], out num);
                        return num;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
            catch (IsolatedStorageException)
            {
                return -1;
            }
        }

        public void setGeoSettings(int state)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (StreamWriter sw = new StreamWriter(store.CreateFile("settings.dat")))
                    {
                        sw.Write(state.ToString());
                    }
                }
            }
            catch (IsolatedStorageException)
            {
            }
        }

        public int getVersionSettings()
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {

                    using (StreamReader r = new StreamReader(store.OpenFile("settings_version.dat", System.IO.FileMode.Open)))
                    {
                        string stringdata = r.ReadToEnd();
                        string[] settings = stringdata.Split(new char[] { '\n' });
                        int num = -1;
                        int.TryParse(settings[0], out num);
                        return num;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
            catch (IsolatedStorageException)
            {
                return -1;
            }
        }

        public void setVersionSettings(int version)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (StreamWriter sw = new StreamWriter(store.CreateFile("settings_version.dat")))
                    {
                        sw.Write(version.ToString());
                    }
                }
            }
            catch (IsolatedStorageException)
            {
            }
        }



        public int loadPreferredHouseSystem()
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {

                    using (StreamReader r = new StreamReader(store.OpenFile("settings_house.dat", System.IO.FileMode.Open)))
                    {
                        string stringdata = r.ReadToEnd();
                        string[] settings = stringdata.Split(new char[] { '\n' });
                        int num = -1;
                        int.TryParse(settings[0], out num);
                        return num;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return HouseCalc.HOUSES_PLACIDUS;
            }
            catch (IsolatedStorageException)
            {
                return HouseCalc.HOUSES_PLACIDUS;
            }
        }

        public void savePreferredHouseSystem(int houseSystem)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (StreamWriter sw = new StreamWriter(store.CreateFile("settings_house.dat")))
                    {
                        sw.Write(houseSystem.ToString());
                        PreferredHouseSystem = houseSystem;
                        ChartData.DefaultHouseSystem = PreferredHouseSystem;
                    }
                }
            }
            catch (IsolatedStorageException)
            {
            }
        }

        public void setAspect(int asp, bool enabled)
        {
            IList<int> asps = new List<int>(AspectsEnabled);
            if (enabled)
            {
                if (!asps.Contains(asp))
                {
                    asps.Add(asp);
                }
            }
            else
            {
                if (asps.Contains(asp))
                {
                    asps.Remove(asp);
                }
            }
            saveAspectsEnabled(asps.ToArray());
        }

        public void saveAspectsEnabled(int[] asps)
        {
            string s = "";
            bool first = true;
            foreach (int a in asps)
            {
                if (!first)
                {
                    s += ";";
                }
                s += a.ToString();
                first = false;
            }
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (StreamWriter sw = new StreamWriter(store.CreateFile("settings_asps.dat")))
                    {
                        sw.Write(s);
                    }
                }
            }
            catch (IsolatedStorageException)
            {
            }
            AspectsEnabled = asps;
        }

        public int[] loadAspectsEnabled() {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (StreamReader r = new StreamReader(store.OpenFile("settings_asps.dat", System.IO.FileMode.Open)))
                    {
                        string stringdata = r.ReadToEnd();
                        string[] asps = stringdata.Split(new char[] { ';' });
                        int[] ret = new int[asps.Length];
                        for (int i = 0; i < asps.Length; i++)
                        {
                            int.TryParse(asps[i], out ret[i]);
                        }
                        return ret;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return AstroDefs.ASPECTS;
            }
            catch (IsolatedStorageException)
            {
                return AstroDefs.ASPECTS;
            }
        }
        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}