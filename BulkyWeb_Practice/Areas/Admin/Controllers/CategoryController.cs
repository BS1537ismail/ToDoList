using ToDoList.DataAccess.Data;
using ToDoList.DataAccess.Repository.IRepository;
using ToDoList.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using ToDoList.Utility;

namespace ToDoListWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork dbcontext;

        public CategoryController(IUnitOfWork dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Index(string search, int? page, string sortOrder)
        {
            ViewData["CategoryNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DisplayOrderSortParm"] = sortOrder == "DisplayOrder" ? "displayOrder_desc" : "DisplayOrder";

            var categories = dbcontext.CategoryRepository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                categories = categories.Where(x => x.Name.Contains(search));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(s => s.Name);
                    break;
                case "DisplayOrder":
                    categories = categories.OrderBy(s => s.DisplayOrder);
                    break;
                case "displayOrder_desc":
                    categories = categories.OrderByDescending(s => s.DisplayOrder);
                    break;
                default:
                    categories = categories.OrderBy(s => s.Name);
                    break;
            }
            var pagedCategories = categories.ToPagedList(page ?? 1, 3);
            return View(pagedCategories);
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
