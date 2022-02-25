using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Administrator.Queries
{
    public class GetUserDtoByIdQuery : IRequest<UserDto>
    {
        public int UserId { get; set; }
    }

    public class GetUserDtoByIdQueryHandler : IRequestHandler<GetUserDtoByIdQuery, UserDto>
    {
        private readonly IApplicationDbContext dbContext;
        private readonly IRedisCache redisCache;
        private readonly IMapper mapper;

        public GetUserDtoByIdQueryHandler(IApplicationDbContext dbContext,
            IRedisCache redisCache,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDto> Handle(GetUserDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var redisUser = await redisCache.GetRedisUser(request.UserId);

            if (redisUser != null)
                return redisUser;

            var userDb = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (userDb == null)
                throw new NotFoundException($"User ({request.UserId}) not found in database");

            var userDto = mapper.Map<UserDto>(userDb);

            await redisCache.SetRedisUser(userDto);

            return userDto;
        }
    }
}
