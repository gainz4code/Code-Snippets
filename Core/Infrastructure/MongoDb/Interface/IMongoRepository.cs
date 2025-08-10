using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Infrastructure.MongoDb.Interface;

public interface IMongoRepository<TDocument> where TDocument : IDocument
{
    Task<TDocument> FindByIdAsync(string id);

    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    Task<List<TDocument>> GetAllAsync();

    Task<List<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression);

    Task<List<TProjected>> FilterByAsync<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression);

    Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents);

    Task<TDocument> InsertOneAsync(TDocument document);

    Task<TDocument> ReplaceOneAsync(TDocument document);

    Task<TDocument> ReplaceOneAsync(IClientSessionHandle session, TDocument document);

    Task DeleteByIdAsync(string id);

    Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);

    Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
}
