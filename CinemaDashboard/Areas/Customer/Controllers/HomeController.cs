namespace CinemaDashboard.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult Index(FilterCustomerMovieVM filter)
        {
            var movies = _context.Movies.Where(m => m.Status == true).Include(m => m.Category).Include(m => m.Cinema).AsQueryable();

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
            return View(movies.Skip((filter.Page - 1) * 8).Take(8).ToList());
        }

        [HttpPost]
        public IActionResult Book(Booking booking)
        {
            booking.BookedAt = DateTime.Now;
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            TempData["Success-Notification"] = "Booking confirmed successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult MyBookings(string? phone, int Page = 1)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                ViewBag.TotalPages = 0; ViewBag.CurrentPage = 1; ViewBag.Phone = phone;
                return View(new List<Booking>());
            }

            var bookings = _context.Bookings.Where(b => b.CustomerPhone == phone).Include(b => b.Movie).ThenInclude(m => m.Cinema).AsQueryable();
            ViewBag.TotalPages = (int)Math.Ceiling(bookings.Count() / 8.0);
            ViewBag.CurrentPage = Page;
            ViewBag.Phone = phone;
            return View(bookings.Skip((Page - 1) * 8).Take(8).ToList());
        }

        public IActionResult DeleteBooking(int id, string phone)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
                TempData["Success-Notification"] = "Booking deleted successfully.";
            }
            return RedirectToAction(nameof(MyBookings), new { phone });
        }
    }
}
