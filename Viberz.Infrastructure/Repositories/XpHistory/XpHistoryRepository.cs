using AutoMapper;
using Viberz.Domain.Entities;
using Viberz.Infrastructure.Data;
using Viberz.Infrastructure.Repositories;

public class XpHistoryRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<XpHistory, int>(context), IXpHistoryRepository
{
    public readonly ApplicationDbContext _context = context;
    public readonly IMapper _mapper = mapper;
}
