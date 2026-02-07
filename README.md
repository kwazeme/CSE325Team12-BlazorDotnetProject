# CSE325Team12-BlazorDotnetProject

PropertyHub is a Blazor Server app for managing rental listings, tenants, and leases with user authentication and full CRUD functionality.

## Requirements
- .NET 10 SDK
- SQLite

## Getting Started
1. Restore packages.
2. Apply database migrations.
3. Run the app.

## Commands
```powershell
dotnet restore
dotnet ef migrations add AddListingsAndLeases
dotnet ef database update
dotnet run --project .\PropertyHub\PropertyHub.csproj
```

## Usage Guide
1. Register a new account.
2. Create listings with address, city, state, listing type, and rent.
3. Add tenants with contact info.
4. Create leases that link listings and tenants.
5. Edit or delete items from the respective lists.

## Testing Checklist (Manual)
- Register, login, logout.
- Create, view, edit, and delete listings.
- Create, view, edit, and delete tenants.
- Create, view, edit, and delete leases.
- Verify users only see their own data.
