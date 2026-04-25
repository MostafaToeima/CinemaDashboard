namespace CinemaDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult Index(FilterMovieVM filter)
        {
            var movies = _context.Movies.Include(m => m.Category).Include(m => m.Cinema).AsQueryable();

            if (filter.MovieName != null) { movies = movies.Where(m => m.Name.Contains(filter.MovieName)); ViewBag.MovieName = filter.MovieName; }
            if (filter.MinPrice > 0) { movies = movies.Where(m => m.Price >= filter.MinPrice); ViewBag.MinPrice = filter.MinPrice; }
            if (filter.MaxPrice > 0) { movies = movies.Where(m => m.Price <= filter.MaxPrice); ViewBag.MaxPrice = filter.MaxPrice; }
            if (filter.CategoryId > 0) { movies = movies.Where(m => m.CategoryId == filter.CategoryId); ViewBag.CategoryId = filter.CategoryId; }
            if (filter.CinemaId > 0) { movies = movies.Where(m => m.CinemaId == filter.CinemaId); ViewBag.CinemaId = filter.CinemaId; }
            if (filter.IsUpcoming) { movies = movies.Where(m => m.DateTime > DateTime.Now); ViewBag.IsUpcoming = true; }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Cinemas = _context.Cinemas.ToList();
            ViewBag.TotalPages = (int)Math.Ceiling(movies.Count() / 8.0);
            ViewBag.CurrentPage = filter.Page;

            return View(movies.Skip((filter.Page - 1) * 8).Take(8).AsEnumerable());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new MovieVM { Movie = new Movie { DateTime = DateTime.Now }, Categories = _context.Categories.ToList(), Cinemas = _context.Cinemas.ToList(), AllActors = _context.Actors.ToList() });
        }

        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile? ImgFile, List<IFormFile>? SubImgFiles, List<int>? ActorIds)
        {
            SaveMainImage(ImgFile, x => movie.MainImg = x);
            var savedMovie = _context.Movies.Add(movie);
            _context.SaveChanges();
            SaveSubImages(savedMovie.Entity.Id, SubImgFiles);
            SaveActors(savedMovie.Entity.Id, ActorIds);
            _context.SaveChanges();
            TempData["Success-Notification"] = "Movie Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return RedirectToAction("NotFoundPage", "Home");
            return View(new MovieVM
            {
                Movie = movie,
                Categories = _context.Categories.ToList(),
                Cinemas = _context.Cinemas.ToList(),
                AllActors = _context.Actors.ToList(),
                SelectedActorIds = _context.MovieActors.Where(ma => ma.MovieId == id).Select(ma => ma.ActorId).ToList(),
                MovieSubImages = _context.MovieSubImages.Where(ms => ms.MovieId == id).ToList()
            });
        }

        [HttpPost]
        public IActionResult Update(Movie movie, IFormFile? ImgFile, List<IFormFile>? SubImgFiles, List<int>? ActorIds)
        {
            var movieInDb = _context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == movie.Id);
            if (movieInDb == null) return RedirectToAction("NotFoundPage", "Home");

            if (ImgFile != null && ImgFile.Length > 0)
            {
                SaveMainImage(ImgFile, x => movie.MainImg = x);
                DeleteMainImage(movieInDb.MainImg);
            }
            else movie.MainImg = movieInDb.MainImg;

            _context.Movies.Update(movie);
            _context.SaveChanges();

            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                var oldImages = _context.MovieSubImages.Where(ms => ms.MovieId == movie.Id).ToList();
                foreach (var item in oldImages) DeleteSubImage(item.Img);
                _context.MovieSubImages.RemoveRange(oldImages);
                SaveSubImages(movie.Id, SubImgFiles);
            }

            var oldActors = _context.MovieActors.Where(ma => ma.MovieId == movie.Id).ToList();
            _context.MovieActors.RemoveRange(oldActors);
            SaveActors(movie.Id, ActorIds);

            _context.SaveChanges();
            TempData["Success-Notification"] = "Movie Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return RedirectToAction("NotFoundPage", "Home");
            DeleteMainImage(movie.MainImg);
            var oldImages = _context.MovieSubImages.Where(ms => ms.MovieId == id).ToList();
            foreach (var item in oldImages) DeleteSubImage(item.Img);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            TempData["Success-Notification"] = "Movie Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        void SaveMainImage(IFormFile? file, Action<string> set)
        {
            if (file == null || file.Length == 0) return;
            var fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(dir);
            using var stream = System.IO.File.Create(Path.Combine(dir, fileName));
            file.CopyTo(stream);
            set(fileName);
        }

        void SaveSubImages(int movieId, List<IFormFile>? files)
        {
            if (files == null) return;
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "movieSubImages");
            Directory.CreateDirectory(dir);
            foreach (var file in files)
            {
                if (file == null || file.Length == 0) continue;
                var fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                using var stream = System.IO.File.Create(Path.Combine(dir, fileName));
                file.CopyTo(stream);
                _context.MovieSubImages.Add(new MovieSubImage { MovieId = movieId, Img = fileName });
            }
        }

        void SaveActors(int movieId, List<int>? actorIds)
        {
            if (actorIds == null) return;
            foreach (var actorId in actorIds)
                _context.MovieActors.Add(new MovieActor { MovieId = movieId, ActorId = actorId });
        }

        void DeleteMainImage(string img)
        {
            if (img == "default.png") return;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", img);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }

        void DeleteSubImage(string img)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "movieSubImages", img);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }
    }
}
