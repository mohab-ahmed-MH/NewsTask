using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsTask.Core.Dto;
using NewsTask.Core.Models;
using NewsTask.Core.Repository;

namespace NewsTask.Api.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorServices _authServices;

        public AuthorsController(IAuthorServices authServices)
        {
            _authServices = authServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _authServices.GetById(id);
            if (author == null)
                return NotFound();

            var authorDTO = new AuthorDto(author.Name, author.Id);

            return Ok(authorDTO);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authServices.GetAll();
            if (authors == null)
                return NotFound();

            var authorsDTOs = new List<AuthorDto>();
            authors.ToList().ForEach(author =>
            {
                authorsDTOs.Add(new AuthorDto(author.Name, author.Id));
            });

            return Ok(authorsDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorDto authorDto)
        {
            var author = new Author
            {
                Name = authorDto.Name
            };

            await _authServices.Create(author);
            var authorDTO = new AuthorDto(author.Name, author.Id);

            return Ok(authorDTO);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AuthorDto authorDto)
        {
            if (authorDto.Id is null) return NotFound();

            var author = await _authServices.GetById(authorDto.Id ?? 0);
            if (author == null)
                return NotFound();

            author.Name = authorDto.Name;



            _authServices.Update(author);

            var authorDTO = new AuthorDto(author.Name, author.Id);

            return Ok(authorDTO);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authServices.GetById(id);
            if (author == null)
                return NotFound();

            _authServices.Delete(author);
            return Ok(author);

        }

    }
}
