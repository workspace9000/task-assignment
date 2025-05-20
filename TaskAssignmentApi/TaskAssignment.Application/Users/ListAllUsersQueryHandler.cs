using MediatR;

namespace TaskAssignment.Application.Users
{
    public class ListAllUsersQueryHandler : IRequestHandler<ListAllUsersQuery, List<ListAllUsersItem>>
    {
        private readonly IUserQueryRepository _userQueryRepository;

        public ListAllUsersQueryHandler(IUserQueryRepository userQueryRepository)
        {
            _userQueryRepository = userQueryRepository;
        }

        public Task<List<ListAllUsersItem>> Handle(ListAllUsersQuery query, CancellationToken cancellationToken)
        {
            return _userQueryRepository.GetAllAsync(cancellationToken);
        }
    }

    public class ListAllUsersQuery : IRequest<List<ListAllUsersItem>>
    {
    }

    public class ListAllUsersItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
