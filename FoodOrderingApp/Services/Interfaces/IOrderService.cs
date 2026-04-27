
using FoodOrderingApp.Models;
using FoodOrderingApp.Models.Enum;
using FoodOrderingApp.DTOs.Order;

namespace FoodOrderingApp.Services.Interfaces
{
    public interface IOrderService
    {
        void PlaceOrder(int userId, List<(int menuItemId, int quantity)> items);

        void UpdateOrderStatus(int orderId, OrderStatus newStatus);

        List<OrderDto> GetUserOrders(int userId);

        // 🔒 NEW: required for secure single order access
        Order GetOrderById(int orderId, int userId);
    }
}
