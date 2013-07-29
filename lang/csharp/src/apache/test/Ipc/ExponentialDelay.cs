using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace Avro.test.Ipc
{
    [TestFixture]
    public class ExponentialDelay
    {
        public static int Exponential(int failedAttempts)
        {
            int delayInSeconds = (1 << failedAttempts)*1000;

            return Math.Min(delayInSeconds, 32000);
        }

        private DateTime? lastReconnect;
        private int failedAttempts;

        [Test]
        public void MethodUnderTest_Scenario_ExpectedBehaviour()
        {
            var watch = new Stopwatch();
            watch.Start();
            

            Retry();
            Retry();
            Retry();
            Retry();
            Retry();
     

            watch.Stop();

            Assert.Inconclusive(watch.ElapsedMilliseconds.ToString());

        }

        public void Retry()
        {

            if (lastReconnect != null)
            {
                // If failed within the last 5 seconds, wait a bit longer before retrying again.
                if (DateTime.UtcNow.Subtract(lastReconnect.Value) < new TimeSpan(0, 0, 5))
                {
                    int delayInMilliseconds = Exponential(failedAttempts++);

                    Thread.Sleep(delayInMilliseconds);
                }
                else
                {
                    failedAttempts = 0;
                }
            }

            lastReconnect = DateTime.UtcNow;
        }

    }
}
