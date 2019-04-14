using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherTimerTest
{
    public class Device
    {
        //This method will return a random integer between 1 and 10 as a measurement of some imaginary object
        public int GetMeasurement()
        {
            Random randMeasurement = new Random();
            return randMeasurement.Next(1, 11);
        }
    }
}
