using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Administrator.RequestDtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Hubs;
using Microsoft.AspNetCore.SignalR;
using Application.Dtos;

namespace Application.Administrator.Commands
{
    public class SaveUserCommand : RequestUserDto, IRequest<int>
    {
    }

    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, int>
    {
        private readonly IApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IHubContext<NotificationHub> hub;

        public SaveUserCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IHubContext<NotificationHub> hub)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        public async Task<int> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (dbUser == null)
                throw new NotFoundException($"User ({request.Id}) not found in database.");

            var user = mapper.Map<Domain.Entities.User>(request);

            await dbContext.AddEntityToGraph(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            await hub.Clients.All.SendAsync("newUserAdded", mapper.Map<UserDto>(user), cancellationToken);

            return user.Id;
        }
    }
}
