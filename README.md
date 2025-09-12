# Gym Team10 🏋️‍♂️

This project demonstrates the implementation of CRUD operations with DTOs, validations, and in-memory persistence for a **gym management API**.

---

## 👥 Team Members
- Member 1: ELIAS SORIA JOAQUIN MATEO  
- Member 2: GARCIA MEZA OLMOS FABIO ADRIAN  
- Member 3: MONTAÑO MEJIA KATHERINE FABIANA  
- Member 4: PITA VARGAS ARIANA AYLEN  

---

## 🎯 Objective
Implement CRUD for the following entities using DTOs and validations:

- **Users** (gym clients)  
- **Trainers**  
- **Memberships**  
- **Routines**  

All data is stored **in-memory** for demonstration purposes.

---

## ✨ Features
- **Users:** Create, Read, Update, Delete client records.  
- **Trainers:** Manage gym trainers (CRUD).  
- **Memberships:** Manage membership plans (CRUD).  
- **Routines:** Manage training routines (CRUD).  
- **DTOs (Data Transfer Objects):** Decouple API requests/responses from entities.  
- **Validations:** Ensure data integrity (required fields, correct formats, ranges).  
- **Pagination, filtering, and sorting:** All `GET` endpoints support pagination, filtering, sorting, and meta info in the response.  

---

## 🛠️ Tech Stack
- **.NET / Java / Node.js (depending on implementation)**  
- **DataAnnotations** (for validations)  
- **Postman** (for testing endpoints)  

---

## 📌 Endpoints Overview

### 👤 Users
- `GET /api/v1/users` → List all users (supports pagination, search, age filter, sorting)  
- `GET /api/v1/users/{id}` → Get user by ID  
- `POST /api/v1/users` → Register a new user  
- `PUT /api/v1/users/{id}` → Update user information  
- `DELETE /api/v1/users/{id}` → Delete a user  

---

### 🏋️ Trainers
- `GET /api/v1/trainers` → List all trainers (supports pagination, search, specialty filter, sorting)  
- `GET /api/v1/trainers/{id}` → Get trainer by ID  
- `POST /api/v1/trainers` → Add a new trainer  
- `PUT /api/v1/trainers/{id}` → Update trainer info  
- `DELETE /api/v1/trainers/{id}` → Delete a trainer  

---

### 🎟 Memberships
- `GET /api/v1/memberships` → List all memberships (supports pagination, price/duration filters, sorting)  
- `GET /api/v1/memberships/{id}` → Get membership by ID  
- `POST /api/v1/memberships` → Create a new membership  
- `PUT /api/v1/memberships/{id}` → Update membership plan  
- `DELETE /api/v1/memberships/{id}` → Remove membership  

---

### 🏃 Routines
- `GET /api/v1/routines` → List all routines (supports pagination, search, duration filters, sorting)  
- `GET /api/v1/routines/{id}` → Get routine by ID  
- `POST /api/v1/routines` → Register a new routine  
- `PUT /api/v1/routines/{id}` → Update routine info  
- `DELETE /api/v1/routines/{id}` → Delete a routine  

---

## ✅ Example Validations
- **User:** Must have a valid email, name not empty, and age ≥ 16.  
- **Trainer:** Must include name, specialty, and valid email.  
- **Membership:** Must have a type, duration ≥ 1 month, and price ≥ 0.  
- **Routine:** Must include a name, description, and duration in weeks > 0.  

---

## 🌐 Example URLs
- Users → `http://localhost:3000/api/v1/users`  
- Trainers → `http://localhost:3000/api/v1/trainers`  
- Memberships → `http://localhost:3000/api/v1/memberships`  
- Routines → `http://localhost:3000/api/v1/routines`  

---

## 🌱 Branches
- **gym/team10** → Main integration branch for the team.  




