# 🏘️ RealEstateApp – ASP.NET Core Real Estate Platform

A scalable real estate management web application built with **ASP.NET Core MVC**, designed to support property listings, agent-client interaction, and role-based access. Developed with industry-grade principles like **Onion Architecture**, **SOLID**, **CQRS**, and secure authentication using **JWT + Identity**, this project exemplifies robust design and clean engineering practices.

---

## 🚀 Features

- 🏡 **Property Listings**  
  Filter and explore properties by type, price, rooms, and amenities, including image galleries and agent contact.

- 🧑‍💼 **Role-Based Access (Admin, Agent, Client)**  
  Tailored views and operations based on login role—admin dashboards, agent property management, and client favorites.

- 🛠️ **Property Maintenance Module**  
  Agents can create, update, and delete properties with photo uploads, unique auto-generated codes, and validation rules.

- 🔐 **Authentication & Authorization**  
  Secured login system using **ASP.NET Core Identity**, JWT-based token validation, role permissions, and access protection.

- 📧 **Email Notifications**  
  Account activation and onboarding flow via **SMTP integration** for Clients.

- 🧱 **Admin Panel**  
  Manage agents, admins, developers, property types, sales types, and upgrade features—all from a powerful interface.

- 📑 **Swagger UI Documentation**  
  Interactive API documentation for developers using **Swagger/OpenAPI**.

## 🎯 Purpose

This application was developed as a functional and technical capstone, with the goal of:
- Modeling a robust real estate management system with admin-agent-client collaboration.
- Applying layered software architecture and modern .NET backend standards.
- Demonstrating code modularity, maintainability, and system scalability.

---

## 🧠 Architecture & Principles

| Layer                  | Tech Stack / Pattern                        |
|------------------------|---------------------------------------------|
| Authentication & Authz | ASP.NET Core Identity + JWT                 |
| Core Architecture      | Onion Architecture                          |
| Validation             | FluentValidation + Behavior Validation      |
| Design Patterns        | CQRS, Mediator Pattern (MediatR)            |
| Database Access        | Entity Framework Core (Code First)          |
| Documentation          | Swagger / OpenAPI                           |
| Design Principles      | SOLID, DRY, Clean Code, Separation of Concerns |

---

## 📂 Getting Started

1. Clone the repository  
   `git clone https://github.com/WilmeGM/SocialMediaApp.git`

2. Update `appsettings.Example.Development.json` with your DB, JTW and SMTP config or create your own based on.

3. Apply database migrations  
   `dotnet ef database update`

4. Run the application  
   `dotnet run`

## 📄 License

This project is licensed under the MIT License – use it, fork it, and feel free to contribute! 🛠️
