namespace CinemaDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult Index() => View(_context.Actors.AsEnumerable());

        [HttpGet]
        public IActionResult Create() => View(new Actor());

        [HttpPost]
        public IActionResult Create(Actor item, IFormFile? ImgFile)
        {
            SaveImage(ImgFile, x => item.Img = x);
            _context.Actors.Add(item);
            _context.SaveChanges();
            TempData["Success-Notification"] = "Actor Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var item = _context.Actors.FirstOrDefault(x => x.Id == id);
            if (item == null) return RedirectToAction("NotFoundPage", "Home");
            return View(item);
        }

        [HttpPost]
        public IActionResult Update(Actor item, IFormFile? ImgFile)
        {
            var old = _context.Actors.AsNoTracking().FirstOrDefault(x => x.Id == item.Id);
            if (old == null) return RedirectToAction("NotFoundPage", "Home");
            if (ImgFile != null && ImgFile.Length > 0)
            {
                SaveImage(ImgFile, x => item.Img = x);
                DeleteImage(old.Img);
            }
            else item.Img = old.Img;

            _context.Actors.Update(item);
            _context.SaveChanges();
            TempData["Success-Notification"] = "Actor Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var item = _context.Actors.FirstOrDefault(x => x.Id == id);
            if (item == null) return RedirectToAction("NotFoundPage", "Home");
            DeleteImage(item.Img);
            _context.Actors.Remove(item);
            _context.SaveChanges();
            TempData["Success-Notification"] = "Actor Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        void SaveImage(IFormFile? file, Action<string> set)
        {
            if (file == null || file.Length == 0) return;
            var fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "actorImages");
            Directory.CreateDirectory(dir);
            using var stream = System.IO.File.Create(Path.Combine(dir, fileName));
            file.CopyTo(stream);
            set(fileName);
        }

        void DeleteImage(string img)
        {
            if (img == "default.png") return;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "actorImages", img);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }
    }
}
