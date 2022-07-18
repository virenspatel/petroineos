using Autofac;
using log4net.Config;
using Petroineos.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Petroineos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            XmlConfigurator.Configure();
            var container = Bootstrapper.BuildContainer();

            var scheduleService =  container.Resolve<SchedulerService>();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                scheduleService
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
