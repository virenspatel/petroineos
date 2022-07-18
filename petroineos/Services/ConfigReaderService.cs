using Petroineos.Helpers;
using Petroineos.Interfaces;

namespace Petroineos.Services
{
    public class ConfigReaderService : IConfigReaderService
    {
        public string CsvFilePath { get { return ConfigurationReader.GetConfigKeyValue<string>("CsvfilePath"); } }
        public int ScheduleIntervalRunMinutes { get { return ConfigurationReader.GetConfigKeyValue<int>("ScheduleIntervalRunMinutes"); } }
    }
}
