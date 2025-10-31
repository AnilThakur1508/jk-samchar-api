using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JKSamachar.DAL.Common;
using JKSamachar.DAL.Enitity;
using JKSamachar.DAL.Repository.IRepository;
using JKSamachar.DTO;
using JKSamachar.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace JKSamachar.Services.Implementation
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppSetting AppSettings;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticateService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, AppSetting appSetting, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            AppSettings = appSetting;
            _roleManager = roleManager;
        }

        public async Task<List<RoleResponseDto>> GetAllRoleAsync()
        {
            try
            {
                var roles = await Task.FromResult(_roleManager.Roles.ToList());

                if (roles.Any())
                {
                    var userRoles = roles.Select(r => new RoleResponseDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).ToList();

                    return userRoles;
                }

                throw new Exception("No roles found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<LoginResponseDto> LoginUser(LoginDto loginDto)
        {
            try
            {
                if(loginDto != null)
                {
                    var userDetails = await _userManager.FindByEmailAsync(loginDto.Email);
                    if (userDetails != null)
                    {
                        var isValidUser = _userManager.PasswordHasher.VerifyHashedPassword(userDetails, userDetails.PasswordHash, loginDto.Password);
                        if (isValidUser == PasswordVerificationResult.Success)
                        {
                            var roles = await _userManager.GetRolesAsync(userDetails);
                            var roleName = roles.FirstOrDefault();
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var secret = Encoding.ASCII.GetBytes(AppSettings.Secret);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                                {
                                    new Claim(ClaimTypes.Name, userDetails.FirstName),
                                    new Claim(ClaimTypes.Surname, userDetails.LastName),
                                    new Claim(ClaimTypes.NameIdentifier, userDetails.UserName),
                                    new Claim(ClaimTypes.Role, roleName),
                        

                    }),
                                Expires = DateTime.UtcNow.AddHours(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature),
                                Issuer = AppSettings.Issuer,
                                Audience = AppSettings.Audience

                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            var userToken = tokenHandler.WriteToken(token);
                            return new LoginResponseDto
                            {
                                UserId = userDetails.Id,
                                Email = userDetails.Email,
                                Token = userToken,
                                FirstName = userDetails.FirstName,
                                LastName = userDetails.LastName,
                                UserName = userDetails.UserName,
                            };
                        }
                        throw new Exception("Password is Incorrect");
                    }
                    throw new Exception("User is not found by this email id");
                }
                throw new Exception("Login details is null");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RegisterUser(RegisterDto registerDto)
        {
            try
            {
                var userexisting = _userManager.FindByEmailAsync(registerDto.Email);
                if (userexisting.Result != null)
                {
                    throw new Exception("User already exists with this email id");
                }
                if (registerDto != null)
                {
                    var user = new AppUser
                    {
                        FirstName = registerDto.FirstName,
                        LastName = registerDto.LastName,
                        Email = registerDto.Email,
                        UserName = registerDto.Email,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                    };
                    var result = await _userManager.CreateAsync(user, registerDto.Password);
                    if (result.Succeeded)
                    {
                        var role = await _roleManager.FindByIdAsync(registerDto.Role);

                        if (registerDto.Role != null && role.Name == "Admin")
                        {
                            if (registerDto.AdminVerfiyCode == "600699446677@JK")
                            {

                            }
                            else {
                                _userManager.DeleteAsync(user);
                                throw new Exception("Admin verify code is incorrect");
                            }
                        }
                        if (role != null)
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                        }

                        return true;
                    }
                    else
                    {
                        throw new Exception("User registration failed");
                    }

                }
                throw new Exception("User data is null");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
