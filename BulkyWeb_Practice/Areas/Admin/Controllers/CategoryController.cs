using ToDoList.DataAccess.Data;
using ToDoList.DataAccess.Repository.IRepository;
using ToDoList.Models;
using Microsoft.AspNetCore.Mvc;

namespace ToDoListWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork dbcontext;

        public CategoryController(IUnitOfWork dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Index(string search)
        {
            List<Category> categories = dbcontext.CategoryRepository.GetAll().ToList();
            if (!string.IsNullOrEmpty(search))
            {
                categories = categories.Where(x => x.Name.Contains(search)).ToList();
            }
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                dbcontext.CategoryRepository.Add(category);
                dbcontext.Save();
                TempData["success"] = "Category Created SuccessFully";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Category category = dbcontext.CategoryRepository.Get(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                dbcontext.CategoryRepository.Update(category);
                dbcontext.Save();
                TempData["success"] = "Category Updated SuccessFully";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Category category = dbcontext.CategoryRepository.Get(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            if (category == null)
            {
                return NotFound();
            }

            dbcontext.CategoryRepository.Remove(category);
            dbcontext.Save();
            TempData["success"] = "Category Deleted SuccessFully";
            return RedirectToAction("Index");

            return View();
        }

        public IActionResult Details(int id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Category category = dbcontext.CategoryRepository.Get(x => x.Id == id);
            return View(category);
        }

    }
}
