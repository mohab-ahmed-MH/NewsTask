using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsTask.Core.Dto;
using NewsTask.Core.Models;
using NewsTask.Core.Repository;
using System.IO;

namespace NewsTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsServices _newsservices;
        private readonly IAuthorServices _authServices;
        public NewsController(INewsServices newsservices, IAuthorServices authServices)
        {
            _newsservices = newsservices;
            _authServices = authServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var news = await _newsservices.GetById(id);
            if (news == null)
                return NotFound();

            return Ok(news);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _newsservices.GetAll());
        }
        
        [HttpGet("GetByAuthorId")]
        public async Task<IActionResult> GetByAuthorId(int authorId)
        {
            return Ok(await _newsservices.GetAll(authorId));
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromForm] NewsDto newsDto)
        {
            
            var isValidAuthor = await _authServices.IsValidAuthor(newsDto.AuthorId);
            if (!isValidAuthor)
                return BadRequest("Invalid Author Id");

            using var datastream = new MemoryStream();
            await newsDto.Image.CopyToAsync(datastream);

            var news = new News
            {
                Title = newsDto.Title,
                AuthorId = newsDto.AuthorId,
                NewsDescription = newsDto.NewsDescription,
                PublicationDate = newsDto.PublicationDate,
                Image = datastream.ToArray(),
            };

            await _newsservices.Create(news);
            return Ok(news);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id,[FromForm] NewsDto newsDto)
        {
            var news  = await _newsservices.GetById(id);
            if (news == null)
                return NotFound();

            var isValidAuthor = await _authServices.IsValidAuthor(newsDto.AuthorId);
            if (!isValidAuthor)
                return BadRequest("Invalid Author Id");

            using var datastream = new MemoryStream();
            newsDto.Image.CopyToAsync(datastream);

            var newsUpdate = new News
            {
                Title = newsDto.Title,
                AuthorId = newsDto.AuthorId,
                NewsDescription = newsDto.NewsDescription,
                PublicationDate = newsDto.PublicationDate,
                Image = datastream.ToArray(),
            };

            _newsservices.Update(newsUpdate);
            return Ok(news);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _newsservices.GetById(id);
            if (news == null)
                return NotFound();

            var result =  _newsservices.Delete(news);
            return Ok(result);
        }
    }
}