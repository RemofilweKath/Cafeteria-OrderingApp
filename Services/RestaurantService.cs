using CafeteriaOrderingApp.Database;
using CafeteriaOrderingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaOrderingApp.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext _context;

        public RestaurantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();
        }

        public IEnumerable<Restaurant> GetAllRestaurants()
        {
            return _context.Restaurants.ToList();
        }

        public async Task<Restaurant> GetRestaurantById(int id)
        {
            return await _context.Restaurants.Include(r => r.MenuItems).FirstOrDefaultAsync(r => r.Id == id); ;
        }

        public void UpdateRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            _context.SaveChanges();
        }

        public void DeleteRestaurant(int id)
        {
            var restaurant = _context.Restaurants.Find(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                _context.SaveChanges();
            }
        }

        public void AddMenuItem(MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();
        }

        public IEnumerable<MenuItem> GetMenuItemsByRestaurant(int restaurantId)
        {
            return _context.MenuItems.Where(mi => mi.RestaurantId == restaurantId).ToList();
        }

        public void UpdateMenuItem(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            _context.SaveChanges();
        }

        public void DeleteMenuItem(int id)
        {
            var menuItem = _context.MenuItems.Find(id);
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                _context.SaveChanges();
            }
        }
    }
}
