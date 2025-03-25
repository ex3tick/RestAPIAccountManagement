# RestAPIAccountManagement - Documentation

## 1. Project Overview
RestAPIAccountManagement is a RESTful API for managing user accounts. The API provides functionality for user registration, authentication, updating, and deletion. A secure password hashing method using Salt and Pepper is implemented.

This service is designed as a **microservice** and is part of a larger application that follows the **Proxy Pattern**. The overarching system aims to pre-sort support tickets using artificial intelligence. This allows for efficient ticket management and routing.

The motivation behind this project is to expand my portfolio by demonstrating my skills in building scalable, secure, and efficient microservices using modern technologies.

## 2. Installation and Configuration

1. **Clone the Repository:**
    ```bash
    git clone <repository-url>
    ```
2. **Install Dependencies:**
    ```bash
    dotnet restore
    ```
3. **Set Environment Variables:**
    - Create or edit `appsettings.json`
    - Set the connection string and Pepper value:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Your MySQL Connection String"
      },
      "Secret": {
        "Pepper": "YourPepperValue"
      }
    }
    ```
4. **Start the Application:**
    ```bash
    dotnet run
    ```

## 3. Setting Up a Local MySQL Database with Docker
You can easily set up a local MySQL database using Docker. Follow these steps:

1. **Ensure Docker is Installed:** Download and install Docker from [Docker's official website](https://www.docker.com/).

2. **Run a MySQL Container:**
    ```bash
    docker run --name mysql-db -e MYSQL_ROOT_PASSWORD=yourpassword -e MYSQL_DATABASE=AccountManagementDB -p 3306:3306 -d mysql:8.0
    ```
    - `MYSQL_ROOT_PASSWORD`: Set your root password.
    - `MYSQL_DATABASE`: The database will be automatically created if it doesn’t exist.
    
3. **Update appsettings.json:**
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;Database=AccountManagementDB;User=root;Password=yourpassword;"
      }
    }
    ```

4. **Automatic Database Creation:**
    If the database specified in the connection string does not exist, it will be created automatically upon application startup using the `CreateDatabase` class.

## 4. API Endpoints

### **1. User Authentication**
- **POST /Account/login**
  - Description: Logs in a user using email/username and password.
  - Parameters: `emailOrName`, `password`
  - Response: HTTP 200 on success, HTTP 401 for invalid credentials.

### **2. Create User**
- **POST /Account/register**
  - Description: Registers a new user.
  - Parameters: `UserModel`
  - Response: HTTP 201 on success, HTTP 400 for invalid data.

### **3. Update User**
- **PUT /Account/update**
  - Description: Updates user data.
  - Parameters: `DTOUpdate`
  - Response: HTTP 200 on success, HTTP 404 if user not found.

### **4. Delete User**
- **DELETE /Account/delete**
  - Description: Deletes a user account.
  - Parameters: `DTODelete`
  - Response: HTTP 200 on success, HTTP 404 if user not found.

## 5. Database Information
The application uses a MySQL database. All CRUD operations are handled through the `AccountDAL` class. The connection string is managed via `appsettings.json`.

## 6. Hashing Mechanism
Password hashing is implemented using a combination of:
- **Salt:** A unique value generated per user.
- **Pepper:** A global secret value from `appsettings.json`.

The `HashHelper.HashGenerate()` method uses HMAC-SHA256 for secure password hashing.

## 7. Models and DTOs
### UserModel
```csharp
public class UserModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
```

## 8. Error Messages and Status Codes
- **200 OK:** Successful request.
- **201 Created:** User created successfully.
- **400 Bad Request:** Invalid request parameters.
- **401 Unauthorized:** Invalid login credentials.
- **404 Not Found:** User not found.

---
This is the complete documentation. If you need further adjustments or additional details, feel free to ask!

