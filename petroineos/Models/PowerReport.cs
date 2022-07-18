using CsvHelper.Configuration.Attributes;

namespace Petroineos.Models
{
    public class PowerReport
    {
        [Name("Local Time")]
        public string LocalTime { get; set; }
        public int Volumn { get; set; }
    }
}
