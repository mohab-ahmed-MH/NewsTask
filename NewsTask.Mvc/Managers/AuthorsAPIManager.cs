using NewsTask.Mvc.Interfaces;
using NewsTask.Mvc.Models;

namespace NewsTask.Mvc.Managers
{
    public class AuthorsAPIManager
    {
        private readonly IAPIManager<AuthorViewModel> _aPIManager;
        private IConfiguration _configuration;

        public AuthorsAPIManager(IAPIManager<AuthorViewModel> aPIManager, IConfiguration configuration)
        {
            _aPIManager = aPIManager;
            _configuration = configuration;
            _aPIManager.ControllerName = _configuration["authorsControllerUrl"];
        }
    }
}
