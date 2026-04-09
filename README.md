# 3-Tier Architecture Contact Management System

A WPF desktop application demonstrating 3-tier architecture pattern with SQL Server database.

## Architecture

```
├── Presentation Layer (WPF)     # UI Layer - WPF Application
├── Business Layer              # Business Logic Layer
├── Data Layer                  # Data Access Layer
└── script.sql                  # Database Script
```

## Features

- **Contact Management**: Add, Edit, Delete, View contacts
- **Country Management**: Support for multiple countries with phone codes
- **3-Tier Architecture**: Clear separation of concerns
  - Presentation Layer: WPF UI
  - Business Layer: Business logic and validation
  - Data Layer: Database operations with ADO.NET

## Technology Stack

- **.NET 8.0**
- **WPF** (Windows Presentation Foundation)
- **SQL Server** (Microsoft.Data.SqlClient)
- **C#**

## Database Setup

1. Execute `script.sql` to create the database
2. Update connection string in `Data Layer/clsDataLayerSettings.cs`

```csharp
// Default connection string
"Server=.;Database=ContactsDBv1;User Id=sa;Password=12345;TrustServerCertificate=True;"
```

## Requirements

- Visual Studio 2022 or later
- SQL Server (Express or Developer)
- .NET 8.0 Runtime

## How to Run

1. Open `Small project to practicing 3 Tire Architure.sln` in Visual Studio
2. Restore NuGet packages
3. Build and run the solution
4. Ensure SQL Server is running and database is created

## Project Structure

| Layer | Description |
|-------|-------------|
| Presentation Layer (WPF) | MainWindow.xaml, AddEditForm.xaml |
| Business Layer | clsContacts.cs, clsCountries.cs |
| Data Layer | DataLayerContacts.cs, DataLayerCountries.cs |

## Screenshots

(Add your application screenshots here)

## License

MIT License