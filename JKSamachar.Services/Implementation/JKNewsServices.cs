using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JKSamachar.DAL.Enitity;
using JKSamachar.DAL.Repository.IRepository;
using JKSamachar.DTO;
using JKSamachar.Services.Interfaces;

namespace JKSamachar.Services.Implementation
{
    public class JKNewsServices : IJKNewsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public JKNewsServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<bool> AddJKNews(JkNewsDto jkNewsDto, string token)
        {
            try
            {
                if (token == null)
                {
                    throw new Exception("You are not login first login then you will can post the news");
                }
                var result = _mapper.Map<JKNews>(jkNewsDto);
                if(jkNewsDto.Role == "Admin")
                {
                    result.IsAdminedAllowPublish = true;
                }
                else
                {
                    result.IsAdminedAllowPublish = false;
                }
                _unitOfWork.Add(result);
                _unitOfWork.Commit();

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add news: {ex.Message}");
            }
        }

        public async Task<bool> DeleteJKNews(string id)
        {
            try
            {
                Guid.TryParse(id, out var newId );
                var result = _unitOfWork.GetById<JKNews>(newId);
                if (result != null)
                {
                    result.IsDeleted = true;
                     _unitOfWork.Update(result);
                    _unitOfWork.Commit();
                    return true;
                }
                throw new Exception("Data not found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<JKNewsResponseDto>> GetAllJKNews(string roleName)
        {
            try
            {
                IEnumerable<JKNews> result;

                if (roleName == "Admin")
                {
                    result = _unitOfWork.Query<JKNews>()
                        .Where(x => !x.IsDeleted)
                        .OrderByDescending(x => x.CreatedOn)
                        .ToList();
                }
                else
                {
                    result = _unitOfWork.Query<JKNews>()
                        .Where(x => !x.IsDeleted && x.IsAdminedAllowPublish == true)
                        .OrderByDescending(x => x.CreatedOn)
                        .ToList();
                }

                if (result != null && result.Any())
                {
                    var jkNewsDto = _mapper.Map<IEnumerable<JKNewsResponseDto>>(result);
                    return jkNewsDto;
                }

                return Enumerable.Empty<JKNewsResponseDto>();
            }
            catch (Exception ex)
            {
                // You can log here if needed
                throw new Exception("Failed to get news", ex);
            }
        }

        public async Task<JKNewsResponseDto> GetJKNewsById(string id)
        {
            try
            {
                Guid.TryParse(id, out var newId);
                var result = _unitOfWork.GetById<JKNews>(newId);
                if (result != null && !result.IsDeleted)
                {
                    var jkNewsDto = _mapper.Map<JKNewsResponseDto>(result);
                    return jkNewsDto;
                }
                throw new Exception("Data not found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Task<bool> UpdateJKNews(string id, JkNewsDto jkNewsDto)
        {
            try
            {
                Guid.TryParse(id, out var newId);
                var existingData = _unitOfWork.GetById<JKNews>(newId);
                if (existingData != null)
                {
                    _mapper.Map(jkNewsDto, existingData);
                    existingData.IsAdminedAllowPublish = true;
                    _unitOfWork.Update(existingData);
                    _unitOfWork.Commit();
                    return Task.FromResult(true);
                }
                throw new Exception("Existing record null here");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
