using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsTask.Mvc.Data;
using NewsTask.Mvc.Interfaces;
using NewsTask.Mvc.Models;

namespace NewsTask.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly NewsTaskMvcContext _context;
        private readonly HttpClient _client;
        private readonly IAPIManager<AuthorViewModel> _aPIManager;
        private string _controllerName;

        public AuthorController(NewsTaskMvcContext context, HttpClient client,
            IAPIManager<AuthorViewModel> aPIManager, IConfiguration configuration)
        {
            _context = context;
            _client = client;
            _aPIManager = aPIManager;
            _controllerName = configuration.GetSection("apis:authorsControllerUrl").Value;
        }

        // GET: AuthorViewModels
        public IActionResult Index()
        {
            var list = _aPIManager.GetList(_controllerName);

            return View(list);
        }

        // GET: AuthorViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _aPIManager.GetById(id ?? 0, _controllerName);

            return View(author);
        }

        // GET: AuthorViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AuthorViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                _aPIManager.CreateEntity(authorViewModel, _controllerName);
                return RedirectToAction(nameof(Index));
            }
            return View(authorViewModel);
        }

        // GET: AuthorViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorViewModel = _aPIManager.GetById(id ?? 0, _controllerName);
            if (authorViewModel == null)
            {
                return NotFound();
            }

            return View(authorViewModel);
        }

        // POST: AuthorViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Name,Id")] AuthorViewModel authorViewModel)
        {
            if (id != authorViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _aPIManager.UpdateEntity(authorViewModel, _controllerName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(authorViewModel);
        }

        // GET: AuthorViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorViewModel = _aPIManager.GetById(id ?? 0, _controllerName);
            if (authorViewModel == null)
            {
                return NotFound();
            }

            return View(authorViewModel);
        }

        // POST: AuthorViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _aPIManager.DeleteById(id ?? 0, _controllerName);

            return RedirectToAction(nameof(Index));
        }
    }
}
