using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsTask.Core.Dto;
using NewsTask.Core.Models;
using NewsTask.Core.Repository;
using System.Data;
using System.IO;

namespace NewsTask.Api.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Create([FromBody] NewsDto newsDto)
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
        public async Task<IActionResult> Update([FromBody] NewsDto newsDto)
        {
            var news = await _newsservices.GetById(newsDto.Id ?? 0);
            if (news == null)
                return NotFound();

            var isValidAuthor = await _authServices.IsValidAuthor(newsDto.AuthorId);
            if (!isValidAuthor)
                return BadRequest("Invalid Author Id");

            using var datastream = new MemoryStream();
            newsDto.Image.CopyToAsync(datastream);


            news.Title = newsDto.Title;
            news.AuthorId = newsDto.AuthorId;
            news.NewsDescription = newsDto.NewsDescription;
            news.PublicationDate = newsDto.PublicationDate;
            news.Image = datastream.ToArray();

            _newsservices.Update(news);
            return Ok(news);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _newsservices.GetById(id);
            if (news == null)
                return NotFound();

            var result = _newsservices.Delete(news);
            return Ok(result);
        }

        [HttpPut("Publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var news = await _newsservices.GetById(id);
            if (news == null)
                return NotFound();

            if (news.IsPublish)
                return BadRequest("This news is already published");

            if (news.PublicationDate.Date < DateTime.Now.Date)
                return BadRequest($"This News Can't be published before {news.PublicationDate.Date}");

            news.IsPublish = true;
            _newsservices.Update(news);

            return Ok();
        }
    }
}