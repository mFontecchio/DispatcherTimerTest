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
        DispatcherTimer timer;                  // Timer to start and stop timer, tick every 15s
        private int queueCount;                 //Helper for initial Array population

        //Initial Create Method
        public MeasureLengthDevice()
        {
            this.unitsToUse = Units.Metric;
            this.dataCaptured = new int[10];
            this.mostRecentMeasure = 0;
            this.queueCount = 0;
        }

        //Property for unitsToUse
        public Units UnitsToUse
        {
            get => this.unitsToUse;
            set => this.unitsToUse = value;
        }

        //Get/Set method for mostRecentMeasure
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

        //Get method for tick Counter
        public int QueueCount
        {
            get => this.queueCount;
        }

        //Return the contents of the dataCaptured array.
        public int[] GetRawData()
        {
            return this.dataCaptured;
        }

        //Return the current value from mostRecentMeasure- convert it if unitsToUse is not Imperial.
        public decimal ImperialValue(decimal capturedValue)
        {
            decimal convertedValue = capturedValue;

            if (this.unitsToUse != Units.Imperial)
            {
                convertedValue = convertedValue * 0.3937m;
            }
            return convertedValue;
        }

        //Return the current value from mostRecentMeasure- convert it if unitsToUse is not Metric.
        public decimal MetricValue(decimal capturedValue)
        {
            decimal convertedValue = capturedValue;

            if (this.unitsToUse != Units.Metric)
            {
                convertedValue = convertedValue * 2.54m;
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
            //Method to store and continually populate the array with current measurements
            StoreHistory(this.mostRecentMeasure);
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

        //Store DataCaptured array
        //This will write until the array is full. 
        //Upon a full array it will then shift all values down 1 index and overwrite index 9 or the max index.
        private void StoreHistory(int measurement)
        {
            if (this.queueCount < 10)
            {
                this.dataCaptured[this.queueCount] = measurement;
                this.queueCount++;

            }

            if (this.queueCount >= 10)
            {
                for (int i = 0; i < this.dataCaptured.Length - 1; i++)
                {
                    if (i < this.dataCaptured.Length - 1)
                    {
                        this.dataCaptured[i] = this.dataCaptured[i + 1];
                    }
                }
                this.dataCaptured[dataCaptured.Length - 1] = measurement;
            }
        }
    }
}
