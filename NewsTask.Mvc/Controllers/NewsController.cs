using Microsoft.AspNetCore.Mvc;
using NewsTask.Mvc.Models;
using Newtonsoft.Json;

namespace NewsTask.Mvc.Controllers
{
    public class NewsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44337/api/News");
        private readonly HttpClient _client;
        public NewsController(HttpClient client)
        {
            _client= client;
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<NewsViewModel> newsList = new List<NewsViewModel>();
            HttpResponseMessage response= _client.GetAsync(_client.BaseAddress + "/GetAll").Result;

            if(response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                newsList = JsonConvert.DeserializeObject<List<NewsViewModel>>(data);
            }
            return View(newsList);
        }
    }
}
