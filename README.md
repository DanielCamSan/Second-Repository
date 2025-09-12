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

- **Members** (gym clients)    
- **Memberships**  
- **CheckIns**  

All data is stored **in-memory** for demonstration purposes.

---

## ✨ Features
- **Members:** Create, Read, Update, Delete client records.   
- **Memberships:** Manage membership plans (CRUD).
- **CheckIns:** Manage gym trainers (CRUD).  
- **DTOs (Data Transfer Objects):** Decouple API requests/responses from entities.  
- **Validations:** Ensure data integrity (required fields, correct formats, ranges).  
- **Pagination, filtering, and sorting:** All `GET` endpoints support pagination, filtering, sorting, and meta info in the response.  

---

## 🛠️ Tech Stack
- **.NET **  
- **DataAnnotations** (for validations)  
- **Postman** (for testing endpoints)  

---

## 📌 Endpoints Overview

### 👤 Memberes
- `GET /api/v1/members` → List all members (supports pagination, search, age filter, sorting)  
- `GET /api/v1/members/{id}` → Get user by ID  
- `POST /api/v1/membersd` → Register a new member  
- `PUT /api/v1/members/{id}` → Update member information  
- `DELETE /api/v1/members/{id}` → Delete a member  

---

### 🎟 Memberships
- `GET /api/v1/memberships` → List all memberships (supports pagination, price/duration filters, sorting)  
- `GET /api/v1/memberships/{id}` → Get membership by ID  
- `POST /api/v1/memberships` → Create a new membership  
- `PUT /api/v1/memberships/{id}` → Update membership plan  
- `DELETE /api/v1/memberships/{id}` → Remove membership  

---

### 🏋️ CheckIns
- `GET /api/v1/checkins` → List all checkin (supports pagination, search, specialty filter, sorting)  
- `GET /api/v1/checkins/{id}` → Get checkin by ID  
- `POST /api/v1/checkins` → Add a new checkin  
- `PUT /api/v1/checkins/{id}` → Update checkin info  
- `DELETE /api/v1/checkins/{id}` → Delete a checkin
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





