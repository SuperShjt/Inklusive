# Employee Management System

This project is a **Fullstack Employee Management System** built using **.NET Core** for the backend and **HTML/CSS/JavaScript** for the frontend. It includes authentication, role-based access control, and user profile management.

## Features
- **Authentication & Authorization:**
  - Users can log in using their credentials.
  - JWT Token is stored in **Cookies** after login.
  - Role-based access control for Super Admin, Admin, and Employee roles.
- **User Management**:
  - Super Admin can manage Admin and Employee accounts.
  - Admins can update their own profiles and approve Employee profile changes.
  - Employees can request profile updates, which require Admin approval.
- **Login System**:
  - First-time login requires users **Employees** to change their password.
  - Secure authentication using **JWT (JSON Web Tokens)**.
- **REST API**:
  - Used for authentication and role management.
- **Database**:
  - Uses **Microsoft SQL Server (MSSQL)** for data storage.
- **Frontend UI**:
  - Built with **HTML, CSS, and JavaScript**.

## Technologies Used
- **Backend**: .NET Core with **Entity Framework Core (EF Core)**
- **Database**: Microsoft SQL Server (**MSSQL**)
- **Authentication**: JSON Web Tokens (**JWT**) stored in **Cookies**
- **Frontend**: HTML, CSS, JavaScript
- **API**: REST API for authentication and user management
- **Development Environment**: Visual Studio, SQL Server Management Studio

---

## Installation Guide

Follow these steps to set up and run the project on your local machine.

### Prerequisites

- Install **Visual Studio** (with .NET Core SDK)
- Install **SQL Server** & **SQL Server Management Studio (SSMS)**

### Steps to Setup & Run

1. **Clone the repository**:
   ```sh
   git clone <repository-url>
   cd <project-folder>
   ```

2. **Set up the database**:
   - Open **SQL Server Management Studio (SSMS)** and create a new database named `EmployeeManagementDB`.
   - Open **Visual Studio** and locate `appsettings.json` in the project.
   - Update the `ConnectionStrings` with your MSSQL database details:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=EmployeeManagementDB;Trusted_Connection=True;"
     }

3. **Run Database Migrations**:
   - Open **Package Manager Console** in Visual Studio and run:
     ```sh
     Update-Database
     ```

4. **Start the API server**:
   - Open a terminal or command prompt.
   - Navigate to the project folder and run:
     ```sh
     dotnet run
     ```
   - The backend API will start running on `http://localhost:5000/` (or a different port if specified).
   - Or open the project in **Visual Studio** and click "Run".

5. **Login Credentials**:
   - Create your Account and choose your role.  
   - After the first login, users ***Employees*** are required to change their password.
   
6. **JWT Authentication**:
   - After login, a JWT token is stored in **Cookies**.
   - The token is automatically sent with API requests for authentication and role verification.

---

## API Endpoints

- `POST /User/Login` - User login (returns JWT token).
- `POST /User/Register` - Register a new user.
- `GET /User/UserIfo` - Get profile details (for logged-in users).
- `PUT /User/UpdateEProfile` - Request profile update (requires admin approval).
- `PUT /User/EditAdminOwnInfo` - Update Admin own profile.
- `DELETE /User/Delete/{userId}` - Super Admin can delete a user.
- `PUT /User/ApproveProfileUpdate?profileUpdateId={id}` - Admin approves employee profile updates.

---

## Additional Notes

- Make sure to **configure your database connection** in `appsettings.json` before running the application.
- After logging in, the **JWT token is stored in Cookies** for authentication.
- Use **Postman** or a similar tool to test API endpoints.
- If the project is not running, check that your database is running and the connection string is correct.

For any issues or improvements, feel free to open an **issue** or a **pull request**. 🚀

