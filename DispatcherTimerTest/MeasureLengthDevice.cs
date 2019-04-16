using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DispatcherTimerTest
{
    class MeasureLengthDevice : Device, IMeasuringDevice
    {
        private Units unitsToUse;               //Units - This field will determine whether the generated measurements are metric or imperial. Its value will be determined from user input.
        private int[] dataCaptured;             //This field will store a history of a limited set of recently captured measurements. Once the array is full, the class should start overwriting the oldest elements while continuing to record the newest captures.
        private int mostRecentMeasure;          //This field will store the most recent measurement captured for convenience of display
        DispatcherTimer timer;

        //Initial Create Method
        public MeasureLengthDevice()
        {
            this.unitsToUse = Units.Metric;
            this.dataCaptured = new int[0];
            this.mostRecentMeasure = 0;
        }

        //Property for unitsToUse
        public Units UnitsToUse
        {
            get => this.unitsToUse;
            set => this.unitsToUse = value;
        }

        public int MostRecentMeasure
        {
            get => this.mostRecentMeasure;
            set
            {
                this.mostRecentMeasure = value;
                //Also set the TickHandler for UI may not be needed the more I think about it?
                OnNewMeasureTick(this.mostRecentMeasure);
            }
        }

        //Return the contents of the dataCapturedarray.
        public int[] GetRawData()
        {
            throw new NotImplementedException();
        }

        //Return the current value from mostRecentMeasure- convert it if unitsToUse is not Imperial.
        public double ImperialValue(double capturedValue)
        {
            double convertedValue = capturedValue;

            if (this.unitsToUse != Units.Imperial)
            {
                convertedValue = convertedValue * 0.3937;
            }
            return convertedValue;
        }

        //Return the current value from mostRecentMeasure- convert it if unitsToUse is not Metric.
        public double MetricValue(double capturedValue)
        {
            double convertedValue = capturedValue;

            if (this.unitsToUse != Units.Metric)
            {
                convertedValue = convertedValue * 2.54;
            }
            return convertedValue;
        }

        /*Start timer to collect data from Device.GetMeasurement() every 15 seconds,
        set the value to mostRecentMeasure and store it to dataCaptured array.*/
        public void StartCollecting()
        {
            //Create timer object and intialize
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            //set interval to 15 seconds
            timer.Interval = new TimeSpan(0, 0, 15);
            //start timer
            timer.Start();
            //get a measurement when timer starts
            timer_Tick(this, this);
    }

        //Stop the timer that started in StartCollecting().
        public void StopCollecting()
        {
            timer.Stop();
        }

        //Tick evenHandler for storing data and updating the MostRecentMeasure
        private void timer_Tick(object sender, object e)
        {
            //Generate random number between 1 & 10
            this.MostRecentMeasure = this.GetMeasurement();
        }

        //Set up Delegate to publish notification when timer_tick event fires.
        public delegate void TimerTickHandler(int mostRecentMeasure);
        public event TimerTickHandler NewMeasureTick;

        //Event Publisher Method
        protected void OnNewMeasureTick(int mostRecentMeasure)
        {
            //If Tick is not blank publish notification
            if (NewMeasureTick != null)
            {
                NewMeasureTick(mostRecentMeasure);
            }
        }
    }
}
