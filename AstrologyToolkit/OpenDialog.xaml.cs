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
//using Phone.Controls;
using LeapingLight.Astrology;
using System.ComponentModel;
using Microsoft.Phone.Tasks;

namespace AstrologyToolkit
{
    public partial class OpenDialog : PhoneApplicationPage
    {
        public OpenDialog()
        {
            InitializeComponent();

            listBox1.ItemTemplate = filenameTemplate;
            listBox1.ItemsSource = new string[] { };
        }

        string DirectoryName;
        string searchDesc = "";
        string[] SearchDirs;
        bool IsComparison;
        bool IsSearch = false;
        int[] searchQuery;
        public const string VipDir = "vip";
        private bool canDownload()
        {
            return VipDir.Equals(DirectoryName);
        }

        protected bool matchesQuery(ChartData cd)
        {
            int planet = searchQuery[0];
            int sign = searchQuery[1];
            int house = searchQuery[2];
            int aspect = searchQuery[3];
            int planet2 = searchQuery[4];
            AstroDefs.planet_positions pp = cd.CalculatePositions();
            bool ok = true;
            if (ok && sign > -1)
            {
                if (sign != AstroDefs.ephem_signForPosition(AstroDefs.ephem_positionOfPlanet(pp, planet)))
                {
                    ok = false;
                }
            }
            if (ok && house > -1)
            {
                float[] houses = cd.CalculateHouses(pp);
                if (house != HouseCalc.houses_houseOfPosition(AstroDefs.ephem_positionOfPlanet(pp, planet), houses))
                {
                    ok = false;
                }
            }
            if (ok && aspect > -1 && planet2 > -1)
            {
                if (aspect != AstroDefs.getAspect(
                    AstroDefs.ephem_positionOfPlanet(pp, planet), 
                    AstroDefs.ephem_positionOfPlanet(pp, planet2), AstroDefs.DEFAULT_ORBS))
                    ok = false;
            }
            return ok;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.Keys.Contains("search"))
            {
                IsSearch = true;
                SearchDirs = NavigationContext.QueryString["search"].Split(new char[] {'_'});
                searchDesc = NavigationContext.QueryString["desc"];
                string[]sq = NavigationContext.QueryString["query"].Split(new char[] { '_' });
                searchQuery = new int[sq.Length];
                for (int i = 0; i < sq.Length; i++)
                {
                    int.TryParse(sq[i], out searchQuery[i]);
                }
                DirectoryName = "";
                IsComparison = false;
            }
            else
            {
                IsSearch = false;
                DirectoryName = this.NavigationContext.QueryString["dir"];
                string comp = this.NavigationContext.QueryString["comparison"];
                IsComparison = (comp == "1");
            }

            if (IsSearch)
            {
                PageTitle.Text = searchDesc;
            }
            else if (IsComparison)
            {
                if (canDownload()) {
                    PageTitle.Text = "compare to vip";
                } else {
                    PageTitle.Text = "compare to";
                }
            }
            else if (canDownload())
            {
                PageTitle.Text = "vip charts";
            }
            else
            {
                PageTitle.Text = "open chart";
            }

            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBarIconButton button1 = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Cancel.png", UriKind.Relative));
            button1.Text = "Cancel";
            button1.Click += cancelButton_Click;
            ApplicationBar.Buttons.Add(button1);

            if (canDownload())
            {
                ApplicationBarIconButton button2 = new ApplicationBarIconButton(new Uri("/Toolkit.Content/appbar.arrow.down.png", UriKind.Relative));
                button2.Text = "Download";
                button2.Click += downloadButton_Click;
                ApplicationBar.Buttons.Add(button2);
            }

            BackgroundWorker worker = new BackgroundWorker(); 
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = false;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            progressBar.Visibility = Visibility.Visible;
            worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender,  
                                       RunWorkerCompletedEventArgs e) 
        {
          progressBar.Visibility = Visibility.Collapsed;
          if (e.Error != null) 
          { 
            // Happens on the UI thread so its ok 
            MessageBox.Show("Error occurred..."); 
          } 
        } 

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int numFiles = updateList();
            if (numFiles == 0)
            {
                if (IsSearch)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Didn't find any charts with " + searchDesc, "No matches", MessageBoxButton.OK);
                        if (NavigationService.CanGoBack)
                        {
                            NavigationService.GoBack();
                        }

                    });
                }
                else if (canDownload())
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("You have no VIP charts stored. Press the download button at the bottom of the screen to get some.", "No Files", MessageBoxButton.OK);
                    });
                }
                else
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Here you can open your saved charts, but you have not saved any.", "No Files", MessageBoxButton.OK);
                        if (NavigationService.CanGoBack)
                        {
                            NavigationService.GoBack();
                        }
                    });
                }
            }
        } 

        private void cancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
        private const string DL_CHART_URL = "http://www.leapinglight.com/astro/vip/";
        private const string DL_CHART_INDEX_URL = DL_CHART_URL + "index.txt";

        //private ProgressIndicator progress;
        //ProgressBar p;

        private string[] existingFiles;
        private IList<string> filesToDownload;
        private int currentlyDownloading;

        private void downloadButton_Click(object sender, EventArgs e)
        {
            existingFiles = AvailableFileNames();
            filesToDownload = new List<String>();
            currentlyDownloading = 0;
            /*
            if (progress == null) {
                progress = new ProgressIndicator();
                progress.ProgressType = ProgressTypes.DeterminateMiddle;
                progress.ShowLabel = true;
            }
            progress.Text = "Downloading index...";
            progress.Show();*/
            //progress.ProgressBar.Value = 0;
            progressBar.Visibility = Visibility.Visible;
            progressBar.Value = 0;
            HttpWebRequest req = HttpWebRequest.CreateHttp(DL_CHART_INDEX_URL);
            req.BeginGetResponse(new AsyncCallback(ReadIndexRequestCallback), req);
        }


        private void ReadIndexRequestCallback(IAsyncResult callbackResult)
        {   
            Dispatcher.BeginInvoke(() => { progressBar.Value = 1; });
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse myResponse;
            try
            {
                myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);
            }
            catch (WebException)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    progressBar.Visibility = Visibility.Collapsed;
                    MessageBox.Show("There was en error trying to connect to the internet. Maybe you are not connected?", "Web Error", MessageBoxButton.OK);
                });
                return;
            }
            string filelist = "";

            using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
            {
                filelist = httpwebStreamReader.ReadToEnd();
            }

            Dispatcher.BeginInvoke(() => { progressBar.Value = 2; });

            string[] files = filelist.Split(new char[] { '\n' });
            int existingNum = existingFiles.Length;
            bool trialLimitReached = false;
            for (int i = 0; i < files.Length; i++)
            {
                if (!existingFiles.Contains(files[i]) && files[i] != null && files[i].Length > 0)
                {
                    if (App.IsTrial && existingNum + filesToDownload.Count >= 10)
                    {
                        trialLimitReached = true;
                    }
                    else
                    {
                        filesToDownload.Add(files[i]);
                    }
                }
            }
            if (filesToDownload.Count <= 0)
            {
                if (trialLimitReached)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        //progressBar.Visibility = Visibility.Collapsed;
                        MessageBoxResult res = MessageBox.Show("Trial version cannot download more than 10 V.I.P. charts.\n" +
                            "Press OK to go buy the full version, or press Cancel to keep to 10",
                            "Trial Version Limit", MessageBoxButton.OKCancel);
                        if (res == MessageBoxResult.OK)
                        {
                            new MarketplaceDetailTask().Show();
                        }
                    });
                }
                else
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        progressBar.Visibility = Visibility.Collapsed;
                        MessageBox.Show("There were no new charts to download", "Nothing to do", MessageBoxButton.OK);
                    });
                }
            }
            else
            {
                if (trialLimitReached)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        //progressBar.Visibility = Visibility.Collapsed;
                        MessageBoxResult res = MessageBox.Show("Trial version cannot download more than 10 V.I.P. charts.\n" +
                            "Press OK to go buy the full version, or press Cancel to keep to 10",
                            "Trial Version Limit", MessageBoxButton.OKCancel);
                        if (res == MessageBoxResult.OK)
                        {
                            new MarketplaceDetailTask().Show();
                        }
                    });
                }
                currentlyDownloading = 0;
                HttpWebRequest req = HttpWebRequest.CreateHttp(DL_CHART_URL + filesToDownload[currentlyDownloading]);
                //progress.Text = filesToDownload[currentlyDownloading] + " (" + (currentlyDownloading + 1) + "/" + filesToDownload.Count + ")";
                req.BeginGetResponse(new AsyncCallback(ReadChartRequestCallback), req);
            }
        }
    

        private void incCurrentlyDownloading() {
            currentlyDownloading += 1;
            int pc = currentlyDownloading * 100 / filesToDownload.Count;
            Dispatcher.BeginInvoke(() =>
            {
                progressBar.Value = pc;
            });
        }

        private void ReadChartRequestCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);
            string fname = filesToDownload[currentlyDownloading];
            string data = "";
            using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
            {
                data = httpwebStreamReader.ReadToEnd();
                //TextBlockResults.Text = results; //-- on another thread!
                //Dispatcher.BeginInvoke(() => txtResult.Text = results);
            }
            saveFile(fname, data);

            incCurrentlyDownloading();
            if (filesToDownload.Count <= currentlyDownloading)
            {
                //progress.Hide();
                updateList();
                Dispatcher.BeginInvoke(() =>
                {
                    progressBar.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Downloaded " + filesToDownload.Count + " new charts", "Download complete", MessageBoxButton.OK);
                });
            }
            else
            {
                HttpWebRequest req = HttpWebRequest.CreateHttp(DL_CHART_URL + filesToDownload[currentlyDownloading]);
                //progress.Text = filesToDownload[currentlyDownloading] + " (" + (currentlyDownloading + 1) + "/" + filesToDownload.Count + ")";
                req.BeginGetResponse(new AsyncCallback(ReadChartRequestCallback), req);
            }
        }

        private void saveFile(string name, string data) {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    store.CreateDirectory(DirectoryName);
                    string fullname = System.IO.Path.Combine(DirectoryName, name);
                    if (store.FileExists(fullname))
                    {
                        store.DeleteFile(fullname);
                    }
                    using (StreamWriter sw = new StreamWriter(store.CreateFile(fullname)))
                    {
                        sw.Write(data);
                    }
                }
            }
            catch (IsolatedStorageException)
            {
            }
        }


        private ChartData[] FindMatchingFiles()
        {
            IList<ChartData> matches = new List<ChartData>();
            try
            {
                Dispatcher.BeginInvoke(() => {
                    progressBar.Value = 0;
                });
                IList<string> filesToSearch = new List<string>();
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                        foreach (string dirname in SearchDirs)
                        {
                            try
                            {

                                string searchpath = System.IO.Path.Combine(dirname, "*.chr");
                                string[] files = store.GetFileNames(searchpath);
                                if (files != null && files.Length > 0)
                                {
                                    foreach (string fn in files)
                                    {
                                        filesToSearch.Add(System.IO.Path.Combine(dirname, fn));
                                    }
                                }
                            }
                            catch (DirectoryNotFoundException)
                            {
                            }
                    }
                    int i = 0;
                    int max = filesToSearch.Count;
                    foreach (string path in filesToSearch)
                    {
                        using (StreamReader r = new StreamReader(store.OpenFile(path, System.IO.FileMode.Open)))
                        {
                            string stringdata = r.ReadToEnd();
                            ChartData chart = new ChartData(stringdata);
                            if (matchesQuery(chart))
                            {
                                chart.FileName = path;
                                matches.Add(chart);
                            }
                        }
                        i++;
                        Dispatcher.BeginInvoke(() =>
                        {
                            progressBar.Value = i * 100 / max;
                        });

                    }
                }
            }
            catch (IsolatedStorageException)
            {
                MessageBox.Show("Error", "Unable to access isloated storage", MessageBoxButton.OK);
            }

            //progress.Hide();

            return matches.ToArray();
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

        private ChartData[] AvailableFiles()
        {
            if (IsSearch)
            {
                return FindMatchingFiles();
            }
            ChartData[] filesInSubDirs = null;
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    store.CreateDirectory(DirectoryName);
                    string searchpath = System.IO.Path.Combine(DirectoryName, "*.chr");
                    string[] fileNames = store.GetFileNames(searchpath);
                    ChartData[] data = new ChartData[fileNames.Length];
                    for (int i = 0; i <fileNames.Length; i++)
                    {
                        string fn = System.IO.Path.Combine(DirectoryName, fileNames[i]);
                        using (StreamReader r = new StreamReader(store.OpenFile(fn, System.IO.FileMode.Open)))
                        {
                            string stringdata = r.ReadToEnd();
                            data[i] = new ChartData(stringdata);
                            data[i].FileName = fileNames[i];
                        }
                    }
                    filesInSubDirs = data;
                }
            }
            catch (IsolatedStorageException)
            {
                MessageBox.Show("Error", "Unable to access isloated storage", MessageBoxButton.OK);
            }
            return filesInSubDirs;
        }

        class ChartFileComparer : IComparer<ChartData>
        {
            public int Compare(ChartData x, ChartData y)
            {
                return x.FileName.CompareTo(y.FileName);
            }
        }
        private int updateList()
        {
            ChartData[] files = AvailableFiles();
            Array.Sort(files, new ChartFileComparer());
            Dispatcher.BeginInvoke(() =>
            {

                listBox1.ItemsSource = files;
            });
            if (files == null)
            {
                return 0;
            }
            return files.Length;
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string selectedFile = ((ChartData)listBox1.SelectedItem).FileName;
                openFile(System.IO.Path.Combine(DirectoryName, selectedFile));
            }
            catch (NullReferenceException)
            {
                // silently ignore... might be we just deleted the file
            }
        }

        private void openFile(string path)
        {
            try
            {
                string stringdata = "";
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (StreamReader r = new StreamReader(store.OpenFile(path, System.IO.FileMode.Open)))
                    {
                        stringdata = r.ReadToEnd();
                    }
                }
                string fn = System.IO.Path.GetFileName(path);
                App app = ((App)App.Current);
                if (!IsComparison)
                {
                    app.IsCompareChart = false;
                    app.IsTransitChart = false;
                    app.currentChartData.updateFromSerialized(stringdata);
                    app.chartFileName = fn;
                    app.InfoPanelSelectedIndex = -1;
                }
                else
                {
                    app.IsCompareChart = true;
                    app.IsTransitChart = false;
                    app.currentCompareChartData = new ChartData(stringdata);
                    app.chartCompareFileName = fn;
                }
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
            catch (IsolatedStorageException)
            {
                MessageBox.Show("Error", "Unable to access isloated storage", MessageBoxButton.OK);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            string fn = ((ChartData)((FrameworkElement)sender).DataContext).FileName;
            MessageBoxResult m = MessageBox.Show("Really delete " + fn + "?", "Delete file", MessageBoxButton.OKCancel);
            if (m != MessageBoxResult.OK)
            {
                return;
            }
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    store.DeleteFile(System.IO.Path.Combine(DirectoryName, fn));
                }
                updateList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error deleting file", "Error", MessageBoxButton.OK);
            }
        }

        private void deleteImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            deleteButton_Click(sender, e);
        }
    }
}