using AutoMapper;
using AutoMapper.QueryableExtensions;
using LearnGenres.DTO;
using LearnGenres.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnGenres.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GenreController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<UserDTO>> getGenres()
        {
            var queryable = _context.Users.AsQueryable();
            return await _context.Users
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        }

        [HttpPost]
        public async Task<ActionResult<UserCreationDTO>> CreateUser([FromBody] UserCreationDTO userCreation)
        {
            User user = _mapper.Map<User>(userCreation);
            _context.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<UserCreationDTO>> UpdateUser(Guid id, [FromBody] UserCreationDTO userCreation)
        {
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(userCreation, user);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(Guid Id)
        {
            User user = await _context.Users.FindAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
