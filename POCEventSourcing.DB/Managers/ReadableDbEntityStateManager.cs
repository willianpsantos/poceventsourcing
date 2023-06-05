using Microsoft.Extensions.Options;
using MongoDB.Driver;
using POCEventSourcing.Core;
using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.Options;

namespace POCEventSourcing.DB.Managers
{
    public class ReadableDbEntityStateManager : IReadableDbEntityStateManager
    {
        private readonly MongoClientSettings _settings;
        private readonly MongoClient _client;
        private readonly ReadableDatabaseOptions _databaseOptions;

        public ReadableDbEntityStateManager(IOptions<ReadableDatabaseOptions> options)
        {
            _databaseOptions = options.Value;

            var connectionString = MongoDBConnectionStringBuilder.BuildConnectionString(_databaseOptions);

            _settings = MongoClientSettings.FromConnectionString(connectionString);
            _client = new MongoClient(_settings);
        }

        private string GetCollection<TEntity>() where TEntity: Entity => typeof(TEntity).Name;

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var collectionName = GetCollection<TEntity>();
            var database = _client.GetDatabase(_databaseOptions.Database);
            var collection = database.GetCollection<TEntity>(collectionName);
            var filter = new JsonFilterDefinition<TEntity>($"{{ _id: {entity.Id} }}");

            await collection.DeleteOneAsync(filter);
        }

        public async Task<long> InsertAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var collectionName = GetCollection<TEntity>();
            var database = _client.GetDatabase(_databaseOptions.Database);
            var collection = database.GetCollection<TEntity>(collectionName);

            await collection.InsertOneAsync(entity);

            return entity.Id;
        }

        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var collectionName = GetCollection<TEntity>();
            var database = _client.GetDatabase(_databaseOptions.Database);
            var collection = database.GetCollection<TEntity>(collectionName);
            var filter = new JsonFilterDefinition<TEntity>($"{{ _id: {entity.Id} }}");
            var findResult = await collection.FindAsync<TEntity>(filter);
            var mongoEntity = await findResult.FirstOrDefaultAsync();

            if (mongoEntity == null)
            {
                throw new KeyNotFoundException("Entity not found!");
            }

            await collection.ReplaceOneAsync(filter, entity);
        }
    }
}
