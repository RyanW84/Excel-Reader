# ExcelReader.RyanW84

A comprehensive .NET 9 console application for reading and writing various file formats including Excel, CSV, and PDF files. The application provides a user-friendly console interface for importing data from different file types and managing database operations.

## Requirements

This is an application that will read data from an Excel spreadsheet into a database.

### Core Requirements

- When the application starts, it should delete the database if it exists, create a new one, create all tables, read from Excel, seed into the database
- You need to use EPPlus package
- You shouldn't read into Json first
- You can use SQLite or SQL Server (or MySQL if you're using a Mac)
- Once the database is populated, you'll fetch data from it and show it in the console
- You don't need any user input
- You should print messages to the console letting the user know what the app is doing at that moment (i.e. reading from excel; creating tables, etc)
- The application will be written for a known table, you don't need to make it dynamic
- When submitting the project for review, you need to include an xls file that can be read by your application

## Challenges

- Before anything else you'll have to create an Excel table that will be stored in your main project folder. The more organised the easier it will be for your program to read it. The first row of your columns need to be the property names of your model class

- Don't forget to create a Github repository for your project from the beginning.

- Remember, this time you don't need any user input. The only interaction your program will have with the user is to show the data from your database.

- You could structure the program in three parts. One for database creation, one for reading from the file and return a list and the last to populate your database using the returned list

## Features

- **Excel Operations**
  - Beginner Excel import functionality
  - Dynamic Excel reading and writing
  - Support for multiple worksheets
  - Excel file creation and modification

- **CSV Operations**
  - CSV file reading and importing
  - Data conversion to database tables

- **PDF Operations**
  - PDF table extraction and import
  - PDF form reading and writing
  - Form field manipulation

- **Database Integration**
  - Entity Framework Core with SQL Server
  - Automatic table creation
  - Data persistence and retrieval

- **User Interface**
  - Interactive console menu using Spectre.Console
  - Color-coded output and notifications
  - Error handling and user feedback

## Technology Stack

- **.NET 9.0** - Target framework
- **C# 13.0** - Programming language
- **Entity Framework Core 9.0.7** - ORM for database operations
- **SQL Server** - Database provider (LocalDB)

## NuGet Packages

The following NuGet packages are used in this project:

- **BouncyCastle.Cryptography** (v2.6.1) - Cryptographic operations
- **EntityFramework** (v6.5.1) - Legacy Entity Framework support
- **EPPlus** (v8.0.8) - Excel file reading and writing operations
- **itext7** (v9.2.0) - PDF manipulation and processing
- **itext7.bouncy-castle-adapter** (v9.2.0) - iText7 cryptographic adapter
- **Microsoft.EntityFrameworkCore** (v9.0.7) - Core Entity Framework functionality
- **Microsoft.EntityFrameworkCore.Abstractions** (v9.0.7) - EF Core abstractions
- **Microsoft.EntityFrameworkCore.Design** (v9.0.7) - Design-time tools for EF Core
- **Microsoft.EntityFrameworkCore.SqlServer** (v9.0.7) - SQL Server provider for EF Core
- **Microsoft.EntityFrameworkCore.Tools** (v9.0.7) - Command-line tools for EF Core
- **Microsoft.Extensions.Hosting** (v9.0.7) - Generic hosting support
- **Microsoft.Extensions.Logging.Console** (v9.0.7) - Console logging provider
- **Spectre.Console** (v0.50.0) - Rich console UI framework

## Prerequisites

- .NET 9.0 SDK
- SQL Server LocalDB (included with Visual Studio)
- Visual Studio 2022 or compatible IDE

## Installation

1. Clone the repository
2. Restore NuGet packages using the following command:
   "dotnet restore"
3. Update the database connection string in `appsettings.json` if needed
4. **Apply database migrations** (if using migrations approach):
 "dotnet ef database update"
5. Or using Package Manager Console in Visual Studio:
Open Package Manager Console and run "Update-Database"
6. Build the solution:
   "dotnet build"
7. Run the application:
   "dotnet run"
   Ensure the Excel file is present in the project directory for import functionality

## Usage
Run the application:
"dotnet run"

The application will present an interactive menu with the following options:
- **Excel: Beginner Import** - Basic Excel file import functionality
- **Excel: Dynamic Import** - Advanced Excel reading with dynamic structure
- **Excel: Write** - Write data to Excel files
- **CSV: Import** - Import data from CSV files
- **PDF: Import** - Extract data from PDF tables
- **PDF: Form Import** - Read PDF form fields
- **PDF: Form Write** - Write data to PDF forms

## Configuration

The application uses `appsettings.json` for configuration:

- **ConnectionStrings.DefaultConnection** - Database connection string
- **Logging** - Logging level configuration

## Project Structure
├── Abstractions/          # Interface definitions
├── Controller/            # Business logic controllers
├── Data/                 # Database context and models
├── Helpers/              # Utility classes
├── Models/               # Data models
├── Repositories/         # Data access layer
├── Services/            # Business services
├── UserInterface/       # Console UI components
└── Program.cs           # Application entry point

## License

EPPlus Non-Commercial License - This project uses EPPlus under the non-commercial license for personal use.

## Author

Ryan Weavers (RyanW84)

## Contributing

This is a personal learning project. Feel free to fork and modify for your own educational purposes.

## Credits
This project was built with the help of the following resources and tutorials:

- [The C# Academy - Excel Reader Project](https://thecsharpacademy.com/project/20/excel-reader)
- [Reading Excel Files with C# and EPPlus](https://coder.farawaytech.com/articles/reading_excel/c_sharp/epplus)
- [Simplifying SQL Table Creation from Excel File Header using .NET Core](https://dotnetcode.medium.com/simplifying-sql-table-creation-from-excel-file-header-using-net-core-4da655f60219)
- [EPPlus Software - Getting Started with Community License](https://epplussoftware.com/en/Home/GettingStartedCommunityLicense)
- [Read Write Excel in .NET Core using EPPlus](https://thecodebuzz.com/read-write-excel-in-dotnet-core-epplus/)
- [Stack Overflow - Import CSV or XLSX into DataTable using EPPlus](https://stackoverflow.com/questions/51633830/import-a-csv-or-xlsx-into-a-datatable-using-epplus#:~:text=For%20work%20with%20csv%2C%20you,and%20xlsm%20and%20write%20xlsx.)
- [How to Create a Fillable PDF in LibreOffice](https://www.wps.com/blog/how-to-create-a-fillable-pdf-in-libreoffice-a-step-by-step-guide/)
- [Stack Overflow - Is there a way to edit a PDF with C#?](https://stackoverflow.com/questions/2923546/is-there-a-way-to-edit-a-pdf-with-c)





