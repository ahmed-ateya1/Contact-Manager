Sure, here is the README content formatted for your project:

---

# Contacts Manager

Contacts Manager is a web application built with ASP.NET Core that allows users to manage their contacts. It provides features such as adding, editing, deleting, and searching for contacts. The application also supports role-based authentication and authorization, with separate roles for admins and regular users.

## Features
- **User Authentication:** Users can register, login, and logout.
- **Role-Based Authorization:** Admin and user roles with different access levels.
- **Contact Management:** Add, edit, delete, and search for contacts.
- **File Uploads:** Upload countries and other files.
- **Data Export:** Export contact data to PDF, CSV, and Excel.
- **Validation:** Client-side and server-side validation for forms.

## Project Structure
- **ContactsManager.Core:** Contains core domain entities, DTOs (Data Transfer Objects), enumerations, helpers, services, and service contracts.
- **ContactsManager.Infrastructure:** Contains infrastructure-related classes like application context, configuration, migrations, and repositories.
- **ContactsManager.UI:** The main web application project containing controllers, views, filters, middleware, and other UI-related components.
- **ContactManager.ServiceTest:** Contains unit tests for the service layer.

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/ContactsManager.git
   ```

2. Navigate to the project directory:
   ```bash
   cd ContactsManager
   ```

3. Restore the dependencies:
   ```bash
   dotnet restore
   ```

4. Update the database:
   ```bash
   dotnet ef database update --project ContactsManager.Infrastructure
   ```

5. Run the application:
   ```bash
   dotnet run --project ContactsManager.UI
   ```

## Usage

- **Register a new user:** Go to the `/Account/Register` endpoint and create a new user.
- **Login:** Go to the `/Account/Login` endpoint to login with your registered credentials.
- **Manage Contacts:**
  - **Add a new contact:** Click on the "Create Person" button.
  - **Edit a contact:** Click on the "Edit" button next to a contact.
  - **Delete a contact:** Click on the "Delete" button next to a contact.
  - **Search for contacts:** Use the search functionality to find specific contacts.
- **Export Data:** Click on "Download PDF", "Download CSV", or "Download Excel" to export the contact data.

## Screenshots

![Main Screen](path/to/screenshot.png)

## Contributing

Contributions are welcome! Please open an issue or submit a pull request with your changes.

## License

This project is licensed under the MIT License. See the LICENSE file for details.

---
