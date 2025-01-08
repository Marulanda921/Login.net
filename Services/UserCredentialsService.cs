using ApiLoginFull.Model;
using MongoDB.Driver;

namespace ApiLoginFull.Services
{
    public class UserCredentialsService
    {
        private readonly IMongoCollection<UserCredentials> _userCollection;

        public UserCredentialsService(IConfiguration configuration) {
            var mongoSettings = configuration.GetSection("MongoDB");
            var client = new MongoClient(mongoSettings["ConnectionString"]);
            var database = client.GetDatabase(mongoSettings["DatabaseName"]);
            _userCollection = database.GetCollection<UserCredentials>(mongoSettings["CollectionName"]);
        }

        public async Task CreateUserAsync(UserCredentials user)
        {
            await _userCollection.InsertOneAsync(user);
        }
    }
}
