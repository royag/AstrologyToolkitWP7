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
using System.Windows.Navigation;
using LeapingLight.Astrology;

namespace AstrologyToolkit
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public class HouseItem
        {
            public int HouseSystemNo { get; set; }
            public string HouseSystemName { get; set; }
        }
        protected HouseItem[] createHouseItems()
        {
            HouseItem[] houses = new HouseItem[HouseCalc.HOUSE_SYSTEM_NAMES.Length-1];
            for (int i = 1; i < HouseCalc.HOUSE_SYSTEM_NAMES.Length; i++ )
            {
                houses[i-1] = new HouseItem();
                houses[i-1].HouseSystemNo = i;
                houses[i-1].HouseSystemName = HouseCalc.HOUSE_SYSTEM_NAMES[i];
            }
            return houses;
        }

        public SettingsPage()
        {
            InitializeComponent();
            
            //housePicker.ItemTemplate = null;

            int houseSystem = ((App)App.Current).PreferredHouseSystem;
            housePicker.ItemsSource = createHouseItems();
            HouseItem selectedHouse = null;
            foreach (HouseItem item in housePicker.ItemsSource)
            {
                if (item.HouseSystemNo == houseSystem)
                {
                    selectedHouse = item;
                    break;
                }
            }
            if (selectedHouse != null)
            {
                housePicker.SelectedItem = selectedHouse;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int geoSettings = ((App)App.Current).getGeoSettings();
            if (geoSettings == 1)
            {
                geoLocationCheckbox.IsChecked = true;
            }
            else
            {
                geoLocationCheckbox.IsChecked = false;
            }
            //int houseSystem = ((App)App.Current).PreferredHouseSystem;
            int[] aspsEnabled = ((App)App.Current).AspectsEnabled;
            int[] aspsTotal = AstroDefs.ASPECTS;
            string aspText = "Showing " + aspsEnabled.Length + " out of " + aspsTotal.Length + " aspects";
            aspectButton.Content = aspText;
        }

        private void geoLocationCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).setGeoSettings(1);
        }

        private void geoLocationCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).setGeoSettings(0);
        }

        private void housePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int houseSystem = ((HouseItem)housePicker.SelectedItem).HouseSystemNo;
                if (houseSystem >= 0)
                {
                    ((App)App.Current).savePreferredHouseSystem(houseSystem);
                }                
            }
            catch (NullReferenceException)
            {
                // silently ignore... might be we just deleted the file
            }
        }

        private void aspectButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AspectSelectionPage.xaml", UriKind.Relative));
        }

    }
}