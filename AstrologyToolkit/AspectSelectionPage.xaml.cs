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

namespace AstrologyToolkit
{
    public partial class AspectSelectionPage : PhoneApplicationPage
    {
        public class AspectInfo
        {
            public string AspectName
            {
                set;
                get;
            }
            public int AspectDeg
            {
                set;
                get;
            }
            public bool Enabled
            {
                get;
                set;
            }
        }

        public AspectSelectionPage()
        {
            InitializeComponent();

            aspectListBox.ItemTemplate = aspectSelectTemplate;

            int[] aspsEnabled = ((App)App.Current).AspectsEnabled;
            int[] asps = AstroDefs.ASPECTS;
            AspectInfo[] a = new AspectInfo[asps.Length];

            for (int i = 0; i < asps.Length; i++)
            {
                a[i] = new AspectInfo();
                a[i].AspectDeg = AstroDefs.ASPECTS[i];
                a[i].AspectName = AstroDefs.ASPECT_NAMES[i];
                a[i].Enabled = aspsEnabled.Contains(AstroDefs.ASPECTS[i]);
            }
            aspectListBox.ItemsSource = a;
        }

        private void aspectCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ListBoxItem checkedItem = this.aspectListBox.ItemContainerGenerator.ContainerFromItem((sender as CheckBox).DataContext) as ListBoxItem;
            if (checkedItem != null)
            {
                AspectInfo a = (AspectInfo)checkedItem.Content;
                ((App)App.Current).setAspect(a.AspectDeg, true);
                checkedItem.IsSelected = true;
            }
        }

        private void aspectCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            ListBoxItem uncheckedItem = this.aspectListBox.ItemContainerGenerator.ContainerFromItem((sender as CheckBox).DataContext) as ListBoxItem;
            if (uncheckedItem != null)
            {
                AspectInfo a = (AspectInfo)uncheckedItem.Content;
                ((App)App.Current).setAspect(a.AspectDeg, false);
                uncheckedItem.IsSelected = false;
            }

        }
    }
}