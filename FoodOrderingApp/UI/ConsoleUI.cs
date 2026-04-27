using System.Net.Http.Json;
using FoodOrderingApp.DTOs.User;
using FoodOrderingApp.DTOs.Menu;
using FoodOrderingApp.DTOs.Order;
using FoodOrderingApp.DTOs.Restaurant;

namespace FoodOrderingApp.UI;

public class ConsoleUI
{
    private readonly HttpClient _client = new()
    {
        BaseAddress = new Uri("http://localhost:5095/")
    };

    private bool _isUserLoggedIn = false;
    private int? _currentRestaurantId = null;

    // ================= COLORS =================

    private static void WriteColor(string text, ConsoleColor color, bool newLine = true)
    {
        Console.ForegroundColor = color;
        if (newLine) Console.WriteLine(text);
        else Console.Write(text);
        Console.ResetColor();
    }

    private static void PrintSuccess(string msg) => WriteColor("✅ " + msg, ConsoleColor.Green);
    private static void PrintError(string msg) => WriteColor("❌ " + msg, ConsoleColor.Red);
    private static void PrintInfo(string msg) => WriteColor("ℹ️  " + msg, ConsoleColor.Cyan);
    private static void PrintWarning(string msg) => WriteColor("⚠️  " + msg, ConsoleColor.Yellow);

    private static void PressEnterToContinue()
    {
        Console.WriteLine();
        WriteColor("Press Enter to continue...", ConsoleColor.DarkGray);
        Console.ReadLine();
        Console.Clear();
    }

    // ================= HEADER =================

    private void PrintHeader()
    {
        Console.Clear();
        WriteColor("╔══════════════════════════════════════╗", ConsoleColor.DarkYellow);
        WriteColor("║      🍽️  FOOD ORDERING SYSTEM  🍽️      ║", ConsoleColor.DarkYellow);
        WriteColor("╚══════════════════════════════════════╝", ConsoleColor.DarkYellow);
        Console.WriteLine();
    }

    // ================= MENU =================

    public async Task Start()
    {
        while (true)
        {
            PrintHeader();

            if (!_isUserLoggedIn && _currentRestaurantId == null)
            {
                WriteColor("  👤 USER", ConsoleColor.Cyan);
                WriteColor("  1. Register User", ConsoleColor.White);
                WriteColor("  2. Login as User", ConsoleColor.White);
                Console.WriteLine();
                WriteColor("  🏪 RESTAURANT", ConsoleColor.Magenta);
                WriteColor("  3. Login as Restaurant", ConsoleColor.White);
            }
            else if (_isUserLoggedIn)
            {
                WriteColor($"  👤 Logged in as User", ConsoleColor.Green);
                Console.WriteLine();
                WriteColor("  🛒 ORDER", ConsoleColor.Cyan);
                WriteColor("  4. Place Order", ConsoleColor.White);
                WriteColor("  5. View My Orders", ConsoleColor.White);
                WriteColor("  6. View Order By Id", ConsoleColor.White);
                Console.WriteLine();
                WriteColor("  7. 🚪 Logout", ConsoleColor.DarkGray);
            }
            else if (_currentRestaurantId != null)
            {
                WriteColor($"  🏪 Logged in as Restaurant ID: {_currentRestaurantId}", ConsoleColor.Green);
                Console.WriteLine();
                WriteColor("  🍴 MENU MANAGEMENT", ConsoleColor.Magenta);
                WriteColor("  8.  Add Menu Item", ConsoleColor.White);
                WriteColor("  9.  View My Menu", ConsoleColor.White);
                WriteColor("  10. Set Item Availability", ConsoleColor.White);
                Console.WriteLine();
                WriteColor("  11. 🚪 Logout", ConsoleColor.DarkGray);
            }

            Console.WriteLine();
            WriteColor("  0. ❌ Exit", ConsoleColor.DarkRed);
            Console.WriteLine();
            WriteColor("Enter choice: ", ConsoleColor.Yellow, newLine: false);

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                PrintError("Invalid input");
                PressEnterToContinue();
                continue;
            }

            switch (choice)
            {
                case 1: await RegisterUser(); break;
                case 2: await LoginUser(); break;
                case 3: LoginRestaurant(); break;
                case 4: await PlaceOrder(); break;
                case 5: await GetMyOrders(); break;
                case 6: await GetOrderById(); break;
                case 7: Logout(); break;
                case 8: await AddMenuItem(); break;
                case 9: await ViewMenu(); break;
                case 10: await SetAvailability(); break;
                case 11: Logout(); break;
                case 0:
                    Console.Clear();
                    WriteColor("👋 Goodbye! Thanks for using Food Ordering System.", ConsoleColor.Cyan);
                    Console.WriteLine();
                    return;
                default:
                    PrintError("Invalid choice");
                    PressEnterToContinue();
                    break;
            }
        }
    }

    // ================= USER =================

    private async Task RegisterUser()
    {
        PrintHeader();
        WriteColor("👤 REGISTER USER", ConsoleColor.Cyan);
        Console.WriteLine();

        WriteColor("Name: ", ConsoleColor.Yellow, newLine: false);
        var name = Console.ReadLine();

        WriteColor("Email: ", ConsoleColor.Yellow, newLine: false);
        var email = Console.ReadLine();

        var response = await _client.PostAsJsonAsync("api/User/register", new { name, email });
        var msg = await response.Content.ReadAsStringAsync();

        Console.WriteLine();
        if (response.IsSuccessStatusCode)
            PrintSuccess("User registered successfully!");
        else
            PrintError($"Failed: {msg}");

        PressEnterToContinue();
    }

    private async Task LoginUser()
    {
        PrintHeader();
        WriteColor("🔐 LOGIN AS USER", ConsoleColor.Cyan);
        Console.WriteLine();

        WriteColor("Email: ", ConsoleColor.Yellow, newLine: false);
        var email = Console.ReadLine();

        WriteColor("Password: ", ConsoleColor.Yellow, newLine: false);
        var password = Console.ReadLine();

        var response = await _client.PostAsJsonAsync("api/User/login", new { email, password });

        if (!response.IsSuccessStatusCode)
        {
            PrintError("Login failed. Check your credentials.");
            PressEnterToContinue();
            return;
        }

        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        var token = result?["token"];

        if (string.IsNullOrEmpty(token))
        {
            PrintError("Login failed. No token received.");
            PressEnterToContinue();
            return;
        }

        // ⭐ Store JWT — every subsequent request sends it automatically
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        _isUserLoggedIn = true;
        _currentRestaurantId = null;

        PrintSuccess("Login successful! Welcome 🎉");
        PressEnterToContinue();
    }

    // ================= RESTAURANT =================

    private void LoginRestaurant()
    {
        PrintHeader();
        WriteColor("🔐 LOGIN AS RESTAURANT", ConsoleColor.Magenta);
        Console.WriteLine();

        WriteColor("Enter Restaurant ID: ", ConsoleColor.Yellow, newLine: false);
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _currentRestaurantId = id;
            _isUserLoggedIn = false;
            _client.DefaultRequestHeaders.Authorization = null;
            PrintSuccess($"Logged in as Restaurant {id}");
        }
        else
        {
            PrintError("Invalid ID");
        }

        PressEnterToContinue();
    }

    private async Task ShowRestaurants()
    {
        var restaurants = await _client.GetFromJsonAsync<List<RestaurantDto>>("api/Restaurant");

        if (restaurants == null || restaurants.Count == 0)
        {
            PrintWarning("No restaurants available.");
            return;
        }

        Console.WriteLine();
        WriteColor("🏪 AVAILABLE RESTAURANTS", ConsoleColor.Magenta);
        Console.WriteLine();

        WriteColor($"  {"ID",-6} {"Name",-30}", ConsoleColor.DarkYellow);
        WriteColor($"  {"──",-6} {"────────────────────────────",-30}", ConsoleColor.DarkGray);

        foreach (var r in restaurants)
        {
            WriteColor($"  {r.Id,-6}", ConsoleColor.Cyan, newLine: false);
            WriteColor($" {r.Name,-30}", ConsoleColor.White);
        }
    }

    private async Task AddMenuItem()
    {
        PrintHeader();
        WriteColor("➕ ADD MENU ITEM", ConsoleColor.Magenta);
        Console.WriteLine();

        WriteColor("Item Name: ", ConsoleColor.Yellow, newLine: false);
        var name = Console.ReadLine();

        WriteColor("Price (₹): ", ConsoleColor.Yellow, newLine: false);
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            PrintError("Invalid price");
            PressEnterToContinue();
            return;
        }

        var response = await _client.PostAsJsonAsync("api/Menu", new
        {
            restaurantId = _currentRestaurantId,
            name,
            price
        });

        Console.WriteLine();
        if (response.IsSuccessStatusCode)
            PrintSuccess("Menu item added successfully!");
        else
            PrintError(await response.Content.ReadAsStringAsync());

        PressEnterToContinue();
    }

    private async Task ViewMenu()
    {
        PrintHeader();
        WriteColor("🍴 MY MENU", ConsoleColor.Magenta);
        Console.WriteLine();

        var menu = await _client.GetFromJsonAsync<List<MenuItemDto>>(
            $"api/Menu/{_currentRestaurantId}"
        );

        if (menu == null || menu.Count == 0)
        {
            PrintWarning("No menu items found.");
            PressEnterToContinue();
            return;
        }

        PrintMenuTable(menu);
        PressEnterToContinue();
    }

    private static void PrintMenuTable(List<MenuItemDto> menu)
    {
        WriteColor($"  {"ID",-6} {"Name",-28} {"Price",-10} {"Available",-10}", ConsoleColor.DarkYellow);
        WriteColor($"  {"──",-6} {"────────────────────────────",-28} {"─────",-10} {"─────────",-10}", ConsoleColor.DarkGray);

        foreach (var item in menu)
        {
            var availColor = item.IsAvailable ? ConsoleColor.Green : ConsoleColor.Red;
            var availText = item.IsAvailable ? "✅ Yes" : "❌ No";

            WriteColor($"  {item.Id,-6}", ConsoleColor.Cyan, newLine: false);
            WriteColor($" {item.Name,-28}", ConsoleColor.White, newLine: false);
            WriteColor($" {"₹" + item.Price,-10}", ConsoleColor.Yellow, newLine: false);
            WriteColor($" {availText,-10}", availColor);
        }
    }

    private async Task SetAvailability()
    {
        PrintHeader();
        WriteColor("🔄 SET ITEM AVAILABILITY", ConsoleColor.Magenta);
        Console.WriteLine();

        WriteColor("Enter Item ID: ", ConsoleColor.Yellow, newLine: false);
        if (!int.TryParse(Console.ReadLine(), out int itemId))
        {
            PrintError("Invalid ID");
            PressEnterToContinue();
            return;
        }

        WriteColor("Available? (true/false): ", ConsoleColor.Yellow, newLine: false);
        if (!bool.TryParse(Console.ReadLine(), out bool isAvailable))
        {
            PrintError("Invalid input. Enter true or false.");
            PressEnterToContinue();
            return;
        }

        var url = $"api/Menu/availability?itemId={itemId}&restaurantId={_currentRestaurantId}&isAvailable={isAvailable}";
        var response = await _client.PutAsync(url, null);

        Console.WriteLine();
        if (response.IsSuccessStatusCode)
            PrintSuccess($"Item {itemId} availability set to {isAvailable}");
        else
            PrintError(await response.Content.ReadAsStringAsync());

        PressEnterToContinue();
    }

    // ================= ORDER =================

    private async Task PlaceOrder()
    {
        PrintHeader();
        WriteColor("🛒 PLACE ORDER", ConsoleColor.Cyan);

        await ShowRestaurants();

        Console.WriteLine();
        WriteColor("Enter Restaurant ID: ", ConsoleColor.Yellow, newLine: false);
        if (!int.TryParse(Console.ReadLine(), out int restaurantId))
        {
            PrintError("Invalid Restaurant ID");
            PressEnterToContinue();
            return;
        }

        var menu = await _client.GetFromJsonAsync<List<MenuItemDto>>(
            $"api/Menu/{restaurantId}"
        );

        if (menu == null || menu.Count == 0)
        {
            PrintWarning("No menu available for this restaurant.");
            PressEnterToContinue();
            return;
        }

        Console.WriteLine();
        WriteColor("🍴 MENU", ConsoleColor.Cyan);
        Console.WriteLine();
        PrintMenuTable(menu);

        var items = new List<object>();

        while (true)
        {
            Console.WriteLine();
            WriteColor("Enter Menu Item ID (0 to finish): ", ConsoleColor.Yellow, newLine: false);
            if (!int.TryParse(Console.ReadLine(), out int menuId) || menuId == 0)
                break;

            var selected = menu.FirstOrDefault(m => m.Id == menuId);

            if (selected == null)
            {
                PrintError("Invalid Menu Item ID");
                continue;
            }

            if (!selected.IsAvailable)
            {
                PrintWarning($"'{selected.Name}' is currently unavailable.");
                continue;
            }

            WriteColor("Quantity: ", ConsoleColor.Yellow, newLine: false);
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                PrintError("Invalid quantity");
                continue;
            }

            items.Add(new { menuItemId = menuId, quantity = qty });
            PrintSuccess($"Added {qty}x {selected.Name} to cart 🛒");
        }

        if (items.Count == 0)
        {
            PrintWarning("No items selected. Order cancelled.");
            PressEnterToContinue();
            return;
        }

        var response = await _client.PostAsJsonAsync("api/Order/place", new
        {
            items
        });

        Console.WriteLine();
        if (response.IsSuccessStatusCode)
            PrintSuccess("🎉 Order placed successfully!");
        else
            PrintError(await response.Content.ReadAsStringAsync());

        PressEnterToContinue();
    }

    private async Task GetMyOrders()
    {
        PrintHeader();
        WriteColor("📦 MY ORDERS", ConsoleColor.Cyan);
        Console.WriteLine();

        var orders = await _client.GetFromJsonAsync<List<OrderDto>>("api/Order/user");

        if (orders == null || orders.Count == 0)
        {
            PrintWarning("No orders found.");
            PressEnterToContinue();
            return;
        }

        foreach (var o in orders)
        {
            var statusColor = o.Status.ToLower() switch
            {
                "delivered" => ConsoleColor.Green,
                "cancelled" => ConsoleColor.Red,
                "pending" => ConsoleColor.Yellow,
                "preparing" => ConsoleColor.Cyan,
                _ => ConsoleColor.White
            };

            Console.WriteLine();
            WriteColor($"  🧾 Order #{o.Id}", ConsoleColor.DarkYellow, newLine: false);
            WriteColor($"   💰 ₹{o.TotalPrice}   Status: ", ConsoleColor.White, newLine: false);
            WriteColor(o.Status, statusColor);

            WriteColor($"  {"Item",-28} {"Qty",-6} {"Price",-10}", ConsoleColor.DarkGray);
            WriteColor($"  {"────────────────────────────",-28} {"───",-6} {"─────",-10}", ConsoleColor.DarkGray);

            foreach (var item in o.Items)
            {
                WriteColor($"  {item.MenuItemName,-28}", ConsoleColor.White, newLine: false);
                WriteColor($" {item.Quantity,-6}", ConsoleColor.Cyan, newLine: false);
                WriteColor($" ₹{item.PriceAtTime,-10}", ConsoleColor.Yellow);
            }
        }

        PressEnterToContinue();
    }

    private async Task GetOrderById()
    {
        PrintHeader();
        WriteColor("🔍 VIEW ORDER BY ID", ConsoleColor.Cyan);
        Console.WriteLine();

        WriteColor("Enter Order ID: ", ConsoleColor.Yellow, newLine: false);
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            PrintError("Invalid ID");
            PressEnterToContinue();
            return;
        }

        var response = await _client.GetAsync($"api/Order/{orderId}");

        Console.WriteLine();
        if (response.IsSuccessStatusCode)
            PrintInfo(await response.Content.ReadAsStringAsync());
        else
            PrintError(await response.Content.ReadAsStringAsync());

        PressEnterToContinue();
    }

    private void Logout()
    {
        _isUserLoggedIn = false;
        _currentRestaurantId = null;
        _client.DefaultRequestHeaders.Authorization = null;
        PrintSuccess("Logged out successfully.");
        PressEnterToContinue();
    }
}