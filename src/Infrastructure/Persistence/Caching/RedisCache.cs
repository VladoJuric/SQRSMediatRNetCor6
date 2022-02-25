using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Persistence.Caching
{
    public class RedisCache : IRedisCache
    {
        private readonly IDistributedCache distributedCache;

        public RedisCache(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task<List<UserDto>> GetAllRedisUsers()
        {
            string serializedUserList;
            List<UserDto> userList = new();
            var redisUserList = await distributedCache.GetAsync("userList");

            if (redisUserList != null)
            {
                serializedUserList = Encoding.UTF8.GetString(redisUserList);
                userList = JsonConvert.DeserializeObject<List<UserDto>>(serializedUserList);
            }

            return userList;
        }

        public async Task<UserDto> GetRedisUser(int userId)
        {
            var userList = await GetAllRedisUsers();

            return userList.FirstOrDefault(u => u.Id == userId);
        }

        public async Task<bool> SetRedisUser(UserDto user)
        {
            var userList = await GetAllRedisUsers();

            try
            {
                userList?.Add(user);
                var serializedUserList = JsonConvert.SerializeObject(userList);
                var redisUserList = Encoding.UTF8.GetBytes(serializedUserList);
                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(60));

                await distributedCache.SetAsync("userList", redisUserList, options);

            }
            catch (Exception)
            {
                throw new NotFoundException("Something went wrong with User on Redis.");
            }
            
            return true;
        }
    }
}
