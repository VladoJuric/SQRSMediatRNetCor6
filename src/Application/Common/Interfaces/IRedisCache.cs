using Application.Dtos;

namespace Application.Common.Interfaces
{
    public interface IRedisCache
    {
        Task<List<UserDto>> GetAllRedisUsers();
        Task<UserDto> GetRedisUser(int userId);
        Task<bool> SetRedisUser(UserDto user);
    }
}
