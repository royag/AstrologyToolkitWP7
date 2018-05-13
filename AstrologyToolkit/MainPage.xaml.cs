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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using LeapingLight.Astrology;
using AstrologyToolkit.LeapingLightAstrology;
using System.Windows.Navigation;
using System.IO.IsolatedStorage;
using Microsoft.Devices;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Device.Location;

namespace AstrologyToolkit
{
    public partial class MainPage : PhoneApplicationPage
    {

        const string TEXT_TAP_ONE_LINE = "Tap one of the lines above to enable search...";
        // Constructor

        private Image[] signImages;
        private Rectangle[] planetImages;
        private Rectangle[] compPlanetImages;

        GeoCoordinateWatcher gcw = null;

        static int zodiacWidth = 40;

        private bool chartHereCancelled = false;
        protected void chartHereAndNow() {
            chartHereCancelled = false;
            gcw = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
			gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
			gcw.Start();
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            ((App)App.Current).AutoChartHereNow = false;
            //stop & clean up, we don’t need it anymore
            if (gcw == null)
            {
                return;
            }
            gcw.Stop();
            gcw.Dispose();
            gcw = null;
            if (!chartHereCancelled)
            {
                Chart = new ChartData();
                Chart.Latitude = (float)e.Position.Location.Latitude;
                Chart.Longitude = (float)e.Position.Location.Longitude;
                Chart.CountryCode = "";
                Chart.CountryName = "Here";
                Chart.PlaceName = "Here";
                updateChart();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            infoPanel.Visibility = Visibility.Collapsed;

            signImages = new Image[12];
            signImages[0] = SignAries;
            signImages[1] = SignTaurus;
            signImages[2] = SignGemini;
            signImages[3] = SignCancer;
            signImages[4] = SignLeo;
            signImages[5] = SignVirgo;
            signImages[6] = SignLibra;
            signImages[7] = SignScorpio;
            signImages[8] = SignSaggitarius;
            signImages[9] = SignCapricorn;
            signImages[10] = SignAquarius;
            signImages[11] = SignPisces;

            planetImages = new Rectangle[13];
            planetImages[0] = PlanetSun;
            planetImages[1] = PlanetMoon;
            planetImages[2] = PlanetMercury;
            planetImages[3] = PlanetVenus;
            planetImages[4] = PlanetMars;
            planetImages[5] = PlanetJupiter;
            planetImages[6] = PlanetSaturn;
            planetImages[7] = PlanetUranus;
            planetImages[8] = PlanetNeptune;
            planetImages[9] = PlanetPluto;
            planetImages[10] = PlanetNode;
            planetImages[11] = PlanetAsc;
            planetImages[12] = PlanetMC;

            compPlanetImages = new Rectangle[13];
            compPlanetImages[0] = CompPlanetSun;
            compPlanetImages[1] = CompPlanetMoon;
            compPlanetImages[2] = CompPlanetMercury;
            compPlanetImages[3] = CompPlanetVenus;
            compPlanetImages[4] = CompPlanetMars;
            compPlanetImages[5] = CompPlanetJupiter;
            compPlanetImages[6] = CompPlanetSaturn;
            compPlanetImages[7] = CompPlanetUranus;
            compPlanetImages[8] = CompPlanetNeptune;
            compPlanetImages[9] = CompPlanetPluto;
            compPlanetImages[10] = CompPlanetNode;
            compPlanetImages[11] = CompPlanetAsc;
            compPlanetImages[12] = CompPlanetMC;


            initChartElements();

            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBarIconButton button1 = new ApplicationBarIconButton(new Uri("/Toolkit.Content/appbar.page.edit.png", UriKind.Relative));
            button1.Text = "Edit Chart";
            button1.Click += editButton_Click;
            ApplicationBar.Buttons.Add(button1);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton(new Uri("/Toolkit.Content/appbar.book.png", UriKind.Relative));
            button2.Text = "Open";
            button2.Click += openButton_Click;
            ApplicationBar.Buttons.Add(button2);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton(new Uri("/Toolkit.Content/appbar.disk.png", UriKind.Relative));
            button3.Text = "Save as";
            button3.Click += saveButton_Click;
            ApplicationBar.Buttons.Add(button3);

            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem("Compare to...");
            menuItem1.Click += compareButton_Click;
            ApplicationBar.MenuItems.Add(menuItem1);
            ApplicationBarMenuItem menuItem2 = new ApplicationBarMenuItem("Transits on/off");
            menuItem2.Click += transitButton_Click;
            ApplicationBar.MenuItems.Add(menuItem2);
            ApplicationBarMenuItem menuItem3 = new ApplicationBarMenuItem("Open VIP chart...");
            menuItem3.Click += openVipButton_Click;
            ApplicationBar.MenuItems.Add(menuItem3);
            ApplicationBarMenuItem menuItem4 = new ApplicationBarMenuItem("Compare to VIP chart...");
            menuItem4.Click += compareVipButton_Click;
            ApplicationBar.MenuItems.Add(menuItem4);
            ApplicationBarMenuItem menuItem5 = new ApplicationBarMenuItem("animate on/off");
            menuItem5.Click += animateButton_Click;
            ApplicationBar.MenuItems.Add(menuItem5);
            ApplicationBarMenuItem menuItem6 = new ApplicationBarMenuItem("about / review / more apps");
            menuItem6.Click += aboutButton_Click;
            ApplicationBar.MenuItems.Add(menuItem6);
            ApplicationBarMenuItem menuItem7 = new ApplicationBarMenuItem("settings");
            menuItem7.Click += settingsButton_Click;
            ApplicationBar.MenuItems.Add(menuItem7);

            Loaded += MainPage_Loaded;

        }


  private void MainPage_Loaded(object sender, RoutedEventArgs e)
 {
     Dispatcher.BeginInvoke(() =>
     {

         App app = (App)App.Current;
         if (app.AutoChartHereNow)
         {
             int geoSettings = app.getGeoSettings();
             if (geoSettings == -1)
             {
                 MessageBoxResult m = MessageBox.Show("Astrology Toolkit will use your location on startup to draw a horoscope for your current time and place.\n" +
                     "Your location is used for this ONLY, and is NOT sent or collected anywhere by Leaping Light. NB: Microsoft might still collect the location beyond our control. If you won't allow this, press CANCEL, and Greenwich/UK will be used as your location instead.\n" +
                     "You may change this at any time under the 'settings' menu choice.",
                     "Chart Here and Now", MessageBoxButton.OKCancel);
                 if (m == MessageBoxResult.OK)
                 {
                     app.setGeoSettings(1);
                 }
                 else
                 {
                     app.setGeoSettings(0);
                 }
                 geoSettings = app.getGeoSettings();
                 // Never run before, just set version, with no "updateInfo"
                 app.setVersionSettings(App.CURRENT_VERISON);
             }
             else
             {
                 // App HAS been used before. Let's check version.
                 
                 int ver = app.getVersionSettings(); // -1 if version 1.0 (since there was no versionSettings then)
                 if (ver < App.CURRENT_VERISON)
                 {
                     string versionInfo = app.GetUpdateInfoAndUpdateVersion(ver);
                     MessageBoxResult m = MessageBox.Show(versionInfo,
                     "Astrology Toolkit updated!", MessageBoxButton.OK);
                 }
                 
             }
             if (geoSettings == 1)
             {
                 chartHereAndNow();
             }
         }
     });
 }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            updateChart();


            
        }


        private ChartData Chart
        {
            get
            {
                return ((App)App.Current).currentChartData;
            }
            set
            {
                ((App)App.Current).currentChartData = value;
            }
        }
        private ChartData ComparisonChart
        {
            get
            {
                return ((App)App.Current).currentCompareChartData;
            }
        }

        private string makeChartInfoString()
        {
            TZInfo.SimpleTime utcTime = Chart.CalculatedUtcTime;
            string utc = utcTime.year.ToString() + "/" +
                utcTime.month.ToString().PadLeft(2,'0') + "/" +
                utcTime.day.ToString().PadLeft(2, '0') + " " +
                utcTime.hour.ToString().PadLeft(2, '0') + ":" + utcTime.minute.ToString().PadLeft(2, '0');


            string s = Chart.TimeString;
            s += "         (" + utc + " UTC)";
            s += "\n";
            s += "Zone: " + Chart.TimeZone.Trim() + " (" + HouseCalc.HOUSE_SYSTEM_NAMES[Chart.HouseSystem] + " houses)";
            s += "\n";
            s += Chart.PlaceDescription + " (" +
                GeoData.coordinatesToString(Chart.Latitude, Chart.Longitude)
                + ")";
            s += "\n";

            if (ChartFileName != null)
            {
                s += System.IO.Path.GetFileNameWithoutExtension(ChartFileName) + " ";
            }
            if (IsCompareChart)
            {
                string compareTime = ComparisonChart.Year.ToString() + "/" +
                    ComparisonChart.Month.ToString().PadLeft(2, '0') + "/" +
                    ComparisonChart.Day.ToString().PadLeft(2, '0') + " " +
                    ComparisonChart.Hour.ToString().PadLeft(2, '0') + ":" + ComparisonChart.Minute.ToString().PadLeft(2, '0');
                if (IsTransitChart)
                {
                     s += "Transits: " + compareTime + " (" + ComparisonChart.TimeZone + ")";
                }
                else
                {
                    s += "vs " + compareTime;
                    if (ChartCompareFileName != null)
                    {
                        s += " (" + System.IO.Path.GetFileNameWithoutExtension(ChartCompareFileName) + ")";
                    }
                    else
                    {
                        s += " (" + ComparisonChart.TimeZone + ")"; // might be progressed chart
                    }
                }
            }

            return s;
        }

        Line m_axisAscDesc;
        Line m_axisMcIc;
        Line m_axis2nd8th;
        Line m_axis3rd9th;
        Line m_axis5th11th;
        Line m_axis6th12th;
        Line[] m_zodLines;
        Line[][] m_aspectLines;

        Line[] m_transitAspectLines;
        Line[] m_planetMarkers;
        Line[] m_compPlanetMarkers;

        TextBlock[] houseNumbers;


        private void initChartElements()
        {
            Brush brushHalfFG = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            Brush brushFG = Application.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush; //new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            ChartCanvas.Children.Add(m_axisAscDesc = new Line());
            m_axisAscDesc.Stroke = brushFG;
            ChartCanvas.Children.Add(m_axisMcIc = new Line());
            m_axisMcIc.Stroke = brushFG;
            ChartCanvas.Children.Add(m_axis2nd8th = new Line());
            m_axis2nd8th.Stroke = brushHalfFG;
            ChartCanvas.Children.Add(m_axis3rd9th = new Line());
            m_axis3rd9th.Stroke = brushHalfFG;
            ChartCanvas.Children.Add(m_axis5th11th = new Line());
            m_axis5th11th.Stroke = brushHalfFG;
            ChartCanvas.Children.Add(m_axis6th12th = new Line());
            m_axis6th12th.Stroke = brushHalfFG;

            m_axisAscDesc.Stroke = brushFG;

            m_zodLines = new Line[12];
            for (int i = 0; i < 12; i++)
            {
                ChartCanvas.Children.Add(m_zodLines[i] = new Line());
                m_zodLines[i].Stroke = brushFG;
            }

            const int numBodies = AstroDefs.NUM_PLANETS + 2;
            m_aspectLines = new Line[numBodies][];
            m_transitAspectLines = new Line[numBodies];
            for (int p1 = 0; p1 < numBodies; p1++)
            {
                ChartCanvas.Children.Add(m_transitAspectLines[p1] = new Line());
                m_transitAspectLines[p1].Visibility = Visibility.Collapsed;
                m_aspectLines[p1] = new Line[numBodies];
                for (int p2 = 0/*p1+1*/; p2 < numBodies; p2++)
                {
                    ChartCanvas.Children.Add(m_aspectLines[p1][p2] = new Line());
                    m_aspectLines[p1][p2].Visibility = Visibility.Collapsed;
                }
            }
            m_planetMarkers = new Line[AstroDefs.NUM_PLANETS];
            m_compPlanetMarkers = new Line[AstroDefs.NUM_PLANETS];
            for (int i = 0; i < AstroDefs.NUM_PLANETS; i++)
            {
                ChartCanvas.Children.Add(m_planetMarkers[i] = new Line());
                m_planetMarkers[i].Visibility = Visibility.Visible;
                m_planetMarkers[i].Stroke = Application.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
                ChartCanvas.Children.Add(m_compPlanetMarkers[i] = new Line());
                m_compPlanetMarkers[i].Visibility = Visibility.Collapsed;
                m_compPlanetMarkers[i].Stroke = Application.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
            }

            houseNumbers = new TextBlock[12];
            for (int h = 0; h <= 11; h++)
            {
                houseNumbers[h] = new TextBlock();
                int element = AstroDefs.SIGN_AND_HOUSE_ELEMENTS[h];
                houseNumbers[h].Foreground = new SolidColorBrush(colorForElement(element));
                houseNumbers[h].Text = (h + 1).ToString();
                houseNumbers[h].FontSize = 24;
                houseNumbers[h].FontWeight = FontWeights.Bold;
                houseNumbers[h].Visibility = Visibility.Visible;
                ChartCanvas.Children.Add(houseNumbers[h]);
            }
        }

        AstroDefs.planet_positions calcPresentablePositions(AstroDefs.planet_positions realPos, int minDistance)
        {
            AstroDefs.planet_positions ret = AstroDefs.Duplicate(realPos);

            float pos1;
            float pos2;
            float dist;
            bool allOk = false;
            int cnt = 0;
            int maxCnt = 10;
            // TODO: Figure out better algorithm ?
            while (!allOk) {
                allOk = true;
                for (int p1 = AstroDefs.SUN; p1 <= AstroDefs.NODE; p1++)
                {
                    pos1 = AstroDefs.ephem_positionOfPlanet(ret, p1);
                    for (int p2 = AstroDefs.SUN; p2 <= AstroDefs.NODE; p2++)
                    {
                        if (p1 == p2) {
                            continue;
                        }
                        pos2 = AstroDefs.ephem_positionOfPlanet(ret, p2);
                        dist = AstroDefs.calcDistance(pos1, pos2);
                        if (dist < minDistance) {
                            allOk = false;
                            float delta = minDistance - dist;
                            float p1attempt = AstroDefs.filter360(pos1 - delta / 2 - 0.1f);
                            float p2attempt = AstroDefs.filter360(pos2 + delta / 2 + 0.1f);
                            if (!(AstroDefs.calcDistance(p1attempt, p2attempt) >= minDistance))
                            {
                                p1attempt = AstroDefs.filter360(pos1 + delta / 2 + 0.1f);
                                p2attempt = AstroDefs.filter360(pos2 - delta / 2 - 0.1f);
                            }
                            AstroDefs.ephem_setPosition(ref ret, p1, p1attempt);
                            AstroDefs.ephem_setPosition(ref ret, p2, p2attempt);
                        }
                    }

                }
                cnt++;
                if (cnt >= maxCnt) {
                    break;
                }
            }
            //qDebug("presentationAttemts=%d", cnt);
            //qDebug("calcPresentablePositions DONE");
            return ret;
        }


        private void setLine(ref Line line, int x, int y, int x2, int y2)
        {
            line.X1 = x;
            line.Y1 = y;
            line.X2 = x2;
            line.Y2 = y2;
        }

        private void fixAxisLines(float[] houses) 
        {

            int x = 0; int y = 0;
            int x2 = 0; int y2 = 0;

            int aries0 = 360 - (int)houses[HouseCalc.HOUSE_1];

            int m_radius = (int)ZodiacOuter.ActualWidth / 2;
            int m_zodInnerRadius = (int)ZodiacInner.ActualWidth / 2;

            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_4] + aries0), m_radius, ref x, ref y);
            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_10] + aries0), m_radius, ref x2, ref y2);
            setLine(ref m_axisMcIc, x, y, x2, y2);

            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_2] + aries0), m_zodInnerRadius, ref x, ref y);
            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_8] + aries0), m_zodInnerRadius, ref x2, ref y2);
            setLine(ref m_axis2nd8th, x, y, x2, y2);

            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_3] + aries0), m_zodInnerRadius, ref x, ref y);
            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_9] + aries0), m_zodInnerRadius, ref x2, ref y2);
            setLine(ref m_axis3rd9th, x, y, x2, y2);

            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_5] + aries0), m_zodInnerRadius, ref x, ref y);
            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_11] + aries0), m_zodInnerRadius, ref x2, ref y2);
            setLine(ref m_axis5th11th, x, y, x2, y2);

            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_6] + aries0), m_zodInnerRadius, ref x, ref y);
            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_12] + aries0), m_zodInnerRadius, ref x2, ref y2);
            setLine(ref m_axis6th12th, x, y, x2, y2);

            //setLine(ref m_axisAscDesc, -m_radius, 0, m_radius, 0);
            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_1] + aries0), m_radius, ref x, ref y);
            getXY(AstroDefs.filter360(houses[HouseCalc.HOUSE_7] + aries0), m_radius, ref x2, ref y2);
            setLine(ref m_axisAscDesc, x, y, x2, y2);
            //setLine(ref m_axisAscDesc, 0,m_radius,m_radius*2,m_radius);

            for (int i = 0; i < 12; i++)
            {
                getXY(aries0 + i * 30, m_radius, ref x, ref y);
                getXY(aries0 + i * 30, m_zodInnerRadius, ref x2, ref y2);
                setLine(ref m_zodLines[i], x, y, x2, y2);
            }

            for (int h = HouseCalc.HOUSE_1; h <= HouseCalc.HOUSE_12; h++)
            {
                int nextHouse = (h == HouseCalc.HOUSE_12 ? HouseCalc.HOUSE_1 : h + 1);
                float dist = AstroDefs.calcDistance(houses[h], houses[nextHouse]);
                int center = aries0 + (int)AstroDefs.filter360(houses[h] + (dist * 0.5f));
                int rad = m_radius / 2;
                centerAtDegRadius(center, rad, houseNumbers[h]); ;
            }
        }

        private bool chartHasHouses()
        {
            return true;
        }
        private bool comparisonChartHasHouses()
        {
            return !IsTransitChart;
            //return false;
        }
        public bool IsCompareChart
        {
            set
            {
                ((App)App.Current).IsCompareChart = value;
            }
            get
            {
                return((App)App.Current).IsCompareChart;
            }
        }
        public bool IsTransitChart
        {
            set
            {
                ((App)App.Current).IsTransitChart = value;
            }
            get
            {
                return ((App)App.Current).IsTransitChart;
            }
        }
        public bool SelectedPlanetIsComparison
        {
            set
            {
                ((App)App.Current).SelectedPlanetIsComparison = value;
            }
            get
            {
                return ((App)App.Current).SelectedPlanetIsComparison;
            }
        }
        public int SelectedPlanetNo
        {
            set
            {
                ((App)App.Current).SelectedPlanetNo = value;
            }
            get
            {
                return ((App)App.Current).SelectedPlanetNo;
            }
        }

        public string ChartFileName
        {
            set
            {
                ((App)App.Current).chartFileName = value;
            }
            get
            {
                return ((App)App.Current).chartFileName;
            }
        }
        public string ChartCompareFileName
        {
            set
            {
                ((App)App.Current).chartCompareFileName = value;
            }
            get
            {
                return ((App)App.Current).chartCompareFileName;
            }
        }
        
        public bool InfoPanelVisible
        {
            set
            {
                ((App)App.Current).InfoPanelVisible = value;
            }
            get
            {
                return ((App)App.Current).InfoPanelVisible;
            }
        }
        public int InfoPanelSelectedIdx
        {
            set
            {
                ((App)App.Current).InfoPanelSelectedIndex = value;
            }
            get
            {
                return ((App)App.Current).InfoPanelSelectedIndex;
            }
        }

        int aspectOrNotEnabled(int asp)
        {
            if (((App)App.Current).AspectsEnabled.Contains(asp))
            {
                return asp;
            }
            else
            {
                return -1;
            }
        }

        int getNatalAspect(float p1, float p2)
        {
            int a = AstroDefs.getAspect(p1, p2, AstroDefs.DEFAULT_ORBS);
            return aspectOrNotEnabled(a);
        }

        int getSynastryAspect(float p1, float p2)
        {
            int a = AstroDefs.getAspect(p1, p2, AstroDefs.DEFAULT_TRANSIT_ORBS);
            return aspectOrNotEnabled(a);
        }

        int getTransitAspect(float p1, float p2)
        {
            int a = AstroDefs.getAspect(p1, p2, AstroDefs.DEFAULT_TRANSIT_ORBS);
            return aspectOrNotEnabled(a);
        }

        public static Color colorForElement(int element)
        {
            byte n = 64;
            byte alpha = 128; // 255;
            switch (element)
            {
                case AstroDefs.ELEM_FIRE: return Color.FromArgb(alpha, 255, n, n); 
                case AstroDefs.ELEM_WATER: return Color.FromArgb(alpha, n, n, 255);
                case AstroDefs.ELEM_AIR: return Color.FromArgb(alpha, 255, 255, n);
                case AstroDefs.ELEM_EARTH: return Color.FromArgb(alpha, n, 255, n);
                default: return Color.FromArgb(alpha, n, n, n);
            }

        }
        public static Color colorForAspect(int aspect) {
            byte n = 64;
            byte alpha = 255;
            switch (aspect) {
                case 0: return Color.FromArgb(alpha, n, n, 255); //=> 0x0000FFFF, #[0,0,255],
                case 30: return Color.FromArgb(alpha, n, n, 255); // => 0x0000FFFF, #[0,0,255],
                case 45: return Color.FromArgb(alpha, 255, n, n); // => 0xFF0000FF, #[255,0,0],
                case 60: return Color.FromArgb(alpha, n, n, 255); // => 0x0000FFFF, #[0,0,255],
                case 72: return Color.FromArgb(alpha, n, n, n); // => 0x000000FF, #[0,0,0],
                case 90: return Color.FromArgb(alpha, 255, n, n); // => 0xFF0000FF, #[255,0,0],
                case 120: return Color.FromArgb(alpha, n, n, 255); // => 0x0000FFFF, #[0,0,255],
                case 135: return Color.FromArgb(alpha, n, 255, n); // => 0x00FF00FF, #[0,255,0],
                case 150: return Color.FromArgb(alpha, n, 255, n); //=> 0x00FF00FF, #[0,255,0],
                case 180: return Color.FromArgb(alpha, 255, n, n); // => 0xFF0000FF #[255,0,0]
                default: return Color.FromArgb(alpha, n, n, n); ;
            }
        }

        AstroDefs.planet_positions CurrentPositions;
        AstroDefs.planet_positions CurrentComparisonPositions;
        float[] Houses;

        int houseOfPosition(float pos)
        {
            for (int i = HouseCalc.HOUSE_1; i <= HouseCalc.HOUSE_12; i++)
            {
                float cur = Houses[i];
                float next = (i == HouseCalc.HOUSE_12 ? Houses[HouseCalc.HOUSE_1] : Houses[i + 1]);
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



        #region ChartDrawing functions (fixAspectLines,updateChart,centerAtDegRadius,getXY,DegreeToRadian)
        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        void getXY(float degree, int radius, ref int x, ref int y)
        {
            int centerX = ((int)ChartCanvas.ActualWidth) / 2;
            int centerY = ((int)ChartCanvas.ActualHeight) / 2;
            degree -= 180;
            if (degree < 0) { degree += 360; }
            if (degree > 360) { degree -= 360; }
            double sinPos = Math.Sin(DegreeToRadian(degree));
            double cosPos = Math.Cos(DegreeToRadian(degree));
            x = centerX + (int)(0 + (cosPos * radius));
            y = centerY - (int)(0 + (sinPos * radius));
        }
        void centerAtDegRadius(int deg, int radius, FrameworkElement im)
        {
            //Ellipse;
            int x = 0; int y = 0;
            getXY(deg, radius, ref x, ref y);
            int w = (int)im.ActualWidth;
            int h = (int)im.ActualHeight;
            Canvas.SetLeft(im, x - (w / 2));
            Canvas.SetTop(im, y - (h / 2));
        }
        private void fixAspectLines(int planetRadius, int aries0, AstroDefs.planet_positions pp, AstroDefs.planet_positions pp2)
        {
            bool showComparisonAspects = IsCompareChart && !IsTransitChart;

            // aspects:
            int aspectRadius = planetRadius - 10;
            const int numBodies = AstroDefs.NUM_PLANETS + 2;

            for (int n1 = 0; n1 < numBodies; n1++)
            {
                if (SelectedPlanetIsComparison && SelectedPlanetNo > -1 && IsTransitChart)
                {
                    // Show any transit aspects for this planet
                    float natPos = AstroDefs.ephem_positionOfPlanet(pp, n1);
                    float transPos = AstroDefs.ephem_positionOfPlanet(pp2, SelectedPlanetNo);
                    int aspect = getTransitAspect(natPos, transPos);
                    if (aspect < 0)
                    {
                        m_transitAspectLines[n1].Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        int x = 0; int y = 0;
                        int x2 = 0; int y2 = 0;
                        int natRadius = aspectRadius;
                        int transRadius = natRadius + 30;
                        getXY(aries0 + natPos, natRadius, ref x, ref y);
                        getXY(aries0 + transPos, transRadius, ref x2, ref y2);
                        setLine(ref m_transitAspectLines[n1], x, y, x2, y2);
                        m_transitAspectLines[n1].Stroke = new SolidColorBrush(colorForAspect(aspect));
                        m_transitAspectLines[n1].StrokeThickness = 4;
                        m_transitAspectLines[n1].Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    m_transitAspectLines[n1].Visibility = Visibility.Collapsed;
                }
            }

            for (int p1 = 0; p1 < numBodies; p1++)
            {
                for (int p2 = 0; p2 < numBodies; p2++)
                {
                    if (!showComparisonAspects && (p2 <= p1))
                    {
                        m_aspectLines[p1][p2].Visibility = Visibility.Collapsed;
                        continue;
                    }
                    if (!showComparisonAspects && (p1 >= AstroDefs.NUM_PLANETS || p2 >= AstroDefs.NUM_PLANETS) && !chartHasHouses())
                    {
                        m_aspectLines[p1][p2].Visibility = Visibility.Collapsed;
                        continue;
                    }
                    else if (showComparisonAspects)
                    {
                        if ((p1 >= AstroDefs.NUM_PLANETS && !chartHasHouses()) ||
                            (p2 >= AstroDefs.NUM_PLANETS && !comparisonChartHasHouses()))
                        {
                            m_aspectLines[p1][p2].Visibility = Visibility.Collapsed;
                            continue;
                        }
                    }
                    float pos1 = AstroDefs.ephem_positionOfPlanet(pp, p1);
                    float pos2;
                    if (!showComparisonAspects)
                    {
                        pos2 = AstroDefs.ephem_positionOfPlanet(pp, p2);
                    }
                    else
                    {
                        pos2 = AstroDefs.ephem_positionOfPlanet(pp2, p2);
                    }

                    int aspect;
                    if (!showComparisonAspects)
                    {
                        aspect = getNatalAspect(pos1, pos2);
                    }
                    else
                    {
                        if (IsTransitChart)
                        {
                            aspect = getTransitAspect(pos1, pos2);
                        }
                        else
                        {
                            aspect = getSynastryAspect(pos1, pos2);
                        }
                    }

                    if (aspect >= 0)
                    {
                        int x = 0; int y = 0;
                        int x2 = 0; int y2 = 0;
                        int radius = aspectRadius;
                        if (showComparisonAspects)
                        {
                            radius = aspectRadius - 10;
                        }
                        getXY(aries0 + pos1, radius, ref x, ref y);
                        getXY(aries0 + pos2, radius, ref x2, ref y2);
                        setLine(ref m_aspectLines[p1][p2], x, y, x2, y2);

                        bool thickLine = false;
                        if (showComparisonAspects && SelectedPlanetIsComparison && p2 == SelectedPlanetNo && showComparisonAspects)
                        {
                            thickLine = true;
                        }
                        else if (!SelectedPlanetIsComparison && (p1 == SelectedPlanetNo) || (p2 == SelectedPlanetNo && !showComparisonAspects && !SelectedPlanetIsComparison))
                        {
                            thickLine = true;
                        }
                        m_aspectLines[p1][p2].Stroke = new SolidColorBrush(colorForAspect(aspect));
                        m_aspectLines[p1][p2].StrokeThickness = (thickLine ? 5 : 1);
                        //m_aspectLines[p1][p2]->update();
                        m_aspectLines[p1][p2].Visibility = Visibility.Visible; //->setVisible(true);
                    }
                    else
                    {
                        m_aspectLines[p1][p2].Visibility = Visibility.Collapsed;
                        //m_aspectLines[p1][p2]->setVisible(false);
                    }
                }
            }

        }
        private void updateChart()
        {
            centerAtDegRadius(0, 0, ZodiacOuter);
            centerAtDegRadius(0, 0, ZodiacInner);

            infoPanel.Margin = ChartCanvas.Margin;
            infoPanel.Height = ChartCanvas.ActualWidth;
            infoPanel.Width = ChartCanvas.ActualWidth;

            ChartData chart = ((App)App.Current).currentChartData;
            try
            {
                CurrentPositions = chart.CalculatePositions();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Error", "Invalid or Unsupported Chart Data", MessageBoxButton.OK);
                return;
            }
            Houses = chart.CalculateHouses(CurrentPositions);
            fixAxisLines(Houses);

            ChartData chart2 = ((App)App.Current).currentCompareChartData;
            CurrentComparisonPositions = CurrentPositions;
            if (chart2 != null)
            {
                try
                {
                    CurrentComparisonPositions = chart2.CalculatePositions();
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Error", "Invalid or Unsupported Chart Data", MessageBoxButton.OK);
                    return;
                }
            }

            ChartInfo.Text = makeChartInfoString(); // uses calculated Utc-time, so must call after calculation

            int aries0 = 360 - (int)Houses[HouseCalc.HOUSE_1];
            int m_radius = (int)ChartCanvas.ActualWidth / 2;
            int signRadius = m_radius - 20;
            for (int i = AstroDefs.ARIES; i <= AstroDefs.PISCES; i++)
            {
                centerAtDegRadius(aries0 + 15 + (i * 30), signRadius, signImages[i]);
            }
            int planetRadius = signRadius - 40;


            fixAspectLines(planetRadius, aries0, CurrentPositions, CurrentComparisonPositions);
            int minDistanceNatal = 7;
            AstroDefs.planet_positions presPos = calcPresentablePositions(CurrentPositions, minDistanceNatal);

            for (int i = AstroDefs.SUN; i <= AstroDefs.MEDIUM_COELI; i++)
            {
                int radius = (i == AstroDefs.ASCENDANT || i == AstroDefs.MEDIUM_COELI ? planetRadius + 30 : planetRadius);
                int deg = (int)(AstroDefs.ephem_positionOfPlanet(presPos, i) + aries0); // presPos
                int degActual = (int)(AstroDefs.ephem_positionOfPlanet(CurrentPositions, i) + aries0);
                centerAtDegRadius(deg, radius, planetImages[i]);

                if (i < AstroDefs.ASCENDANT)
                {
                    int x = 0; int y = 0;
                    int x2 = 0; int y2 = 0;
                    int lineOuter = m_radius - zodiacWidth;
                    int lineInner = lineOuter - 5;
                    getXY(degActual, lineOuter, ref x, ref y);
                    getXY(degActual, lineInner, ref x2, ref y2);
                    setLine(ref m_planetMarkers[i], x, y, x2, y2);
                }


            }
            for (int i = 0; i < compPlanetImages.Length; i++)
            {
                compPlanetImages[i].Visibility = Visibility.Collapsed;
            }
            for (int i = 0; i < m_compPlanetMarkers.Length; i++) {
                m_compPlanetMarkers[i].Visibility = Visibility.Collapsed;
            }

            if (IsCompareChart)
            {
                int minDistance = (IsTransitChart ? 5 : 10);
                AstroDefs.planet_positions presPos2 = calcPresentablePositions(CurrentComparisonPositions, minDistance);
                int comparisonRadius = (IsTransitChart ? planetRadius + zodiacWidth : planetRadius - 30);
                int lastPlanet = (comparisonChartHasHouses() ? AstroDefs.MEDIUM_COELI : AstroDefs.NODE);
                for (int i = AstroDefs.SUN; i <= lastPlanet; i++)
                {
                    int deg = (int)(AstroDefs.ephem_positionOfPlanet(presPos2, i) + aries0);
                    int degActual = (int)(AstroDefs.ephem_positionOfPlanet(CurrentComparisonPositions, i) + aries0);
                    centerAtDegRadius(deg, comparisonRadius, compPlanetImages[i]);
                    compPlanetImages[i].Visibility = Visibility.Visible;
                    if (i < AstroDefs.ASCENDANT)
                    {
                        int x = 0; int y = 0;
                        int x2 = 0; int y2 = 0;
                        int lineOuter = m_radius - zodiacWidth + 2;
                        int lineInner = lineOuter - 5;
                        getXY(degActual, lineOuter, ref x, ref y);
                        getXY(degActual, lineInner, ref x2, ref y2);
                        setLine(ref m_compPlanetMarkers[i], x, y, x2, y2);
                        m_compPlanetMarkers[i].Visibility = Visibility.Visible;
                    }
                }
            }

            setSelectedPlanet(SelectedPlanetNo, SelectedPlanetIsComparison);
            updateInfoPanelIfVisible();
        }
        #endregion



        private void editButton_Click(object sender, EventArgs e)
        {
            ((App)App.Current).tempChartData = ((App)App.Current).currentChartData.Duplicate();
            NavigationService.Navigate(new Uri("/ChartInputDataPage.xaml", UriKind.Relative));
            //test1();
        }


        private void removeAdjustedChartFilename()
        {
            if (IsCompareChart)
            {
                ChartCompareFileName = null;
            }
            else
            {
                ChartFileName = null;
            }
        }

        private void buttonLess_Click(object sender, RoutedEventArgs e)
        {
            ChartData chart = (IsCompareChart ? ComparisonChart : Chart);
            chart.adjustTime(0, 0, -1, 0, 0);
            removeAdjustedChartFilename();
            InfoPanelSelectedIdx = -1;
            aspectListBox.SelectedIndex = -1;
            updateChart();
        }

        private void buttonMore_Click(object sender, RoutedEventArgs e)
        {
            ChartData chart = (IsCompareChart ? ComparisonChart : Chart);
            chart.adjustTime(0, 0, 1, 0, 0);
            removeAdjustedChartFilename();
            InfoPanelSelectedIdx = -1;
            aspectListBox.SelectedIndex = -1;
            updateChart();
        }
        private void buttonLessStep_Click(object sender, RoutedEventArgs e)
        {
            ChartData chart = (IsCompareChart ? ComparisonChart : Chart);
            chart.adjustTime(0, 0, 0, -1, 0);
            removeAdjustedChartFilename();
            InfoPanelSelectedIdx = -1;
            aspectListBox.SelectedIndex = -1;
            updateChart();
        }
        private void buttonMoreStep_Click(object sender, RoutedEventArgs e)
        {
            ChartData chart = (IsCompareChart ? ComparisonChart : Chart);
            chart.adjustTime(0, 0, 0, 1, 0);
            removeAdjustedChartFilename();
            InfoPanelSelectedIdx = -1;
            aspectListBox.SelectedIndex = -1;
            updateChart();
        }
        private void ChartCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateChart();
        }

        private void ChartCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            //updateChart();
        }

        private void ChartCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            updateChart();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SaveDialog.xaml?dir=charts", UriKind.Relative));
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/OpenDialog.xaml?dir=charts&comparison=0", UriKind.Relative));
            AnimateEnabled = false;
            SelectedPlanetNo = -1;
            SelectedPlanetIsComparison = false;
        }

        private void compareButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/OpenDialog.xaml?dir=charts&comparison=1", UriKind.Relative));
            AnimateEnabled = false;
            SelectedPlanetNo = -1;
            SelectedPlanetIsComparison = false;
        }

        private void openVipButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/OpenDialog.xaml?dir=vip&comparison=0", UriKind.Relative));
            AnimateEnabled = false;
            SelectedPlanetNo = -1;
            SelectedPlanetIsComparison = false;
        }

        private void compareVipButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/OpenDialog.xaml?dir=vip&comparison=1", UriKind.Relative));
            AnimateEnabled = false;
            SelectedPlanetNo = -1;
            SelectedPlanetIsComparison = false;
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void transitButton_Click(object sender, EventArgs e)
        {
            if (IsTransitChart)
            {
                IsTransitChart = false;
                IsCompareChart = false;
                AnimateEnabled = false;
                updateChart();
            }
            else
            {
                ((App)App.Current).currentCompareChartData = ChartData.CurrentTransits();
                IsTransitChart = true;
                IsCompareChart = true;
                AnimateEnabled = true;
                updateChart();
            }
        }
        private void animateButton_Click(object sender, EventArgs e)
        {
            AnimateEnabled = !AnimateEnabled;
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml",
                    UriKind.Relative));
        }

        private void setSelectedPlanet(int planetNo, bool comparison)
        {
            SelectedPlanetNo = planetNo;
            SelectedPlanetIsComparison = comparison;
            if (SelectedPlanetIsComparison && !IsCompareChart)
            {
                SelectedPlanetNo = planetNo = -1;
            }
            if (planetNo < 0)
            {
                textPlanetInfo.Text = "Touch planet symbols for information";
                moreInfoButton.Visibility = Visibility.Collapsed;
                PlanetInfoImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                moreInfoButton.Visibility = Visibility.Visible;
                string padding = "      ";
                // TODO // PlanetInfoImage.Source = planetImages[planetNo].Source;
                PlanetInfoImage.Visibility = Visibility.Visible;
                PlanetInfoImage.OpacityMask = planetImages[planetNo].OpacityMask;
                if (!comparison)
                {
                    textPlanetInfo.Text = padding + AstroDefs.PLANET_FULL_NAMES[planetNo] +
                        " in " +
                        AstroDefs.ephem_positionToChar6(AstroDefs.ephem_positionOfPlanet(CurrentPositions, planetNo));
                }
                else
                {
                    string type = (IsTransitChart ? "Transit" : "Synastry");
                    textPlanetInfo.Text = padding + type + " " + AstroDefs.PLANET_FULL_NAMES[planetNo] +
                        " in " +
                        AstroDefs.ephem_positionToChar6(AstroDefs.ephem_positionOfPlanet(CurrentComparisonPositions, planetNo));
                }
            }
        }

        private void Planet_MouseEnter(object sender, MouseEventArgs e)
        {
            bool comparison = false;
            int planetNo = -1;
            for (int i = 0; i < planetImages.Length; i++)
            {
                if (planetImages[i] == sender)
                {
                    comparison = false;
                    planetNo = i;
                    break;
                }
            }
            if (planetNo < 0)
            {
                for (int i = 0; i < compPlanetImages.Length; i++)
                {
                    if (compPlanetImages[i] == sender)
                    {
                        comparison = true;
                        planetNo = i;
                        break;
                    }
                }
            }
            if (planetNo > -1)
            {
                // TODO // PlanetInfoImage.Source = ((Image)sender).Source;
                PlanetInfoImage.Visibility = Visibility.Visible;
                PlanetInfoImage.OpacityMask = ((Rectangle)sender).OpacityMask;

                VibrateController vibrate = VibrateController.Default;
                vibrate.Start(TimeSpan.FromMilliseconds(20));
                setSelectedPlanet(planetNo, comparison);
                InfoPanelSelectedIdx = -1;
            }
            updateChart(); //         fixAspectLines();
        }

        public class InfoPanelElem
        {
            public Brush color
            {
                get
                {
                    if (_aspect < 0)
                    {
                        return Application.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
                    }
                    return new SolidColorBrush(colorForAspect(_aspect));
                }
            }
            public string description
            {
                get;
                set;
            }
            public string query
            {
                get;
                set;
            }
            private float _pos;
            private float _orb;
            public string pos
            {
                get
                {
                    if (_pos < 0) { return ""; }
                    return AstroDefs.ephem_positionToChar6(_pos);
                }
            }
            public Visibility posVisibility
            {
                get
                {
                    if (_pos < 0) { return Visibility.Collapsed; }
                    return Visibility.Visible;
                }
            }
            public string orb
            {
                get {
                    if (_orb < 0) 
                    { 
                        return ""; 
                    }
                    return _orb.ToString("0.00");
                }
            }
            public InfoPanelElem(string s, float pos, float orb, string q)
            {
                description = s;
                query = q;
                _pos = pos;
                _orb = orb;
                internalQuery = null;
            }
            int _aspect = -1;
            public string internalQuery { get; set; }
            public InfoPanelElem withPositions(int planet, int sign, int house, int aspect, int planet2) {
                internalQuery = planet + "_" + sign + "_" + house + "_" + aspect + "_" + planet2;
                _aspect = aspect;
                return this;
            }
        }

        private void populateInfoPanel()
        {
            if (SelectedPlanetIsComparison && !IsCompareChart)
            {
                SelectedPlanetNo = -1;
            }
            int p = SelectedPlanetNo;
            if (p < 0)
            {
                return;
            }
            float pos = AstroDefs.ephem_positionOfPlanet(CurrentPositions, p);
            if (SelectedPlanetIsComparison)
            {
                pos = AstroDefs.ephem_positionOfPlanet(CurrentComparisonPositions, p);
            }
            int house = houseOfPosition(pos);
            int nextHouse = (house == HouseCalc.HOUSE_12 ? HouseCalc.HOUSE_1 : house + 1);
            float nextHousePos = Houses[nextHouse];
            float nextHouseOrb = Math.Abs(AstroDefs.calcDistance(pos, nextHousePos));
            bool showNextHouseToo = false;
            showNextHouseToo = (SelectedPlanetIsComparison && (getSynastryAspect(pos, nextHousePos) == 0)) ||
                (!SelectedPlanetIsComparison && (getNatalAspect(pos, nextHousePos) == 0));
            // TODO // PlanetInfoImage.Source = planetImages[p].Source;
            PlanetInfoImage.Visibility = Visibility.Visible;
            PlanetInfoImage.OpacityMask = planetImages[p].OpacityMask;

            IList<InfoPanelElem> l = new List<InfoPanelElem>();

            int sign = AstroDefs.ephem_signForPosition(pos);
            string planet = AstroDefs.PLANET_FULL_NAMES[p];
            string signName = AstroDefs.SIGN_NAMES[sign];
            string houseName = HouseCalc.HOUSE_NAMES[house];
            string planetPrefix = "";
            if (SelectedPlanetIsComparison)
            {
                planetPrefix = (IsTransitChart ? "Transit " : "Synastry ");
                searchInfo.Visibility = Visibility.Visible;
                webSearchButton.Visibility = Visibility.Visible;
                chartSearchButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                searchInfo.Visibility = Visibility.Visible;
                webSearchButton.Visibility = Visibility.Visible;
                chartSearchButton.Visibility = Visibility.Visible;
            }
            string desc = "";
            string query = "";
            if (!SelectedPlanetIsComparison)
            {
                // sign position of transit/synastry planets is not very interesting...
                desc = query = planetPrefix + planet + " in " + signName;
                l.Add(new InfoPanelElem(desc, -1, -1, query).withPositions(p,sign,-1,-1,-1));
            }

            // Don't show the house cusps house positions....
            if (SelectedPlanetIsComparison || 
                (!(SelectedPlanetNo == AstroDefs.ASCENDANT) && !(SelectedPlanetNo == AstroDefs.MEDIUM_COELI))) {
                desc = query = planetPrefix + planet + " in " + houseName + " house";
                l.Add(new InfoPanelElem(desc, -1, -1, query).withPositions(p,-1,house,-1,-1));

                if (showNextHouseToo) {
                    desc = planetPrefix + planet + " conjunct " + HouseCalc.HOUSE_NAMES[nextHouse] + " house";
                    query = planetPrefix + planet + " in " + HouseCalc.HOUSE_NAMES[nextHouse] + " house";
                    l.Add(new InfoPanelElem(desc, nextHousePos, nextHouseOrb, query).withPositions(p,-1,nextHouse,-1,-1));
                }
            }

            const int numBodies = AstroDefs.NUM_PLANETS + 2;

            //if (!SelectedPlanetIsComparison)
            //{
                for (int p2 = 0; p2 < numBodies; p2++)
                {
                    if (p2 != p || SelectedPlanetIsComparison)
                    {
                        float pos2 = AstroDefs.ephem_positionOfPlanet(CurrentPositions, p2);
                        int asp = -1;
                        if (!SelectedPlanetIsComparison)
                        {
                            asp = getNatalAspect(pos, pos2);
                        }
                        else
                        {
                            if (IsTransitChart)
                            {
                                asp = getTransitAspect(pos, pos2);
                            }
                            else
                            {
                                asp = getSynastryAspect(pos, pos2);
                            }
                        }
                        if (asp >= 0)
                        {
                            float orb = AstroDefs.calcDistance(pos, pos2) - (float)asp;
                            desc = AstroDefs.ephem_nameOfAspect(asp) + 
                                (SelectedPlanetIsComparison ? " natal " : " ") + 
                                AstroDefs.PLANET_FULL_NAMES[p2];
                            query = planetPrefix + planet + " " + desc;
                            l.Add(new InfoPanelElem(
                                desc,
                                pos2,
                                Math.Abs(orb),
                                query).withPositions(p,-1,-1,asp,p2));
                        }
                    }
                }
            /*}
            else
            {
                for (int pn = 0; pn < numBodies; pn++)
                {
                    // get Natal position    
                    float pnPos = AstroDefs.ephem_positionOfPlanet(CurrentComparisonPositions, pn);
                    int asp = (IsTransitChart ? getTransitAspect(pos, pnPos) : getSynastryAspect(pos, pnPos));;
                    if (asp >= 0)
                    {
                        float orb = AstroDefs.calcDistance(pos, pnPos) - (float)asp;
                        desc = AstroDefs.ephem_nameOfAspect(asp) + " natal " + AstroDefs.PLANET_FULL_NAMES[pn];
                        query = planetPrefix + planet + " " + desc;
                        l.Add(new InfoPanelElem(
                            desc,
                            pnPos,
                            Math.Abs(orb),
                            query).withPositions(p, -1, -1, asp, pn));
                    }
                }
            }*/
            aspectListBox.ItemTemplate = aspectListTemplate;
            aspectListBox.ItemsSource = l;
            if (InfoPanelSelectedIdx >= 0)
            {
                aspectListBox.SelectedIndex = InfoPanelSelectedIdx;
            }
        }

        private void updateInfoPanelIfVisible()
        {
            bool shouldClose = (SelectedPlanetIsComparison && !IsCompareChart);
            if (InfoPanelVisible && !shouldClose)
            {
                moreInfoButton.Content = "Less info...";
                infoPanel.Visibility = Visibility.Visible;
                populateInfoPanel();
                aspectListBox.SelectedIndex = InfoPanelSelectedIdx;
            } else {
                moreInfoButton.Content = "More info...";
                infoPanel.Visibility = Visibility.Collapsed;
                if (shouldClose)
                {
                    InfoPanelVisible = false;
                }
            }
        }

        private void moreInfoButton_Click(object sender, RoutedEventArgs e)
        {
            InfoPanelVisible = !InfoPanelVisible;
            searchInfo.Text = TEXT_TAP_ONE_LINE;
            updateInfoPanelIfVisible();
            webSearchButton.IsEnabled = false;
            chartSearchButton.IsEnabled = false;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (infoPanel.Visibility == Visibility.Visible)
            {
                InfoPanelVisible = false;
                updateInfoPanelIfVisible();
                moreInfoButton.Content = "More info...";
                e.Cancel = true;
            }
        }

        private void webSearchButton_Click(object sender, RoutedEventArgs e)
        {
            InfoPanelSelectedIdx = aspectListBox.SelectedIndex;
            InfoPanelElem elem = (InfoPanelElem)aspectListBox.SelectedItem;
            if (elem != null)
            {
                SearchTask searchTask = new SearchTask();
                searchTask.SearchQuery = elem.query;
                searchTask.Show();
            }
        }

        private void chartSearchButton_Click(object sender, RoutedEventArgs e)
        {
            InfoPanelSelectedIdx = aspectListBox.SelectedIndex;
            InfoPanelElem elem = (InfoPanelElem)aspectListBox.SelectedItem;
            if (elem != null)
            {
                NavigationService.Navigate(new Uri("/OpenDialog.xaml?search=charts_vip"+
                    "&desc=" + Uri.EscapeDataString(elem.query) +
                    "&query=" + elem.internalQuery, 
                    UriKind.Relative));
            }
        }

        private void aspectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InfoPanelElem elem = (InfoPanelElem)aspectListBox.SelectedItem;
            if (elem != null) {
                searchInfo.Text = "Search for " + elem.query + "...";
                webSearchButton.IsEnabled = true;
                chartSearchButton.IsEnabled = true;
            }
            else
            {
                searchInfo.Text = TEXT_TAP_ONE_LINE;
                webSearchButton.IsEnabled = false;
                chartSearchButton.IsEnabled = false;
            }
        }

        private bool AnimateEnabled
        {
            set
            {
                Visibility v = (value ? Visibility.Visible : Visibility.Collapsed);
                buttonLess.Visibility = v;
                buttonLessStep.Visibility = v;
                buttonMore.Visibility = v;
                buttonMoreStep.Visibility = v;
            }
            get
            {
                return Visibility.Visible == buttonLess.Visibility;
            }
        }

    }
}