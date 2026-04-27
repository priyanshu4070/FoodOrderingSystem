🍽 Food Ordering System (Console UI + .NET Web API)

A layered Food Ordering System built using .NET Web API + Console Application, following clean architecture principles with JWT-based authentication.

🚀 Features
👤 User
Register with email & password
Login using JWT authentication
Place orders
View personal orders
View order details securely
🏪 Restaurant
Add menu items
Update menu items
Set item availability
Delete menu items
🧾 Orders
Place multi-item orders
Track order status
Secure access (users can only see their own orders)
🧱 Architecture

This project follows a layered architecture:

ConsoleUI (Client)
   ↓
Controllers (API Layer)
   ↓
Services (Business Logic)
   ↓
Repositories (Data Access)
   ↓
Entity Framework Core (DbContext)
   ↓
SQL Server Database
🔐 Authentication (JWT)
Uses JSON Web Token (JWT) for secure authentication
Token generated during login
Stored in client and sent with each request
User identity extracted from token claims
Login → JWT Token → Authorization Header → Secure API Access
📁 Project Structure
FoodOrderingSolution/
│
├── FoodOrderingAPI/
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── DTOs/
│   ├── Models/
│   └── Data/
│
├── FoodOrderingApp/ (Console UI)
│   └── UI/
⚙️ Technologies Used
.NET (ASP.NET Core Web API)
C#
Entity Framework Core
SQL Server
JWT Authentication
Console Application (Client)
▶️ How to Run
1. Clone Repository
git clone https://github.com/your-username/food-ordering-system.git
2. Setup Database
Update connection string in appsettings.json
Run migrations:
Add-Migration InitialCreate
Update-Database
3. Run API
dotnet run

Swagger will open at:

https://localhost:xxxx/swagger
4. Run Console App
Set Console project as startup OR run separately
Login → get JWT → perform operations
🔑 API Endpoints
User
POST /api/User/register
POST /api/User/login
Orders (Protected)
POST /api/Order/place
GET /api/Order/user
GET /api/Order/{orderId}
Menu
GET /api/Menu/{restaurantId}
POST /api/Menu
PUT /api/Menu/update
DELETE /api/Menu
🔒 Security
Password-based login
JWT token validation
[Authorize] for protected endpoints
User identity extracted from token (no client-side userId)
📌 Key Concepts Implemented
Layered Architecture
Dependency Injection
Repository Pattern
DTO Pattern
JWT Authentication
Secure API Design
🎯 Future Improvements
Role-based authentication (User / Restaurant / Admin)
Password hashing (BCrypt)
Refresh tokens
UI upgrade (Web / React frontend)
Logging & exception middleware
