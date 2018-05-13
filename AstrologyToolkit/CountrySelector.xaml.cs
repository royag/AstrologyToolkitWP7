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
using System.Windows.Navigation;

namespace AstrologyToolkit
{
    public partial class CountrySelector : PhoneApplicationPage
    {
        public CountrySelector()
        {
            InitializeComponent();
            IList<GeoData.GeoCountryInfo> ctrs = GeoData.loadCountryInfo();

            GeoData.GeoCountryInfo custom = new GeoData.GeoCountryInfo();
            custom.countryName = "[CUSTOM LOCATION]";
            custom.countryCode = "CUSTOM";

            ctrs.Insert(0, custom);

            listBox1.ItemTemplate = countryTemplate;
            listBox1.ItemsSource = ctrs;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.Keys.Contains("selected")) {
                string sel = NavigationContext.QueryString["selected"];
                int i = 0;
                foreach (object o in listBox1.ItemsSource) {
                    if (((GeoData.GeoCountryInfo)o).countryName.Equals(sel, StringComparison.InvariantCultureIgnoreCase)) {
                        //listBox1.ScrollIntoView(o);
                        
                        Dispatcher.BeginInvoke(() =>
                        {
                            System.Threading.Thread.Sleep(200);
                            listBox1.ScrollIntoView(listBox1.Items[i]);
                        });
                        break;
                    }
                    i++;
                }
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GeoData.GeoCountryInfo selectedCountry = (GeoData.GeoCountryInfo)listBox1.SelectedItem;
            ((App)App.Current).tempChartData.CountryCode = selectedCountry.countryCode;
            ((App)App.Current).tempChartData.CountryName = selectedCountry.countryName;
            NavigationService.GoBack();
        }
    }
}