using Microsoft.AspNetCore.Mvc;
using NewsTask.Mvc.Data;
using NewsTask.Mvc.Interfaces;
using NewsTask.Mvc.Managers;
using NewsTask.Mvc.Models;
using NewsTask.Mvc.RequestModels;
using System.Net.Http.Headers;

namespace NewsTask.Mvc.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _client;
        private readonly IAuthAPIManager _authAPIManager;
        private string _controllerName;
        public LoginController(HttpClient client,
           IAuthAPIManager authAPIManager, IConfiguration configuration)
        {
            _client = client;
            _authAPIManager = authAPIManager;
            _controllerName = configuration.GetSection("apis:newsControllerUrl").Value;
        }

        public IActionResult Login()
        {
            return View(new TokenRequestModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(TokenRequestModel requestModel)
        {
            if (ModelState.IsValid)
            {
                var logInModel = _authAPIManager.Login(requestModel);
                if (logInModel is not null)
                {
                    HttpContext.Session.SetString("JWT_Token", logInModel.Token);

                    return RedirectToAction("Index", "News");
                }
            }

            return RedirectToAction(nameof(Login));

        }
    }
}
