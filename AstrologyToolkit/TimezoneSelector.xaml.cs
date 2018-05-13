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
    public partial class TimezoneSelector : PhoneApplicationPage
    {
        public TimezoneSelector()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            listBox1.ItemTemplate = timezoneTemplate;
            IList<String> zones = TZInfo.readZones();
            string suggest = ((App)App.Current).tempSuggestedTimezones;
            if (suggest != null)
            {
                string[] szones = suggest.Split(new char[] { ';' });
                for (int i = 0; i < szones.Length; i++) {
                    zones.Insert(i, szones[i]);
                }
            }
            listBox1.ItemsSource = zones;

            if (NavigationContext.QueryString.Keys.Contains("selected"))
            {
                string sel = NavigationContext.QueryString["selected"];
                int i = 0;
                foreach (object o in listBox1.ItemsSource)
                {
                    if (((String)o).Equals(sel, StringComparison.InvariantCultureIgnoreCase))
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

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedZone = (string)listBox1.SelectedItem;
            ((App)App.Current).tempChartData.TimeZone = selectedZone;
            NavigationService.GoBack();
        }
    }
}