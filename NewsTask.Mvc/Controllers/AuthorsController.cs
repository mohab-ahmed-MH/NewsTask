using Microsoft.AspNetCore.Mvc;
using NewsTask.Mvc.Models;
using Newtonsoft.Json;

namespace NewsTask.Mvc.Controllers
{
    public class AuthorsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44337/api/News");
        private readonly HttpClient _client;

        public AuthorsController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<AuthorViewModel> AuthList = new List<AuthorViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/GetAll").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                AuthList = JsonConvert.DeserializeObject<List<AuthorViewModel>>(data);
            }
            return View(AuthList);
        }
    }
}
