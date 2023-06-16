using NewsTask.Mvc.Interfaces;
using NewsTask.Mvc.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NewsTask.Mvc.RequestModels;

namespace NewsTask.Mvc.Managers
{
    public class AuthAPIManager : IAuthAPIManager
    {
        private readonly HttpClient _client;
        private IConfiguration _configuration;
        private string _baseURL;

        public AuthAPIManager(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            _baseURL = _configuration.GetSection("apis:baseAPI").Value;

        }

        public AuthViewModel Login(TokenRequestModel requestModel)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
                var response = _client
                    .PostAsync($"{_baseURL}" +
                    $"{_configuration.GetSection("apis:auth:authControllerUrl").Value}" +
                    $"{_configuration.GetSection("apis:auth:loginUrl").Value}", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    if (data is not null)
                    {

                        var entity = JsonConvert.DeserializeObject<AuthViewModel>(data);

                        return entity;
                    }

                    return null;
                }

                return null;

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}