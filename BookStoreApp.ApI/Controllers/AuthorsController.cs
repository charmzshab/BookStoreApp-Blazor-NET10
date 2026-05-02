using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.ApI.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using BookStoreApp.ApI.DTos.Author;
using BookStoreApp.ApI.Static;

namespace BookStoreApp.ApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            try { 
                var authors = await _context.Authors.ToListAsync();
                var authorReadOnlyDtos = _mapper.Map<List<AuthorReadOnlyDto>>(authors);
                return Ok(authorReadOnlyDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while processing your request in {nameof(GetAuthors)}");
                return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error500Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            try { 
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"Author with id {id} not found in {nameof(GetAuthor)}");
                    return NotFound();
                }
                var authorReadOnlyDto = _mapper.Map<AuthorReadOnlyDto>(author);
                return Ok(authorReadOnlyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while processing your request in {nameof(GetAuthor)}");
                return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error500Message);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorUpdateDto)
        {
            if (id != authorUpdateDto.Id)
            {
                _logger.LogWarning($"Author id {id} does not match the id in the request body in {nameof(PutAuthor)}");
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                _logger.LogWarning($"Author with id {id} not found in {nameof(PutAuthor)}");
                return NotFound();
            }

            _mapper.Map(authorUpdateDto, author);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"A concurrency error occurred while processing your request in {nameof(PutAuthor)}");
                    return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error500Message);
                }
          
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(AuthorCreateDto authorCreateDto)
        {
            try { 
                var author = _mapper.Map<Author>(authorCreateDto);
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                var authorReadOnlyDto = _mapper.Map<AuthorReadOnlyDto>(author);
                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorReadOnlyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while processing your request in {nameof(PostAuthor)}");
                return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error500Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try { 
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"Author with id {id} not found in {nameof(DeleteAuthor)}");
                    return NotFound();
                }
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while processing your request in {nameof(DeleteAuthor)}");
                return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error500Message);
            }
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}
