namespace Petroineos.Interfaces
{
    public interface IConfigReaderService
    {
        string CsvFilePath { get; }
        int ScheduleIntervalRunMinutes { get; }
    }
}
