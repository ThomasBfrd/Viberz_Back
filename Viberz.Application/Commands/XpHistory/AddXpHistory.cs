using AutoMapper;
using MediatR;
using Viberz.Application.DTO.Xp;
using Viberz.Application.Interfaces.XpHistory;
using Viberz.Domain.Models;

public class AddXpHistory : IRequest<UserXpDTO>
{
    public string UserId { get; }
    public AddXpHistoryGame XpHistory { get; }
    public AddXpHistory(string userId, AddXpHistoryGame xpHistory)
    {
        UserId = userId;
        XpHistory = xpHistory;
    }

    public class Handler : IRequestHandler<AddXpHistory, UserXpDTO>
    {
        private readonly IXpHistoryService _xpHistoryService;
        private readonly IXpHistoryRepository _xpHistoryRepository;
        private readonly IUserXp _userXp;
        private readonly IMapper _mapper;

        public Handler(IXpHistoryService xpHistoryService, IXpHistoryRepository xpHistoryRepository, IUserXp userXp, IMapper mapper)
        {
            _xpHistoryService = xpHistoryService;
            _xpHistoryRepository = xpHistoryRepository;
            _userXp = userXp;
            _mapper = mapper;
        }
        public async Task<UserXpDTO> Handle(AddXpHistory request, CancellationToken cancellationToken)
        {
            bool isLevelUp = false;

            UserXpInfo beforeUser = await _userXp.GetUserXp(request.UserId);

            int newTotal = beforeUser.CurrentXp += request.XpHistory.EarnedXp;

            if ((beforeUser.XpForNextLevel - newTotal) <= 0)
            {
                isLevelUp = true;
            }

            beforeUser.LevelUp = isLevelUp;

            await _xpHistoryService.AddXpHistory(request.UserId, request.XpHistory, newTotal, beforeUser);

            UserXpInfo user = await _userXp.GetUserXp(request.UserId);
            user.LevelUp = isLevelUp;

            UserXpDTO userDto = _mapper.Map<UserXpDTO>(user);

            return userDto;
        }
    }
}
