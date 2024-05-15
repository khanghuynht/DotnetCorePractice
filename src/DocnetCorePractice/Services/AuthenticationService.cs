using DocnetCorePractice.Data.Entity;
using DocnetCorePractice.Model;
using DocnetCorePractice.Repository;
using DocnetCorePractice.Service;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocnetCorePractice.Services
{
    public interface IAuthenticationService
    {
        ResponseLoginModel Authenticator(RequestLoginModel model);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string Key = "suifbweudfwqudgweufgewufgwefcgweiudgweidgwed";
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokensRepository _refreshTokenRepository;
        public AuthenticationService(IUserRepository userRepository, IRefreshTokensRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public ResponseLoginModel Authenticator(RequestLoginModel model)
        {
            var account = _userRepository.GetAllUser().Where(x => x.Account == model.Account && x.Password == model.Password).FirstOrDefault();

            if (account == null)
            {
                throw new ArgumentException("Wrong password or account does not exist");
            }

            var token = CreateJwtToken(account);
            var refreshToken = CreateRefreshToken(account);
            var result = new ResponseLoginModel
            {
                FullName = account.FirstName,
                UserId = account.Account,
                Token = token,
                RefreshToken = refreshToken.Token
            };
            return result;
        }

        private RefreshTokens CreateRefreshToken(UserEntity account)
        {
            Random res = new Random();

            // String that contain both alphabets and numbers 
            string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int size = 10;

            // Initializing the empty string 
            string result = "";

            for (int i = 0; i < size; i++)
            {

                // Selecting a index randomly 
                int x = res.Next(str.Length);

                // Appending the character at the  
                // index to the random alphanumeric string. 
                result = result + str[x];
            }
            var token = result; // Viết hàm tạo chuỗi random string
            var refreshToken = new RefreshTokens
            {
                Id = Guid.NewGuid(),
                UserId = account.Id,
                Expires = DateTime.Now.AddDays(1),
                IsActive = true,
                Token = token
            };
            // viết code insert refreshToken vào DB
            var save = _refreshTokenRepository.InsertToken(refreshToken);
            if(save <= 0)
            {
                throw new ArgumentException("Error when insert refresh token");
            }
            return refreshToken;
        }

        private string CreateJwtToken(UserEntity account)
        {
            var tokenHanler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Key);
            var securityKey = new SymmetricSecurityKey(key);
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, account.FirstName),
                    new Claim(ClaimTypes.Email, "sdasdasd"),
                    new Claim("CarNumber", "1")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credential
            };
            var token = tokenHanler.CreateToken(tokenDescription);
            return tokenHanler.WriteToken(token);
        }
    }
}
