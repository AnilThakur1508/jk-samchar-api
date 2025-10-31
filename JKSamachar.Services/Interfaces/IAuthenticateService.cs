using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKSamachar.DTO;

namespace JKSamachar.Services.Interfaces
{
    public interface IAuthenticateService
    {
        Task<bool> RegisterUser(RegisterDto registerDto);
        Task<LoginResponseDto> LoginUser(LoginDto loginDto);
        Task<List<RoleResponseDto>> GetAllRoleAsync();
    }
}
