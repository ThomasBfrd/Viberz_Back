using MediatR;
using Viberz.Application.DTO.Xp;
using Viberz.Application.Models;
using Viberz.Domain.Entities;

public interface IXpHistoryRepository
{
    Task<XpHistory> AddHistoryGame(string userId, AddXpHistoryGame historyGame, UserXpDTO user, int newTotal);
}
