using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsTask.Core.Dto;
using NewsTask.Core.Models;
using NewsTask.Core.Repository;

namespace NewsTask.Api.Controllers
{
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

            return Ok(author);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _authServices.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorDto authorDto)
        {
            var author = new Author
            {
                Name = authorDto.Name
            };

            await _authServices.Create(author);
            return Ok(author);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id ,AuthorDto authorDto)
        {
            var author = await _authServices.GetById(id);
            if (author == null)
                return NotFound();

            var authorUpdate = new Author
            {
                Name = authorDto.Name
            };

            _authServices.Update(authorUpdate);
            return Ok(author);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authServices.GetById(id);
            if(author == null)
                return NotFound();

            _authServices.Delete(author);
            return Ok(author);

        }

    }
}
