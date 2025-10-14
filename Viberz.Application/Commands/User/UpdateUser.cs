using AutoMapper;
using MediatR;
using Viberz.Application.DTO.User;
using Viberz.Application.Utilities;
using Viberz.Domain.Entities;

public class UpdateUser : IRequest<User>
{
    public UserUpdateDTO UserToUpdate { get; }
    public string UserId { get; }

    public UpdateUser(UserUpdateDTO userToUpdate, string userId)
    {
        UserToUpdate = userToUpdate;
        UserId = userId;
    }

    public class Handler : IRequestHandler<UpdateUser, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ConvertImageToBase64 _imageToBase64;

        public Handler(
            IUserRepository userRepository,
            IMapper mapper,
            ConvertImageToBase64 imageToBase64)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _imageToBase64 = imageToBase64;
        }

        public async Task<User> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUser(request.UserId)
                ?? throw new Exception("User not found in database");

            bool userNameExists = await _userRepository.CheckUserName(request.UserToUpdate.Username!);

            User updatedUser = _mapper.Map(request.UserToUpdate, user);
            updatedUser.UpdatedAt = DateTime.UtcNow;

            return await _userRepository.UpdateUser(updatedUser);
        }
    }
}
