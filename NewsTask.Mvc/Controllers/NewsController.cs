using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsTask.Mvc.Data;
using NewsTask.Mvc.Interfaces;
using NewsTask.Mvc.Models;

namespace NewsTask.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly NewsTaskMvcContext _context;
        private readonly HttpClient _client;
        private readonly IAPIManager<NewsViewModel> _aPIManager;
        private readonly IAPIManager<AuthorViewModel> _authorAPIManager;
        private string _controllerName;
        private string _authorControllerName;

        public NewsController(NewsTaskMvcContext context, HttpClient client,
            IAPIManager<NewsViewModel> aPIManager, IConfiguration configuration,
            IAPIManager<AuthorViewModel> authorAPIManager)
        {
            _context = context;
            _client = client;
            _aPIManager = aPIManager;
            _authorAPIManager = authorAPIManager;
            _controllerName = configuration.GetSection("apis:newsControllerUrl").Value;
            _authorControllerName = configuration.GetSection("apis:authorsControllerUrl").Value;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var list = _aPIManager.GetList(_controllerName);

            return View(list);
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _aPIManager.GetById(id ?? 0, _controllerName);

            return View(author);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            ViewBag.Authors = _authorAPIManager.GetList(_authorControllerName);
            var news = new NewsViewModel();
            news.PublicationDate = DateTime.Now;
            return View(news);
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsViewModel newsViewModel)
        {
            if (ModelState.IsValid)
            {
                if (newsViewModel.ImageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        newsViewModel.ImageFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        //string base64 = Convert.ToBase64String(fileBytes);
                        if (fileBytes is not null)
                            newsViewModel.Image = fileBytes;
                    }
                }
                newsViewModel.ImageFile = null;
                _aPIManager.CreateEntity(newsViewModel, _controllerName);
                return RedirectToAction(nameof(Index));
            }

            return View(newsViewModel);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NewsViewModel == null)
            {
                return NotFound();
            }

            var newsViewModel = await _context.NewsViewModel.FindAsync(id);
            if (newsViewModel == null)
            {
                return NotFound();
            }

            ViewBag.Authors = _authorAPIManager.GetList(_authorControllerName);

            return View(newsViewModel);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,NewsDescription,PublicationDate,Image,AuthorId")] NewsViewModel newsViewModel)
        {
            if (id != newsViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _aPIManager.UpdateEntity(newsViewModel, _controllerName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(newsViewModel);
        }

        // GET: News/Delete/5
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

        // POST: News/Delete/5
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
