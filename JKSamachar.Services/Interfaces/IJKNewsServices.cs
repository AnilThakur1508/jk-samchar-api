using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKSamachar.DTO;

namespace JKSamachar.Services.Interfaces
{
    public interface IJKNewsServices
    {
        Task<bool> AddJKNews(JkNewsDto jkNewsDto, string token);
        Task<IEnumerable<JKNewsResponseDto>> GetAllJKNews();
        Task<JKNewsResponseDto> GetJKNewsById(string id);
        Task<bool> UpdateJKNews(string id, JkNewsDto jkNewsDto);
        Task<bool> DeleteJKNews(string id);
    }
}
