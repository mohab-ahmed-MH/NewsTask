using NewsTask.Mvc.Interfaces;
using NewsTask.Mvc.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Text;

namespace NewsTask.Mvc.Managers
{
    public class APIManager<T> : IAPIManager<T> where T : class
    {
        private readonly HttpClient _client;
        private IConfiguration _configuration;
        private string _baseURL;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public APIManager(HttpClient client, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _configuration = configuration;
            _baseURL = configuration.GetSection("apis:baseAPI").Value;
            _httpContextAccessor = httpContextAccessor;
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWT_Token");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

        }

        public void CreateEntity(T entity, string controllerName)
        {

            try
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync($"{_baseURL}{controllerName}/", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    if (data is not null)
                        entity = JsonConvert.DeserializeObject<T>(data);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void DeleteById(int id, string controllerName)
        {
            try
            {
                T entity = null;
                var builder = new UriBuilder($"{_baseURL}{controllerName}/");
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["id"] = id.ToString();
                builder.Query = query.ToString();
                string url = builder.ToString();
                HttpResponseMessage response = _client.DeleteAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    if (data is not null)
                        entity = JsonConvert.DeserializeObject<T>(data);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public T GetById(int id, string controllerName)
        {
            try
            {
                T entity = null;
                var builder = new UriBuilder($"{_baseURL}{controllerName}/{id}");
                //builder.Port = -1;
                //var query = HttpUtility.ParseQueryString(builder.Query);
                //query["id"] = id.ToString();
                //builder.Query = query.ToString();
                string url = builder.ToString();
                HttpResponseMessage response = _client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    if (data is not null)
                        entity = JsonConvert.DeserializeObject<T>(data);
                }

                return entity;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<T> GetList(string controllerName)
        {
            try
            {
                var entityList = new List<T>();
                HttpResponseMessage response = _client.GetAsync($"{_baseURL}{controllerName}/getAll").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    entityList = JsonConvert.DeserializeObject<List<T>>(data);
                }
                else
                    throw new Exception(response.ReasonPhrase);

                return entityList;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void UpdateEntity(T entity, string controllerName)
        {
            try
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync($"{_baseURL}{controllerName}/", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    if (data is not null)
                        entity = JsonConvert.DeserializeObject<T>(data);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
