using MongoDB.Driver;

namespace RecipesharingService.MongoDB
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            // Retrieve the connection string and database name from appsettings.json
            var connectionString = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"];

            // Create a MongoDB client and connect to the database
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        // Generic method to get a collection by name
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
