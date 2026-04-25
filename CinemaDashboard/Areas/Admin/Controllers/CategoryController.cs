namespace CinemaDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult Index() => View(_context.Categories.AsEnumerable());

        [HttpGet]
        public IActionResult Create() => View(new Category());

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid) { TempData["Error-Notification"] = "Invalid Data"; return View(category); }
            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["Success-Notification"] = "Category Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return RedirectToAction("NotFoundPage", "Home");
            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            _context.Categories.Update(category);
            _context.SaveChanges();
            TempData["Success-Notification"] = "Category Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return RedirectToAction("NotFoundPage", "Home");
            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["Success-Notification"] = "Category Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
