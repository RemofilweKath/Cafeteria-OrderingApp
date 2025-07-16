using CafeteriaOrderingApp.Database;
using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaOrderingApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ApplicationDbContext _context;

        public OrdersController(OrderService orderService, ApplicationDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            //var user = await _userManager.GetUserAsync(User);
            //if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            try
            {
                var purchase = await _orderService.PlaceOrderAsync(order);
                return RedirectToAction(nameof(Confirmation), new { orderId = purchase.Id });
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Restaurants");
            }
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return NotFound();

            var model = new OrderConfirmation
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                Items = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ItemName = oi.MenuItem.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPriceAtTimeOfOrder,
                    TotalPrice = oi.Quantity * oi.UnitPriceAtTimeOfOrder
                }).ToList()
            };

            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage()
        {
            var pendingOrders = await _context.Orders
                .Where(o => o.Status != Order.OrderStatus.Delivered)
                .Include(o => o.Employee)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .OrderBy(o => o.OrderDate)
                .ToListAsync();

            return View(pendingOrders);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, Order.OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Order #{orderId} status updated to {status}.";
            return RedirectToAction(nameof(Manage));
        }

        //[Authorize]
        public async Task<IActionResult> History(int id)
        {
            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //    return RedirectToAction("Login", "Account", new { area = "Identity" });

            var orders = await _orderService.GetEmployeeOrdersAsync(id);
            return View(orders);
        }
    }
}
