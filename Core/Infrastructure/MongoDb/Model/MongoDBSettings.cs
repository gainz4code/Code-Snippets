namespace Core.Infrastructure.MongoDb.Model;
public class MongoDBSettings
{
    public string ConnectionURI { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
