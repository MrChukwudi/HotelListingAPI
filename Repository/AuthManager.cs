﻿using AutoMapper;
using HotelListingAPI.Contracts;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace HotelListingAPI.Repository
{
    public class AuthManager : IAuthManager 
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager; //UserManager does the Job of Creating Users for our Users:
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        private const string _loginProvider = "HotelListingAPI";
        private const string _refreshToken = "RefreshToken";


        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }




        public async Task<string> CreateRefreshToken()
        {
            //Go to the Identity Database and Delete the old Token Generated by the Issuer _loginProvider a_refreshTokenh this particular _user :
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);

            //Generate a New Token: for the same user, with the same Issuer and tokenName
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);

            //Set the Token in the Database:
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);

            return newRefreshToken;
        }


        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            //Generate a JwtTokenHandler Object
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            //Get the Token from the RequestDto Object:
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);

            //Pull the UserName out of the Claims in the Token in the Request: But because the Claims are an IEnumerable, we ned to convert them to List first.
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;

            //Use the UserName (in Our case, username == Email) to Find our user from the DB
            _user = await _userManager.FindByNameAsync(username);

            if(_user == null || _user.Id != request.UserId)
            {
                return null;
            }

            //Validate the RefreshToken
            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);

            if(isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            //If the RefreshToken is not Valid, Regenerate a Security Stamp to Log out the User
            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }



        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
           
           
               _user = await _userManager.FindByEmailAsync(loginDto.Email); //Search through the UserDirectory and Find this User by the email
                bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password); //Check if the Password Corresponds and returns a boolean
                if (_user == null || isValidUser == false)
                {
                    return null;
                }

               var token = await GenerateToken();

            return new AuthResponseDto
            {
                UserId = _user.Id,
                Token = token,
                RefreshToken = await CreateRefreshToken()

                    //You could configure your AuthResponseDto object to contain any additional details/info you need to dsiplay
                };
          
        }




        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email; // This is because we didn't explicitly ask the User to supply a UserName:

            var result = await _userManager.CreateAsync(_user, userDto.Password); //This will also perform basic hashing operations on this Pasword

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User"); //Here we add the registered User to the Role of "USER"
            }

            return result.Errors; //This will return any errors if there's a failiure, or no erorrs if there's a success.
        }


        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])); //Our Security Key
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // This is the hashing and decoding functionality setup


            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaim = await _userManager.GetClaimsAsync(_user);

            //Define the List of Claims that needs to be contained within your token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email), //Username
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id)
            }.Union(userClaim).Union(roleClaims);


            //Generate a Token that is consistent with our Jwt-TokenValidationParameters in Program.cs
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token); //Returns our Generated token object as a string:
        }
    }
}