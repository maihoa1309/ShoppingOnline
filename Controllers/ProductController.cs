using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingOnline.Data;
using ShoppingOnline.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ShoppingOnline.Controllers
{
    public class ProductController : Controller
    {
    
        private DbConnection Db;
        public ProductController(DbConnection db)
        {
            Db = db;
        }

        public ActionResult Index(string q, string sortOrder)
        {
            List<Product> products = Db.Product.Include(c => c.CategoryEntity).ToList();
            // search
            HashSet<Product> newPro = new HashSet<Product>();
            if (!string.IsNullOrEmpty(q))
            {
                products.Clear();
                string[] aSearch = q.Split(' ');
                if (aSearch.Length > 0)
                {
                    foreach (string key in aSearch)
                    {
                        Db.Product.ToList().Where(pro => pro.Name.Contains(q))
                            .ToList()
                            .ForEach(p => newPro.Add(p));
                    }
                }
                ViewData["keyword"] = q;
                foreach (Product pro in newPro)
                {
                    products.Add(pro);
                }
            }
            //sort 
            ViewData["SortByName"] = (string.IsNullOrEmpty(sortOrder)) ? "SortByNameASC" : "";
            ViewData["SortByQuantity"] = (string.IsNullOrEmpty(sortOrder)) ? "SortByQuantity" : "";
            switch (sortOrder)
            {
                case "SortByNameASC": products = products.OrderBy(p => p.Name).ToList(); break;
                case "SortByQuantity": products = products.OrderBy(p => p.Quantity).ToList(); break;
                default: products = products.OrderByDescending(p => p.Name).ToList();break;
            }


            return View(products);
        }

        public ActionResult Add()
        {
            ViewData["CategoryId"] = new SelectList(Db.Category, "Id", "Name");
            return View("Add");
        }

        [HttpPost]
        public ActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                Db.Add(product);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(Db.Category, "Id", "Name", Db.Category);
            return View(nameof(Add));
        }

        public ActionResult Delete(int id)
        {
            Product pro = Db.Product.Find(id);
            if (pro != null)
            {
                Db.Product.Remove(pro);
                Db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit( int id)
        {
            ViewData["CategoryId"] = new SelectList(Db.Category, "Id", "Name");
            Product pro = Db.Product.Find(id);
            return View(pro);
        }
        [HttpPost]
        public ActionResult Edit(Product pro)
        {
            if (ModelState.IsValid)
            {
                Db.Product.Update(pro);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit));
        }
    }
}
