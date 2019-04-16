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
            this.dataTest.Text = this.newDevice.MostRecentMeasure.ToString();

            if (this.newDevice.UnitsToUse == Units.Metric)
            {
                this.newMeasure.Text = this.newDevice.ImperialValue(this.newDevice.MostRecentMeasure).ToString();
            }
            else
            {
                this.newMeasure.Text = this.newDevice.MetricValue(this.newDevice.MostRecentMeasure).ToString();
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.newDevice.StartCollecting();
            toggleTest.Content = "Stop";
        }

        private void ToggleTest_Unchecked(object sender, RoutedEventArgs e)
        {
            this.newDevice.StopCollecting();
            toggleTest.Content = "Start";
        }

        private void MetricRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.newDevice.UnitsToUse = Units.Metric;
        }

        private void ImperialRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.newDevice.UnitsToUse = Units.Imperial;
        }
    }
}
