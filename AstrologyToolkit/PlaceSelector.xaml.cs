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
    public partial class PlaceSelector : PhoneApplicationPage
    {
        public PlaceSelector()
        {
            InitializeComponent();
            listBox1.ItemTemplate = placeTemplate;
        }
        String CountryCode;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("CountryCode"))
            {
                CountryCode = this.NavigationContext.QueryString["CountryCode"];
                IList<GeoData.GeoPlaceInfo> places = GeoData.loadPlaceInfoForCountryCode(CountryCode);
                listBox1.ItemsSource = places;

                if (NavigationContext.QueryString.Keys.Contains("selected")) {
                    string sel = NavigationContext.QueryString["selected"];
                    int i = 0;
                    foreach (object o in listBox1.ItemsSource) {
                        if (((GeoData.GeoPlaceInfo)o).placeName.Equals(sel, StringComparison.InvariantCultureIgnoreCase))
                        {
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
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GeoData.GeoPlaceInfo selectedPlace = (GeoData.GeoPlaceInfo)listBox1.SelectedItem;
            ((App)App.Current).tempChartData.Latitude = ((float)selectedPlace.latitudeCents) / 100.0f;
            ((App)App.Current).tempChartData.Longitude = ((float)selectedPlace.longitudeCents) / 100.0f;
            ((App)App.Current).tempChartData.PlaceName = selectedPlace.placeName;
            string curCountryName = ((App)App.Current).tempChartData.CountryName;
            //((App)App.Current).tempChartData.PlaceDescription = curCountryName + "/" + selectedPlace.placeName;
            NavigationService.GoBack();
        }
    }
}