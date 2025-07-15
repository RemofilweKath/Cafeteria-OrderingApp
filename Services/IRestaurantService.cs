using CafeteriaOrderingApp.Models;

namespace CafeteriaOrderingApp.Services
{
    public interface IRestaurantService
    {
        void AddRestaurant(Restaurant restaurant);
        IEnumerable<Restaurant> GetAllRestaurants();
        Restaurant GetRestaurantById(int id);
        void UpdateRestaurant(Restaurant restaurant);
        void DeleteRestaurant(int id);
        void AddMenuItem(MenuItem menuItem);
        IEnumerable<MenuItem> GetMenuItemsByRestaurant(int restaurantId);
        void UpdateMenuItem(MenuItem menuItem);
        void DeleteMenuItem(int id);
    }
}