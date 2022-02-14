using BimaPimaUssd.Contracts;

namespace BimaPimaUssd.Models
{
    public class StoreDatabaseSettings : IStoreDatabaseSettings
    {
        public StoreDatabaseSettings()
        {
            FarmCollection = "BimaExtn";
            ConnectionString = @"mongodb://localhost:27017";
            DatabaseName = "BimaExtn";

        }
        public string FarmCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
