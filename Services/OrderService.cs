using CafeteriaOrderingApp.Database;
using CafeteriaOrderingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaOrderingApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> PlaceOrderAsync(Order order)
        {
            if (order == null || order.EmployeeId <= 0)
                throw new ArgumentException("Invalid order details.");
            var employee = await _context.Employees.FindAsync(order.EmployeeId);
            if (employee == null)
                throw new ArgumentException("Employee not found.");

            decimal totalCost = 0;
            var orderItems = new List<OrderItem>();

            // Calculate total cost and prepare order items
            foreach (var item in order.OrderItems)
            {
                if (item.Quantity <= 0) continue;

                var menuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
                if (menuItem != null)
                {
                    totalCost += menuItem.Price * item.Quantity;
                    orderItems.Add(new OrderItem
                    {
                        MenuItemId = item.MenuItemId,
                        Quantity = item.Quantity,
                        UnitPriceAtTimeOfOrder = menuItem.Price
                    });
                }
            }

            // Check if balance is sufficient
            if (employee.Balance < totalCost)
                throw new InvalidOperationException("Insufficient funds to place this order.");

            // Deduct the total cost from employee's balance
            employee.Balance -= totalCost;

            // Create the order
            var orderDetaills = new Order
            {
                EmployeeId = order.EmployeeId,
                TotalAmount = totalCost,
                OrderDate = DateTime.Now,
                Status = Order.OrderStatus.Pending,
                OrderItems = orderItems
            };

            _context.Orders.Add(orderDetaills);
            await _context.SaveChangesAsync();

            return order;
        }

        
        /// Retrieves all orders for a specific employee, including order items
        public async Task<List<Order>> GetEmployeeOrdersAsync(int employeeId)
        {
            return await _context.Orders
                .Where(o => o.EmployeeId == employeeId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        /// Retrieves a specific order by its ID, including order items
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        /// Retrieves all pending orders (not yet "Delivered") for admin management.
        public async Task<List<Order>> GetPendingOrdersAsync()
        {
            return await _context.Orders
                .Where(o => o.Status != Order.OrderStatus.Delivered)
                .Include(o => o.Employee)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .OrderBy(o => o.OrderDate)
                .ToListAsync();
        }

        /// <summary>
        /// Updates the status of an existing order.
        /// </summary>
        /// <param name="orderId">The ID of the order to update.</param>
        /// <param name="status">The new status for the order (e.g., "Pending", "Preparing", "Delivering", "Delivered").</param>
        /// <returns>True if the update was successful; false if the order was not found.</returns>
        public async Task<bool> UpdateOrderStatusAsync(int orderId, Order.OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all orders in the system, useful for admin reporting or oversight.
        /// </summary>
        /// <returns>A list of all Orders, including employee and order item details.</returns>
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
