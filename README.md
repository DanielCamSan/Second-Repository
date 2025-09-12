# Gym API - Team10 🏋️‍♂️

This project demonstrates the implementation of a **REST API in ASP.NET Core** that exposes CRUD operations and listings for **3 resources**: **Members**, **Memberships**, and **CheckIns**.  
The implementation includes DTOs, validations, and in-memory persistence for demo purposes.

---

## 👥 Team Members
- Elias Soria Joaquin Mateo  
- Garcia Meza Olmos Fabio Adrian  
- Montaño Mejia Katherine Fabiana  
- Pita Vargas Ariana Aylen  

---

## 🎯 Objective
Build a REST API from scratch in **ASP.NET Core** that provides CRUD operations for 3 resources (Members, Memberships, CheckIns) and listings with:

- **Pagination:** `page`, `limit`  
- **Sorting:** `sort`, `order`  
- **Standardized response:** `{ data, meta }`
  
---

## 📦 Models
Member(
    Guid Id,
    string Email,
    string FullName,
    bool Active
)

Membership(
    Guid Id,
    Guid MemberId,
    string Plan,         // basic | pro | premium
    DateTime StartDate,
    DateTime EndDate,
    string Status        // active | expired | canceled
)

CheckIn(
    Guid Id,
    string BadgeCode,    // example: "GYM-12345"
    DateTime Timestamp
)

### Endpoints
**Members**
- GET /api/v1/members → list (pagination + sorting)
- GET /api/v1/members/{id} → get by id
- POST /api/v1/members → create (201 Created)
- PUT /api/v1/members/{id} → update (200 OK)
- DELETE /api/v1/members/{id} → delete (204 No Content)

**Memberships**
GET /api/v1/memberships → list (pagination + sorting)
GET /api/v1/memberships/{id} → get by id
POST /api/v1/memberships → create (201 Created)
PUT /api/v1/memberships/{id} → update (200 OK)
DELETE /api/v1/memberships/{id} → delete (204 No Content)

**CheckIns**
GET /api/v1/checkins → list (pagination + sorting)
GET /api/v1/checkins/{id} → get by id
POST /api/v1/checkins → create (201 Created)
DELETE /api/v1/checkins/{id} → delete (204 No Content)









