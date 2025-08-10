using Core.Infrastructure.MongoDb.Interface;
using Core.Infrastructure.MongoDb.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Infrastructure.MongoDb.Implementation;

public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument //IIDocument because it will give id field as accesable
{
    public readonly IMongoDatabase _database;
    public MongoRepository(IOptions<MongoDBSettings> mongoDBSettings)
    {
        ArgumentNullException.ThrowIfNull(mongoDBSettings);

        var settings = MongoClientSettings.FromConnectionString(mongoDBSettings.Value.ConnectionURI);
        // Set the ServerApi field of the settings object to set the version of the Stable API on the client
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        MongoClient client = new MongoClient(settings);
        _database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public virtual IMongoCollection<TDocument> GetCollection
    {
        get
        {
            return _database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }
    }

    public string GetCollectionName(Type documentType)
    {
        var collectionAttribute = (BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()!;
        return collectionAttribute?.CollectionName ?? string.Empty;
    }

    public virtual IQueryable<TDocument> Table
    {
        get
        {
            return GetCollection.AsQueryable();
        }
    }

    public async Task<TDocument> FindByIdAsync(string id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        return await GetCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await GetCollection.Find(filterExpression).FirstOrDefaultAsync();
    }

    public async Task<List<TDocument>> GetAllAsync()
    {
        return await GetCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<List<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await GetCollection.Find(filterExpression).ToListAsync();
    }

    public async Task<List<TProjected>> FilterByAsync<TProjected>(Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        return await GetCollection.Find(filterExpression).Project(projectionExpression).ToListAsync();
    }

    public async Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents)
    {
        await GetCollection.InsertManyAsync(documents);
        return documents;
    }

    public async Task<TDocument> InsertOneAsync(TDocument document)
    {
        await GetCollection.InsertOneAsync(document);
        return document;
    }

    public async Task<TDocument> ReplaceOneAsync(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        await GetCollection.FindOneAndReplaceAsync(filter, document);
        return document;
    }

    public async Task<TDocument> ReplaceOneAsync(IClientSessionHandle session, TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        await GetCollection.FindOneAndReplaceAsync(session, filter, document);
        return document;
    }

    public async Task DeleteByIdAsync(string id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        await GetCollection.DeleteOneAsync(filter);
    }

    public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await GetCollection.DeleteManyAsync(filterExpression);
    }

    public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await GetCollection.FindOneAndDeleteAsync(filterExpression);
    }
}
