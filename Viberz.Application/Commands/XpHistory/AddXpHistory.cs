using MediatR;
using Viberz.Application.DTO.Xp;
using Viberz.Application.Models;
using Viberz.Domain.Entities;

public class AddXpHistory : IRequest<UserXpDTO>
{
    public string UserId { get; }
    public AddXpHistoryGame XpHistory { get; }
    public AddXpHistory(string userId, AddXpHistoryGame xpHistory)
    {
        UserId = userId;
        XpHistory = xpHistory;
    }

    public class  Handler : IRequestHandler<AddXpHistory, UserXpDTO>
    {
        private readonly IXpHistoryRepository _xpHistoryRepository;
        private readonly IUserXp _userXp;

        public Handler(IXpHistoryRepository xpHistoryRepository, IUserXp userXp)
        {
            _xpHistoryRepository = xpHistoryRepository;
            _userXp = userXp;
        }
        public async Task<UserXpDTO> Handle(AddXpHistory request, CancellationToken cancellationToken)
        {
            bool isLevelUp = false;

            UserXpDTO beforeUser = await _userXp.GetUserXp(request.UserId);

            int newTotal = beforeUser.CurrentXp += request.XpHistory.EarnedXp;

            if ((beforeUser.XpForNextLevel - newTotal) <= 0)
            {
                isLevelUp = true;
            }

            beforeUser.LevelUp = isLevelUp;

            XpHistory history = await _xpHistoryRepository.AddHistoryGame(request.UserId, request.XpHistory, beforeUser, newTotal);

            UserXpDTO user = await _userXp.GetUserXp(request.UserId);
            user.LevelUp = isLevelUp;

            return user;
        }
    }
}
