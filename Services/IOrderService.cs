using CafeteriaOrderingApp.Models;

namespace CafeteriaOrderingApp.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(Order order);
        Task<List<Order>> GetEmployeeOrdersAsync(int employeeId);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetPendingOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(int orderId, Order.OrderStatus status);
        Task<List<Order>> GetAllOrdersAsync();
    }
}