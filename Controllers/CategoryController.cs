using Microsoft.AspNetCore.Mvc;
using ShoppingOnline.Data;
using ShoppingOnline.Models.Entity;
using System.Xml.Linq;
using X.PagedList;

namespace ShoppingOnline.Controllers
{
    public class CategoryController : Controller
    {
        private DbConnection Db;
        public CategoryController(DbConnection Db) 
        {
            this.Db = Db;
        }
        public IActionResult Index(string q, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 2;
            List<Category> newCat = Db.Category.ToList();
            HashSet<Category> categories= new HashSet<Category>();
            if (!string.IsNullOrEmpty(q))
            {
                string[] aSearch = q.Split(' ');
                if (aSearch.Length > 0)
                {
                    foreach(string key in aSearch)
                    {
                        Db.Category.ToList().Where(cat => cat.Name.Contains(q))
                            .ToList()
                            .ForEach(c => categories.Add(c));
                    }
                }
                ViewData["keyword"] = q;
                foreach (Category cat in categories)
                {
                    newCat.Add(cat);
                }
            }
            
            return View(newCat.ToPagedList(pageNumber,pageSize));
        }
        
        public ActionResult Add() 
        {
            return View("Add");
        }

        [HttpPost]
        public ActionResult Add(Category category) 
        {
            if(ModelState.IsValid)
            {
                Db.Category.Add(category);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View("Add");
        }

        public ActionResult Delete(int id)
        {
            Category cat = Db.Category.Find(id);
            if(cat != null)
            {
                Db.Category.Remove(cat);
                Db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }


}
