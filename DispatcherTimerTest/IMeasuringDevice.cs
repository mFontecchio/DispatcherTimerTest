using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherTimerTest
{
    interface IMeasuringDevice
    {
        //This method will return a decimal that represents the metric value of the most recent measurement that was captured.
        decimal MetricValue(decimal capturedValue);

        //This method will return a decimal that represents the imperial value of the most recent measurement that was captured.
        decimal ImperialValue(decimal capturedValue);

        //This method will start the device running. It will begin collecting measurements and record them.
        void StartCollecting();

        //This method will stop the device. It will cease collecting measurements.
        void StopCollecting();

        //This method will retrieve a copy of all of the recent data that the measuring device has captured. The data will be returned as an array of integer values.
        int[] GetRawData();
    }
}
