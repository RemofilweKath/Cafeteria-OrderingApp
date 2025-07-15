using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaOrderingApp.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
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
    }
}
