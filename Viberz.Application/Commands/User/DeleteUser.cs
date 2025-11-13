using MediatR;

namespace Viberz.Application.Commands.User;

public class DeleteUser : IRequest<bool>
{
    public string userId { get; }

    public DeleteUser(string userId)
    {
        this.userId = userId;
    }

    public class Handler : IRequestHandler<DeleteUser, bool>
    {
        private readonly IUserRepository _userRepository;

        public Handler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            return await _userRepository.DeleteUser(request.userId);
        }
    }
}
