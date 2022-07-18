using log4net;
using Petroineos.Interfaces;
using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace Petroineos
{
    public partial class SchedulerService : ServiceBase
    {
        private readonly ILog _log;
        private readonly IConfigReaderService _configReader;
        private readonly IReportCreatorService _reportCreator;
        private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        private Thread _thread;
        private int _scheduleIntervalRunMilliSeconds;
        public SchedulerService() {
            InitializeComponent();
        }

        public SchedulerService(ILog log, IConfigReaderService configReader, IReportCreatorService reportCreator)
        {
            InitializeComponent();
            _log = log;
            _configReader = configReader;
            _reportCreator = reportCreator;
        }
        protected override void OnStart(string[] args)
        {

            _scheduleIntervalRunMilliSeconds = _configReader.ScheduleIntervalRunMinutes * 60000; // minutes -> milliseconds
            _thread = new Thread(heartbeat_Elapsed);
            _thread.Name = "Petroineos Worker Thread";
            _thread.IsBackground = true;
            _thread.Start();

            _log.Debug($"Petroineos Windows Service STARTED {Environment.MachineName}");
        }

        protected override void OnStop()
        {
            _shutdownEvent.Set();
            if (!_thread.Join(3000))
            { // give the thread 3 seconds to stop
                _thread.Abort();
            }

            _log.Debug($"Petroineos Windows Service STOPPED {Environment.MachineName}");
        }

        private void heartbeat_Elapsed()
        {
            // we're going to wait 5 minutes between calls to GetEmployees, so 
            // set the interval to 300000 milliseconds 
            // (1000 milliseconds = 1 second, 5 * 60 * 1000 = 300000)
            int interval = _scheduleIntervalRunMilliSeconds; // 5 minutes    
                                   // this variable tracks how many milliseconds have gone by since 
                                   // the last call to GetEmployees. Set it to zero to indicate we're 
                                   // starting fresh
            int elapsed = _scheduleIntervalRunMilliSeconds;
            // because we don't want to use 100% of the CPU, we will be 
            // sleeping for 1 second between checks to see if it's time to 
            // call _reportCreator.Process()
            int waitTime = 1000; // 1 second
            try
            {
                // do this loop forever (or until the service is stopped)
                while (true)
                {
                    // if enough time has passed
                    if (interval <= elapsed)
                    {
                        // reset how much time has passed to zero
                        elapsed = 0;
                        // call _reportCreator.Process()
                        _reportCreator.Process();
                    }
                    // Sleep for 1 second
                    Thread.Sleep(waitTime);
                    // indicate that 1 additional second has passed
                    elapsed += waitTime;
                }
            }
            catch (ThreadAbortException ex)
            {
                _log.Error(ex.Message);
                // we want to eat the excetion because we don't care if the 
                // thread has aborted since we probably did it on purpose by 
                // stopping the service.
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
            }
        }

        public void Writefile(string line)
        {
            string[] lines = { line };
            File.WriteAllLines($"c:\\{line}", lines);
        }
    }
}
