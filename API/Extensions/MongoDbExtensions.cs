using Core.Infrastructure.MongoDb.Implementation;
using Core.Infrastructure.MongoDb.Interface;
using Core.Infrastructure.MongoDb.Model;

namespace API.Extensions;

public static class MongoDbExtensions
{
    public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDBSettings>(configuration.GetSection("MongoDB"));
        services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));
    }
}

