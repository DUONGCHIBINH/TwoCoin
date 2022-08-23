using TwoCoinApi.Models;
using TwoCoinApi.Repos.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TwoCoinApi.Services
{
    public class AuthManager
    {
        private readonly IConfiguration _config;
        private readonly TokenManager _tokenManager;
        private readonly UserManager _userManager;

        public AuthManager(IConfiguration config, TokenManager tokenManager, UserManager userManager)
        {
            _config = config;
            _tokenManager = tokenManager;
            _userManager = userManager;
        }

        public AuthResultModel Login(LoginModel loginModel)
        {
            try
            {
                var userValidated = _userManager.CheckLogin(loginModel);
                if (userValidated is null) return null;

                var token = GenerateToken(userValidated);
                if (token == null) return null;

                return new AuthResultModel()
                {
                    user = userValidated,
                    tokens = token,
                    expires = token.expired.AddHours(7), //GMT +7
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        TokenModel GenerateToken(UserModel user, string freshTokenOld = "")
        {
            if (!string.IsNullOrEmpty(freshTokenOld))
            {
                //không tồn tại trong REDIS
                if (!_tokenManager.IsExitsRefreshToken(user.Username, freshTokenOld)) return null;
            }

            var accesstoken = GenerateAccessToken(user);
            var freshtoken = GenerateRefreshToken(user);

            var token = new TokenModel()
            {
                accesstoken = WriteToken(accesstoken),
                freshtoken = WriteToken(freshtoken),
                expired = accesstoken.ValidTo,
                //ipAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
            };
            var kq = _tokenManager.AddAndCheckToken(user, token, freshTokenOld);
            return kq ? token : null;
        }

        private JwtSecurityToken GenerateAccessToken(UserModel user)
        {
            var jwtSettings = new JwtSettings();
            _config.Bind(nameof(JwtSettings), jwtSettings);

            var sercurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecret));
            var credentials = new SigningCredentials(sercurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.GivenName),
                new Claim(ClaimTypes.Surname,user.Surname),
            };
            user.GetRoles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });


            var tokenOb = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return tokenOb;

        }
        private JwtSecurityToken GenerateRefreshToken(UserModel user)
        {
            var jwtSettings = new JwtSettings();
            _config.Bind(nameof(JwtSettings), jwtSettings);

            var sercurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.RefreshTokenSecret));
            var credentials = new SigningCredentials(sercurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.GivenName),
                new Claim(ClaimTypes.Surname,user.Surname),
            };
            user.GetRoles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var tokenOb = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(jwtSettings.RefreshTokenExpirationDays),
                signingCredentials: credentials
                );
            return tokenOb;
        }

        string WriteToken(JwtSecurityToken tokenOb)
        {
            return new JwtSecurityTokenHandler().WriteToken(tokenOb);
        }

        internal AuthResultModel RefreshToken(TokenParam token)
        {
            if (token == null || string.IsNullOrEmpty(token.accesstoken) || string.IsNullOrEmpty(token.freshtoken))
                return null;

            var jwtSettings = new JwtSettings();
            _config.Bind(nameof(JwtSettings), jwtSettings);
            var tokenHandler = new JwtSecurityTokenHandler();

            //kiểm tra ACCESS
            try
            {
                var accesssValidateionParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true, // check thời gian hết hạn
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecret)),
                    ValidateIssuerSigningKey = true,

                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ClockSkew = TimeSpan.Zero
                };
                tokenHandler.ValidateToken(token.accesstoken, accesssValidateionParameters, out var accvalidatedToken);

                if (accvalidatedToken != null)
                {
                    //access còn hạn mà đi refresh
                    var payload = tokenHandler.ReadJwtToken(token.accesstoken)?.Payload;

                    var userID = payload.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value;
                    var user = _userManager.GetById(userID).GetInfo();

                    var result = new AuthResultModel()
                    {
                        user = user,
                        tokens = new TokenModel()
                        {
                            accesstoken = token.accesstoken,
                            freshtoken = token.freshtoken,
                        },
                        expires = accvalidatedToken.ValidTo.AddHours(7), //GMT +7
                    };
                    return result;
                }
            }
            catch
            {
            }

            //kiểm tra FRESH
            try
            {
                var refreshValidateionParameters = new TokenValidationParameters
                {
                    ValidateLifetime = false, // không check thời gian hết hạn

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.RefreshTokenSecret)),
                    ValidateIssuerSigningKey = true,

                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidateIssuer = true,

                    ClockSkew = TimeSpan.Zero
                };
                tokenHandler.ValidateToken(token.freshtoken, refreshValidateionParameters, out var validatedToken);
                if (validatedToken == null) return null;

                var jwtToken = (JwtSecurityToken)validatedToken;
                var payload = tokenHandler.ReadJwtToken(token.freshtoken)?.Payload;

                var userID = payload.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value;
                var user = _userManager.GetById(userID).GetInfo();


                var tokenNew = GenerateToken(user, token.freshtoken);
                if (tokenNew == null) return null;

                var result = new AuthResultModel()
                {
                    user = user,
                    tokens = tokenNew,
                    expires = tokenNew.expired.AddHours(7), //GMT +7
                };
                return result;
            }
            catch
            {

                return null;
            }
        }

        internal bool Logout(string userName)
        {
            return _tokenManager.Delete(userName);
        }
    }


}
