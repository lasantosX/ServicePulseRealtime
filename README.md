# ServicePulse RealTime

ServicePulse RealTime is a real-time notification dashboard built with ASP.NET Core, SignalR, SQL Server, and Vue.js.

The project demonstrates a production-style full stack architecture for service operation notifications, including REST APIs, real-time updates, SQL persistence, frontend filtering, API validation, and centralized error handling.

## Tech Stack

### Backend
- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server LocalDB
- SignalR
- Swagger / OpenAPI
- Global exception middleware
- Service layer architecture

### Frontend
- Vue 3
- TypeScript
- Vite
- Axios
- Microsoft SignalR Client
- Responsive dashboard UI

## Features

- Create service notifications
- Store notifications in SQL Server
- Display live notifications in Vue
- Real-time updates using SignalR
- Mark notifications as read
- Filter notifications by status
- Dashboard stats for total, unread, success, warning, and error notifications
- Seed sample notifications
- Swagger API documentation
- Centralized exception handling
- Basic API validation

## Architecture

```txt
ServicePulseRealtime
│
├── ServicePulseRealtime.Api
│   ├── Controllers
│   ├── Data
│   ├── DTOs
│   ├── Hubs
│   ├── Interfaces
│   ├── Middleware
│   ├── Models
│   └── Services
│
└── servicepulse-realtime-ui
    ├── src/api
    ├── src/services
    ├── src/types
    └── src/views
API Endpoints
Method	Endpoint	Description
GET	/api/Notifications	Get all notifications
POST	/api/Notifications	Create a notification
PUT	/api/Notifications/{id}/mark-as-read	Mark notification as read
POST	/api/Notifications/seed	Create sample notifications
SignalR Events
Event	Description
NotificationCreated	Broadcasts a new notification to connected clients
NotificationUpdated	Broadcasts notification read status changes
How to Run the Backend

Open the solution in Visual Studio.

Set ServicePulseRealtime.Api as the startup project.

Run the API using the http profile.

Swagger should be available at:

http://localhost:5245/swagger

If needed, apply the database migration:

Update-Database
How to Run the Frontend

From the frontend folder:

cd servicepulse-realtime-ui
npm install
npm run dev

Frontend URL:

http://localhost:5173
Local Development URLs

Backend:

http://localhost:5245

Frontend:

http://localhost:5173

SignalR Hub:

http://localhost:5245/hubs/notifications
Sample Notification Request
{
  "title": "Repair Order Updated",
  "message": "RO #10235 was updated successfully.",
  "type": "Success"
}
Portfolio Pitch

This project shows my ability to build real-time, production-style full stack systems using ASP.NET Core, SignalR, SQL Server, and Vue.js.


<img width="1229" height="817" alt="image" src="https://github.com/user-attachments/assets/78b117bc-b517-4c50-a260-91ac27544cd2" />

It demonstrates backend API design, database persistence, frontend integration, real-time communication, error handling, and clean project structure.
