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
using Microsoft.Phone.Tasks;

namespace AstrologyToolkit
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
            if (App.IsTrial)
            {
                reviewButton.Content = "Upgrade from TRIAL version";
            }
        }

        private void reviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsTrial)
            {
                new MarketplaceDetailTask().Show();
            }
            else
            {
                new MarketplaceReviewTask().Show();
            }
        }

        private void moreButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();
            marketplaceSearchTask.SearchTerms = "leaping light";
            marketplaceSearchTask.Show();
        }
    }
}