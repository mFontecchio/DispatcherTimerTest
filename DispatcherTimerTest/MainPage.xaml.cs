using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DispatcherTimerTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Instantiate MeasureLengthDevice object
        private MeasureLengthDevice newDevice = null;

        public MainPage()
        {
            this.InitializeComponent();

            //Create newDevice
            this.newDevice = new MeasureLengthDevice();
            //Create newDevice object TickHandler for use by UI
            this.newDevice.NewMeasureTick += new MeasureLengthDevice.TimerTickHandler(TickedTime);

            //Initialize RadioButton DefaultCheck
            this.metricRadioButton.IsChecked = true;
        }

        //On Tick_Timer write to textblock using delegate handler.
        //Subscribes to notification of the above delegate.
        private void TickedTime(int mostRecentMeasure)
        {
            //Data Bind for mostRecentMeasure
            DataContext = null;
            DataContext = this.newDevice;

            //Direct Update - Before WK 6 Tips
            //this.mostRecentMeasure.Text = this.newDevice.MostRecentMeasure.ToString();

            //Display timestamp on tick
            timeStamp.Text = GetTimeStamp(DateTime.Now);

            measureListView.Items.Clear();
            int[] history = newDevice.GetRawData();

            for (int i = history.Length - 1; i >= 0; i--)
            {
                if (history[i] != 0)
                {
                    measureListView.Items.Add(history[i]);
                }
            }

            //Convert recentMeasure to the opposite unit that is selected
            if (this.newDevice.UnitsToUse == Units.Metric)
            {
                this.convertedUnit.Text = $"{this.newDevice.ImperialValue(this.newDevice.MostRecentMeasure).ToString()} in.";
            }
            else
            {
                this.convertedUnit.Text = $"{this.newDevice.MetricValue(this.newDevice.MostRecentMeasure).ToString()} cm.";
            }
        }

        //Start Collection
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.newDevice.StartCollecting();
            startStopToggle.Content = "Stop";
        }

        //Stop Collection
        private void ToggleTest_Unchecked(object sender, RoutedEventArgs e)
        {
            this.newDevice.StopCollecting();
            startStopToggle.Content = "Start";
        }

        //User select units to convert from
        private void MetricRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.newDevice.UnitsToUse = Units.Metric;
        }

        private void ImperialRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.newDevice.UnitsToUse = Units.Imperial;
        }

        //Generate String Timestamp to display in textblock
        private static String GetTimeStamp(DateTime date)
        {
            return date.ToString("MM/dd/yyyy hh:mm:ss tt");
        }

        //Show Measure History
        private void ShowHistory_Checked(object sender, RoutedEventArgs e)
        {
            //Order list with most recent towards the top.
            /*int[] history = newDevice.GetRawData();

            for (int i = history.Length-1; i >= 0; i--)
            {
                if (history[i] != 0)
                {
                    measureListView.Items.Add(history[i]);
                }
            }*/

            showHistory.Content = "Clear History";
            measureListView.Visibility = Visibility.Visible;
        }

        //Hide History and clear the list
        private void ShowHistory_Unchecked(object sender, RoutedEventArgs e)
        {
            //measureListView.Items.Clear();
            showHistory.Content = "Show History";
            measureListView.Visibility = Visibility.Collapsed;
        }
    }
}
