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
using Microsoft.Phone.Controls.Primitives;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;

namespace AstrologyToolkit
{
    public partial class GeoPositionSelectorPage : PhoneApplicationPage
    {
        public abstract class LoopingDataSourceBase : ILoopingSelectorDataSource
        {
            private object selectedItem;

            #region ILoopingSelectorDataSource Members

            public abstract object GetNext(object relativeTo);

            public abstract object GetPrevious(object relativeTo);

            public object SelectedItem
            {
                get
                {
                    return this.selectedItem;
                }
                set
                {
                    // this will use the Equals method if it is overridden for the data source item class
                    if (!object.Equals(this.selectedItem, value))
                    {
                        // save the previously selected item so that we can use it
                        // to construct the event arguments for the SelectionChanged event
                        object previousSelectedItem = this.selectedItem;
                        this.selectedItem = value;
                        // fire the SelectionChanged event
                        this.OnSelectionChanged(previousSelectedItem, this.selectedItem);
                    }
                }
            }

            public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

            protected virtual void OnSelectionChanged(object oldSelectedItem, object newSelectedItem)
            {
                EventHandler<SelectionChangedEventArgs> handler = this.SelectionChanged;
                if (handler != null)
                {
                    handler(this, new SelectionChangedEventArgs(new object[] { oldSelectedItem }, new object[] { newSelectedItem }));
                }
            }

            #endregion
        }
        public class IntLoopingDataSource : LoopingDataSourceBase
        {
            private int minValue;
            private int maxValue;
            private int increment;

            public IntLoopingDataSource()
            {
                this.MaxValue = 10;
                this.MinValue = 0;
                this.Increment = 1;
                this.SelectedItem = 0;
            }

            public int MinValue
            {
                get
                {
                    return this.minValue;
                }
                set
                {
                    if (value >= this.MaxValue)
                    {
                        throw new ArgumentOutOfRangeException("MinValue", "MinValue cannot be equal or greater than MaxValue");
                    }
                    this.minValue = value;
                }
            }

            public int MaxValue
            {
                get
                {
                    return this.maxValue;
                }
                set
                {
                    if (value <= this.MinValue)
                    {
                        throw new ArgumentOutOfRangeException("MaxValue", "MaxValue cannot be equal or lower than MinValue");
                    }
                    this.maxValue = value;
                }
            }

            public int Increment
            {
                get
                {
                    return this.increment;
                }
                set
                {
                    if (value < 1)
                    {
                        throw new ArgumentOutOfRangeException("Increment", "Increment cannot be less than or equal to zero");
                    }
                    this.increment = value;
                }
            }

            public override object GetNext(object relativeTo)
            {
                int nextValue = (int)relativeTo + this.Increment;
                if (nextValue > this.MaxValue)
                {
                    nextValue = this.MinValue;
                }
                return nextValue;
            }

            public override object GetPrevious(object relativeTo)
            {
                int prevValue = (int)relativeTo - this.Increment;
                if (prevValue < this.MinValue)
                {
                    prevValue = this.MaxValue;
                }
                return prevValue;
            }
        }

        public class StringLoopingDataSource : LoopingDataSourceBase
        {
            private String[] values;

            public StringLoopingDataSource(String[] strings)
            {
                this.SelectedItem = 0;
                this.values = strings;
            }

            private int indexOf(string s)
            {
                for (int i = 0; i < values.Length; i++ )
                {
                    if (values[i] == s)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public override object GetNext(object relativeTo)
            {
                int i = indexOf(relativeTo.ToString());
                if (i >= values.Length - 1)
                {
                    return values[0];
                }
                return values[i + 1];
            }

            public override object GetPrevious(object relativeTo)
            {
                int i = indexOf(relativeTo.ToString());
                if (i <= 0)
                {
                    return values[values.Length-1];
                }
                return values[i - 1];
            }
        }

        private void HandleSelectorIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                // Ensure that only one selector is expanded at a time
                selector1.IsExpanded = (sender == selector1);
                selector2.IsExpanded = (sender == selector2);
                selector3.IsExpanded = (sender == selector3);
                selector4.IsExpanded = (sender == selector4);
                selector5.IsExpanded = (sender == selector5);
                selector6.IsExpanded = (sender == selector6);
            }
        }

        public GeoPositionSelectorPage()
        {
            InitializeComponent();
            this.selector1.DataSource = new IntLoopingDataSource() { MinValue = 0, MaxValue = 89, SelectedItem = 1 };
            this.selector2.DataSource = new StringLoopingDataSource(new string[] { "N", "S" });
            this.selector3.DataSource = new IntLoopingDataSource() { MinValue = 0, MaxValue = 99, SelectedItem = 1 };

            this.selector4.DataSource = new IntLoopingDataSource() { MinValue = 0, MaxValue = 179, SelectedItem = 1 };
            this.selector5.DataSource = new StringLoopingDataSource(new string[] { "E", "W" });
            this.selector6.DataSource = new IntLoopingDataSource() { MinValue = 0, MaxValue = 99, SelectedItem = 1 };

            selector1.IsExpandedChanged += new DependencyPropertyChangedEventHandler(HandleSelectorIsExpandedChanged);
            selector2.IsExpandedChanged += new DependencyPropertyChangedEventHandler(HandleSelectorIsExpandedChanged);
            selector3.IsExpandedChanged += new DependencyPropertyChangedEventHandler(HandleSelectorIsExpandedChanged);
            selector4.IsExpandedChanged += new DependencyPropertyChangedEventHandler(HandleSelectorIsExpandedChanged);
            selector5.IsExpandedChanged += new DependencyPropertyChangedEventHandler(HandleSelectorIsExpandedChanged);
            selector6.IsExpandedChanged += new DependencyPropertyChangedEventHandler(HandleSelectorIsExpandedChanged);

            selector1.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(SelectorChanged);
            selector2.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(SelectorChanged);
            selector3.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(SelectorChanged);
            selector4.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(SelectorChanged);
            selector5.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(SelectorChanged);
            selector6.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(SelectorChanged);

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

        private bool isUpdating = false;

        private void updateFromTemp()
        {
            int tempLat = ((App)App.Current).TempLatitude;
            int tempLong = ((App)App.Current).TempLongitude;
            
            int latDeg = (int)(Math.Abs(tempLat)) / (int)100;
            int latCent = (int)(Math.Abs(tempLat)) - (latDeg * 100);
            string latDirS = (tempLat < 0 ? "S" : "N");

            int longDeg = (int)(Math.Abs(tempLong)) / (int)100;
            int longCent = (int)(Math.Abs(tempLong)) - (longDeg * 100);
            string longDirS = (tempLong < 0 ? "W" : "E");

            isUpdating = true;

            selector1.DataSource.SelectedItem = latDeg;
            selector2.DataSource.SelectedItem = latDirS;
            selector3.DataSource.SelectedItem = latCent;

            selector4.DataSource.SelectedItem = longDeg;
            selector5.DataSource.SelectedItem = longDirS;
            selector6.DataSource.SelectedItem = longCent;

            isUpdating = false;
        }

        private void SelectorChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUpdating)
            {
                return;
            }
            int latDeg = (int)selector1.DataSource.SelectedItem;
            string latDirS = (string)selector2.DataSource.SelectedItem;
            int latDir = (latDirS == "N" ? 1 : -1);
            int latCent = (int)selector3.DataSource.SelectedItem;

            ((App)App.Current).TempLatitude = ((latDeg * 100) + latCent) * latDir;

            int longDeg = (int)selector4.DataSource.SelectedItem;
            string longDirS = (string)selector5.DataSource.SelectedItem;
            int longDir = (longDirS == "E" ? 1 : -1);
            int longCent = (int)selector6.DataSource.SelectedItem;

            ((App)App.Current).TempLongitude = ((longDeg * 100) + longCent) * longDir;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            updateFromTemp();
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            ((App)App.Current).tempChartData.Latitude = ((float)((App)App.Current).TempLatitude) / 100.0f;
            ((App)App.Current).tempChartData.Longitude = ((float)((App)App.Current).TempLongitude) / 100.0f;
            ((App)App.Current).tempChartData.PlaceName = "Custom";
            ((App)App.Current).tempChartData.CountryName = "Custom";

            NavigationService.GoBack();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}