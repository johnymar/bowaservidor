using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    /// <summary>
    /// The Watchdog Application
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ///ApplicationWatcher initialization

            int monitoringInterval = 5000;

            try
            {
                monitoringInterval = Convert.ToInt32(ConfigurationManager.AppSettings["MonitoringInterval"]);

                if (monitoringInterval == 0)
                {
                    monitoringInterval = 5000;
                }
            }
            catch (Exception ex)
            {
                monitoringInterval = 5000;
                Debug.WriteLine("ApplicationWatcher Exception2: " + ex.StackTrace);
            }

            ApplicationWatcher applicationWatcher = new ApplicationWatcher("MonitoredApplication", "copiar_temp.exe", 5000);
            ApplicationWatcher applicationWatcher1 = new ApplicationWatcher("MonitoredApplication", "copiar_temp2.exe", 5000);
            ApplicationWatcher applicationWatcher2 = new ApplicationWatcher("MonitoredApplication", "copiar_temp3.exe", 5000);
        }
    }
}
