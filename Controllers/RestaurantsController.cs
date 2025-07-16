using CafeteriaOrderingApp.Database;
using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaOrderingApp.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ApplicationDbContext _context;

        public RestaurantsController(IRestaurantService restaurantService, ApplicationDbContext context)
        {
            _restaurantService = restaurantService;
            _context = context;
        }

        // View all restaurants
        public IActionResult Index()
        {
            var restaurants = _restaurantService.GetAllRestaurants();
            return View(restaurants);
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = _restaurantService.GetRestaurantById(id.Value);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // Display restaurant creation form
        public IActionResult Create()
        {
            return View();
        }

        // Handle restaurant creation form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _restaurantService.AddRestaurant(restaurant);
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // Display edit form for a restaurant
        public IActionResult Edit(int id)
        {
            var restaurant = _restaurantService.GetRestaurantById(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        // Handle edit form submission
        [HttpPost]
        public IActionResult Edit(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _restaurantService.UpdateRestaurant(restaurant);
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // Confirm deletion of a restaurant
        public IActionResult Delete(int id)
        {
            var restaurant = _restaurantService.GetRestaurantById(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        // Handle deletion after confirmation
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _restaurantService.DeleteRestaurant(id);
            return RedirectToAction(nameof(Index));
        }

        // Display detailed menu for a specific restaurant
        public async Task<IActionResult> Menu(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.MenuItems)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
                return NotFound();

            return View(restaurant);
        }

        // Add a new menu item
        public void AddMenuItem(MenuItem menuItem)
        {
            _restaurantService.AddMenuItem(menuItem);
        }

        // View all menu items
        public IEnumerable<MenuItem> ViewMenu(int restaurantId)
        {
            return _restaurantService.GetMenuItemsByRestaurant(restaurantId);
        }

        // Edit a menu item
        public void EditMenuItem(MenuItem menuItem)
        {
            _restaurantService.UpdateMenuItem(menuItem);
        }

        // Delete a menu item
        public void DeleteMenuItem(int itemId)
        {
            _restaurantService.DeleteMenuItem(itemId);
        }
    }
}
