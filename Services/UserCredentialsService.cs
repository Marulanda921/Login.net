using ApiLoginFull.Model;
using ApiLoginFull.Utils.Dto;
using BCrypt.Net;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiLoginFull.Services
{
    public class UserCredentialsService
    {
        private readonly IMongoCollection<UserCredentials> _userCollection;
        private readonly JwtService _jwtService;
        public UserCredentialsService(IConfiguration configuration, JwtService jwtService)
        {
            _jwtService = jwtService;

            var mongoSettings = configuration.GetSection("MongoDB");
            var client = new MongoClient(mongoSettings["ConnectionString"]);
            var database = client.GetDatabase(mongoSettings["DatabaseName"]);
            _userCollection = database.GetCollection<UserCredentials>(mongoSettings["CollectionName"]);
            CreateUniqueEmailIndex();
        }


        private void CreateUniqueEmailIndex()
        {
            var indexKeysDefinition = Builders<UserCredentials>.IndexKeys.Ascending(user => user.Email);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<UserCredentials>(indexKeysDefinition, indexOptions);
            _userCollection.Indexes.CreateOne(indexModel);
        }

        public async Task CreateUserAsync(UserCredentials user)
        {
            //Verificar si el email ya existe
            var existingUser = await _userCollection
                .Find(x => x.Email == user.Email)
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                throw new DuplicateEmailException($"El correo electrónico '{user.Email}' ya está registrado.");
            }

        }

        public async Task<LoginResponse> LoginAsync(string email, string password) { 
            
            var user =  await _userCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new UnauthorizedAccessException("Credenciales Invalidas");
            }

            if (string.IsNullOrEmpty(user.CurrentToken)|| !_jwtService.ValidateToken(user.CurrentToken))
            {
                user.CurrentToken = _jwtService.GeneratedToken(user.Id.ToString(), user.Email);
                var update = Builders<UserCredentials>.Update.Set(u => user.CurrentToken, user.CurrentToken);
                await _userCollection.UpdateOneAsync(x => x.Id == user.Id, update);
            }
            return new LoginResponse
            {
                Token = user.CurrentToken,
                Email = user.Email
            };
        }

        public class DuplicateEmailException : Exception
        {
            public DuplicateEmailException(string message) : base(message)
            {
            }
        }
    }
}
