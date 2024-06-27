using ToDoList.DataAccess.Data;
using ToDoList.DataAccess.Repository.IRepository;
using ToDoList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoListWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork dbcontext;

        public ProductController(IUnitOfWork dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Index(string search)
        {
            List<Product> categories = dbcontext.ProductRepository.GetAll().ToList();
           
            if (!string.IsNullOrEmpty(search))
            {
                categories = categories.Where(x => x.Title.Contains(search)).ToList();
            }
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = dbcontext.CategoryRepository
               .GetAll().Select(x => new SelectListItem
               {
                   Text = x.Name,
                   Value = x.Id.ToString()
               });
            ViewBag.CategoryList = CategoryList;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product category)
        {
            if (ModelState.IsValid)
            {
                dbcontext.ProductRepository.Add(category);
                dbcontext.Save();
                TempData["success"] = "Product Created SuccessFully";
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
            Product category = dbcontext.ProductRepository.Get(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Product category)
        {
            if (ModelState.IsValid)
            {
                dbcontext.ProductRepository.Update(category);
                dbcontext.Save();
                TempData["success"] = "Product Updated SuccessFully";
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
            Product category = dbcontext.ProductRepository.Get(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Product category)
        {
            if (category == null)
            {
                return NotFound();
            }

            dbcontext.ProductRepository.Remove(category);
            dbcontext.Save();
            TempData["success"] = "Product Deleted SuccessFully";
            return RedirectToAction("Index");

            return View();
        }

        public IActionResult Details(int id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Product category = dbcontext.ProductRepository.Get(x => x.Id == id);
            return View(category);
        }

    }
}
