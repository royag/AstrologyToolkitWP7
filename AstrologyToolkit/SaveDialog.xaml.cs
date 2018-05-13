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
using System.IO.IsolatedStorage;
using System.IO;
using AstrologyToolkit.LeapingLightAstrology;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace AstrologyToolkit
{
    public partial class SaveDialog : PhoneApplicationPage
    {
        public SaveDialog()
        {
            InitializeComponent();
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBarIconButton button1 = new ApplicationBarIconButton(new Uri("/Toolkit.Content/appbar.disk.png", UriKind.Relative));
            button1.Text = "Done";
            button1.Click += saveButton_Click;
            ApplicationBar.Buttons.Add(button1);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Cancel.png", UriKind.Relative));
            button2.Text = "Cancel";
            button2.Click += cancelButton_Click;
            ApplicationBar.Buttons.Add(button2);

        }

        string DirectoryName;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DirectoryName = this.NavigationContext.QueryString["dir"];
            textBox1.Text = defaultName();
        }

        private string[] AvailableFileNames()
        {
            string[] filesInSubDirs = new string[0];
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    store.CreateDirectory(DirectoryName);
                    string searchpath = System.IO.Path.Combine(DirectoryName, "*.chr");
                    filesInSubDirs = store.GetFileNames(searchpath);
                }
            }
            catch (IsolatedStorageException)
            {
                MessageBox.Show("Error", "Unable to access isloated storage", MessageBoxButton.OK);
            }
            return filesInSubDirs;
        }


        private string fixName(string name)
        {
            return name;
        }

        private string chartData() {
            App app = (App)App.Current;
            return app.currentChartData.serialize();
            /*string chartData =
                currentChartData.CountryCode + "\n" +
                currentChartData.CountryName + "\n" +
                currentChartData.PlaceName + "\n" +
                (int)(currentChartData.Latitude * 100.0) + "\n" +
                (int)(currentChartData.Longitude * 100.0) + "\n" +
                currentChartData.TimeZone + "\n" +
                currentChartData.Year + "\n" +
                currentChartData.Month + "\n" +
                currentChartData.Day + "\n" +
                currentChartData.Hour + "\n" +
                currentChartData.Minute + "\n";
            return chartData;*/
        }

        private string defaultName() {
            App app = (App)App.Current;
            ChartData currentChartData = app.currentChartData;
            return currentChartData.Year +
                currentChartData.Month.ToString().PadLeft(2,'0') +
                currentChartData.Day.ToString().PadLeft(2, '0') +
                "_" + currentChartData.Hour.ToString().PadLeft(2, '0') +
                currentChartData.Minute.ToString().PadLeft(2, '0') +
                '_' + currentChartData.CountryCode;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (App.IsTrial)
            {
                string[] storedFiles = AvailableFileNames();
                if (storedFiles.Length >= 3)
                {
                    MessageBoxResult res = MessageBox.Show("Trial version cannot store more than 3 charts at a time.\n" +
                        "You can use the trash can symbol in the 'Open Chart' dialog to delete existing charts.\n" +
                        "Press OK to go buy the full version, or press Cancel to keep to 3",
                        "Trial Version Limit", MessageBoxButton.OKCancel);
                    if (res == MessageBoxResult.OK)
                    {
                        new MarketplaceDetailTask().Show();
                    }
                    return;
                }
            }

            try
            {
                string name = "";
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    store.CreateDirectory(DirectoryName);
                    name = fixName(textBox1.Text) + ".chr";
                    string fullname = System.IO.Path.Combine(DirectoryName, name);
                    if (store.FileExists(fullname))
                    {
                        MessageBoxResult m = MessageBox.Show("Save File", "File name already exists. Overwrite?", MessageBoxButton.OKCancel);
                        if (m != MessageBoxResult.OK)
                        {
                            return;
                        }
                        store.DeleteFile(fullname);
                    }
                    using (StreamWriter sw = new StreamWriter(store.CreateFile(fullname)))
                    {
                        sw.Write(chartData());
                    }
                }
                MessageBox.Show("File saved", "File was successfully saved!", MessageBoxButton.OK);
                ((App)App.Current).chartFileName = name;
                NavigationService.GoBack();
            }
            catch (IsolatedStorageException)
            {
                MessageBox.Show("Error", "Unable to access isloated storage", MessageBoxButton.OK);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

    }
}