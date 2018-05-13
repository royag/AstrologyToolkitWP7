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
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace AstrologyToolkit
{
    public partial class ChartInputDataPage : PhoneApplicationPage
    {

        //public static GeoData.GeoCountryInfo SelectedCountry;

        string lastCountryCode;

        public ChartInputDataPage()
        {
            InitializeComponent();
            if (Chart == null)
            {
                loadCurrentChart();
            }
            lastCountryCode = Chart.CountryCode;
            //hidePlacesIfNoCountry();
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBarIconButton button1 = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Check.png", UriKind.Relative));
            button1.Text = "Done";
            button1.Click += doneButton_Click;
            ApplicationBar.Buttons.Add(button1);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Cancel.png", UriKind.Relative));
            button2.Text = "Cancel";
            button2.Click += cancelButton_Click;
            ApplicationBar.Buttons.Add(button2);
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            if (Chart.TimeZone == null || Chart.TimeZone == "")
            {
                MessageBox.Show("You must select a Time Zone");
                return;
            }
            if (Chart.PlaceName == null || Chart.PlaceName == "")
            {
                MessageBox.Show("You must select a Place");
                return;
            }
            applyChart();
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        /*private void hidePlacesIfNoCountry()
        {
            //placeSelect.Visibility = (Chart.CountryCode == null || Chart.CountryCode.Length == 0 ? Visibility.Collapsed : Visibility.Visible);
        }*/

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Chart == null)
            {
                loadCurrentChart();
            }
            if (lastCountryCode != null)
            {
                if (!lastCountryCode.Equals(Chart.CountryCode))
                {
                    countryChanged();
                }
            }
            updateSelectedTexts();
        }

        private void updateSelectedTexts()
        {
            countrySelect.Content = (Chart.CountryCode.Length > 0 ? Chart.CountryName : "");
            placeSelect.Content = (Chart.PlaceName.Length > 0 ? 
                Chart.PlaceName + 
                " (" + GeoData.coordinatesToString(Chart.Latitude, Chart.Longitude) + ")"
                : "Select Place");
            timeZoneSelect.Content = (Chart.TimeZone.Length > 0 ? Chart.TimeZone : "Select Time Zone");
            //hidePlacesIfNoCountry();
            MyDatePicker.Value = new DateTime(Chart.Year, Chart.Month, Chart.Day);
            MyTimePicker.Value = new DateTime(Chart.Year, Chart.Month, Chart.Day, Chart.Hour, Chart.Minute, 0);
        }

        private void countryChanged()
        {
            setSuggestedZones(null);
            lastCountryCode = Chart.CountryCode;
            IList<string> defaultZones = new List<string>();
            int numSuggestions = TimeZoneDefaults.addDefaultZonesForCountry(Chart.CountryCode, ref defaultZones);
            if (1 == numSuggestions)
            {
                timeZoneSelect.Content = Chart.TimeZone = defaultZones[0];
            }
            else
            {
                Chart.TimeZone = "";
                if (numSuggestions > 1)
                {
                    setSuggestedZones(defaultZones.ToArray());
                }
            }
            Chart.PlaceName = "";
            updateSelectedTexts();
        }

        private void setSuggestedZones(string[] defaultZones)
        {
            string s = "";
            if (defaultZones == null)
            {
                s = null;
            }
            else
            {
                bool first = true;
                foreach (string zone in defaultZones)
                {
                    if (!first)
                    {
                        s += ";";
                    }
                    s += zone;
                    first = false;
                }
            }
            ((App)App.Current).tempSuggestedTimezones = s;
        }

        private ChartData Chart
        {
            get
            {
                return ((App)App.Current).tempChartData;
            }
        }
        private void loadCurrentChart()
        {
            ((App)App.Current).tempChartData = ((App)App.Current).currentChartData.Duplicate();
        }
        private void applyChart() {
            ((App)App.Current).currentChartData = ((App)App.Current).tempChartData.Duplicate();
            ((App)App.Current).chartFileName = null;
            NavigationService.GoBack();
        }

        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            bool ok = true;
            if (e.NewDateTime.Value.Year < 1892)
            {
                ok = false;
                MyDatePicker.Value = new DateTime(1892, 1, 1);
                MessageBox.Show("Date too early");
            }
            else if (e.NewDateTime.Value.Year > 2099)
            {
                ok = false;
                MyDatePicker.Value = new DateTime(2099, 12, 31);
                MessageBox.Show("Date too late");
            }
            if (App.IsTrial)
            {
                if (e.NewDateTime.Value.Year < 1995)
                {
                    ok = false;
                    MyDatePicker.Value = new DateTime(1995, 1, 1);
                    MessageBoxResult res = MessageBox.Show("Trial version does not support dates earlier than 1995.\n"+
                        "Press OK to go buy the full version, or press Cancel to reset to 1995", 
                        "Trial Version Limit", MessageBoxButton.OKCancel);
                    if (res == MessageBoxResult.OK)
                    {
                        new MarketplaceDetailTask().Show();
                    }
                }
                else if (e.NewDateTime.Value.Year > 2019)
                {
                    ok = false;
                    MyDatePicker.Value = new DateTime(2019, 12, 31);
                    MessageBoxResult res = MessageBox.Show("Trial version does not support dates later than 2019.\n" +
                        "Press OK to go buy the full version, or press Cancel to reset to 2019",
                        "Trial Version Limit", MessageBoxButton.OKCancel);
                    if (res == MessageBoxResult.OK)
                    {
                        new MarketplaceDetailTask().Show();
                    }
                }
            }
            if (ok)
            {
                Chart.Year = e.NewDateTime.Value.Year;
                Chart.Month = e.NewDateTime.Value.Month;
                Chart.Day = e.NewDateTime.Value.Day;
            }
        }

        private void MyTimePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            Chart.Hour = e.NewDateTime.Value.Hour;
            Chart.Minute = e.NewDateTime.Value.Minute;
        }

        private void countrySelect_Click(object sender, RoutedEventArgs e)
        {
            string sel = "";
            if (Chart.CountryName != null && Chart.CountryName != "")
            {
                sel = "?selected=" + Uri.EscapeDataString(Chart.CountryName);
            }
            NavigationService.Navigate(new Uri("/CountrySelector.xaml" + sel, UriKind.Relative));
        }

        private void placeSelect_Click(object sender, RoutedEventArgs e)
        {
            if (Chart.CountryCode == "CUSTOM" || Chart.CountryCode == "")
            {
                ((App)App.Current).TempLatitude = (int)(Chart.Latitude * 100);
                ((App)App.Current).TempLongitude = (int)(Chart.Longitude * 100);
                NavigationService.Navigate(new Uri("/GeoPositionSelectorPage.xaml", UriKind.Relative));
                return;
            }

            if (Chart.CountryCode == null || Chart.CountryCode.Length < 1)
            {
                MessageBox.Show("You must select a country before you can select a place");
            }
            else
            {
                string sel = "";
                if (Chart.PlaceName != null && Chart.PlaceName != "")
                {
                    sel = "&selected=" + Uri.EscapeDataString(Chart.PlaceName);
                }
                NavigationService.Navigate(new Uri("/PlaceSelector.xaml?CountryCode=" + Chart.CountryCode + sel, UriKind.Relative));
            }
        }

        private void timeZoneSelect_Click(object sender, RoutedEventArgs e)
        {
            string sel = "";
            if (Chart.TimeZone != null && Chart.TimeZone != "")
            {
                sel = "?selected=" + Uri.EscapeDataString(Chart.TimeZone);
            }
            NavigationService.Navigate(new Uri("/TimezoneSelector.xaml" + sel, UriKind.Relative));
        }
    }
}