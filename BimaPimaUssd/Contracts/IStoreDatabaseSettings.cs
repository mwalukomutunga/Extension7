namespace BimaPimaUssd.Contracts
{
    public interface IStoreDatabaseSettings
    {
        string FarmCollection { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
