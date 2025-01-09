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
           
        }

        public async Task CreateUserAsync(UserCredentials user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            //Generacion de Jwt
            user.CurrentToken = _jwtService.GeneratedToken(ObjectId.GenerateNewId().ToString(), user.Email);
            await _userCollection.InsertOneAsync(user);
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
    }
}
