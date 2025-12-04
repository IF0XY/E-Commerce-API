# 🛒 E-Commerce RESTful Web API

A fully structured, scalable, and secure **ASP.NET Core Web API** built
using **Clean Architecture**, **Unit of Work**, **Generic Repository**,
**JWT Authentication**, **Redis Caching**, and **Stripe Payments**.

This project follows professional backend engineering standards and is
designed for real-world production systems.

## 🚀 **Tech Stack**

-   **ASP.NET Core Web API**
-   **C# / .NET 8**
-   **Entity Framework Core**
-   **SQL Server**
-   **AutoMapper**
-   **JWT Authentication**
-   **Role-Based Authorization**
-   **Stripe Payment Integration**
-   **Redis Caching**
-   **Generic Repository Pattern**
-   **Unit of Work**
-   **Clean Architecture**

## 🧱 **Project Architecture**

    Domain (Entities, Interfaces)
    │
    Infrastructure (EF Core, Repositories, UoW, Configurations)
    │
    Application (Services, DTOs, Validation, Mapping)
    │
    Presentation (Controllers, API Endpoints)

## 🔐 **Key Features**

### Authentication & Authorization

-   JWT token-based authentication
-   Role-based user authorization

### High Performance

-   Redis caching
-   AutoMapper DTO mapping

### Payments

-   Stripe Payment integration

### Data Access

-   Generic Repository
-   Unit of Work

### API Enhancements

-   Global exception handling
-   Fluent validation
-   RESTful endpoint design

## 📁 **Folder Structure**

    E-Commerce-API
    │── Domain
    │── Infrastructure
    │── Application
    │── Presentation

## ▶️ **How to Run**

### Clone

    git clone https://github.com/IF0XY/E-Commerce-API.git

### Configure DB

Update `appsettings.json`.

### Apply migrations

    dotnet ef database update

### Run

    dotnet run

## 📌 **Sample Endpoints**

### Auth

  Method   Endpoint             Description
  -------- -------------------- -------------
  POST     /api/auth/register   Register
  POST     /api/auth/login      Login

### Products

  Method   Endpoint             Description
  -------- -------------------- ---------------
  GET      /api/products        All products
  GET      /api/products/{id}   Product by ID
  POST     /api/products        Add
  PUT      /api/products/{id}   Update
  DELETE   /api/products/{id}   Delete

## ⭐ **Give the project a star!**

## 🏷️ **Hashtags**

#dotnet #csharp #aspnetcore #dotnetcore #backenddevelopment #webapi
#restapi #cleanarchitecture #efcore #sqlserver #unitofwork
#repositorypattern #genericrepository #jwt #redis #stripepayments
#githubprojects #buildinpublic #scalablesystems
