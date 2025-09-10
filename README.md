# 🏋️ Gym API Challenge

## 🎯 Objetivo
Construir una API REST **desde cero** en ASP.NET Core que exponga **CRUD** para 3 recursos (Members, Memberships, CheckIns) y listados con:

- **Paginación**: `page`, `limit`  
- **Ordenamiento**: `sort`, `order`
- **Respuesta estandarizada**: `{ data, meta }`  

⚠️ **No se pide** CORS, rate limit ni filtros adicionales.

---

## 👥 Organización y Git Flow
- Rama de equipo:  
  ```bash
  git checkout -b gym/teamXX
  ```

- Subramas por feature:
  ```bash
    gym/teamXX-members

    gym/teamXX-memberships

    gym/teamXX-checkins

Merge de cada subrama → gym/teamXX

**Solo se revisará la rama principal del equipo.**


## 👥 Modelos y dtos esperados

```bash
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
    string BadgeCode,    // código/tarjeta del socio (ej: "GYM-12345")
    DateTime Timestamp
)
```
DTOS OBLIGATORIOS:
- CreateMemberDto
- UpdateMemberDto
- CreateMembershipDto
- UpdateMembershipDto
- CreateCheckInDto
- UpdateCheckInDto


## 📌 Endpoints obligatorios
Members:
```
GET /api/v1/members (lista con paginación, orden)

GET /api/v1/members/{id}

POST /api/v1/members

PUT /api/v1/members/{id}

DELETE /api/v1/members/{id}
```

Memberships:
```

GET /api/v1/memberships (lista con paginación, orden)

GET /api/v1/memberships/{id}

POST /api/v1/memberships

PUT /api/v1/memberships/{id}

DELETE /api/v1/memberships/{id}
```

CheckIns
```
GET /api/v1/checkins (lista con paginación, orden)

GET /api/v1/checkins/{id}

POST /api/v1/checkins

DELETE /api/v1/checkins/{id}
```

## Query Params comunes

* page (default 1, min 1)

* limit (default 10, rango 1–100)

* sort (campo permitido por recurso)

* order (asc | desc, default asc)


## Respuesta estándar
```
{
  "data": [ /* items */ ],
  "meta": { "page": 1, "limit": 10, "total": 134 }
}
```
## ✅ Reglas HTTP

201 Created → al crear

200 OK → al leer/actualizar

204 No Content → al borrar

400 Bad Request → validación fallida (ValidationProblem(ModelState))

404 Not Found → recurso inexistente

Ejemplo:
```
{ "error": "Member not found", "status": 404 }
```

## 🧪 Ejemplos JSON

POST /api/v1/members
```
{ "email":"ana@example.com", "fullName":"Ana Pérez", "active":true }

```
POST /api/v1/memberships
```
{
  "memberId":"00000000-0000-0000-0000-000000000001",
  "plan":"pro",
  "startDate":"2025-09-10T00:00:00Z",
  "endDate":"2026-09-10T00:00:00Z",
  "status":"active"
}
```

POST /api/v1/checkins
```
{ "badgeCode":"GYM-12345" }
```


## 🔗 Ejemplos de URLs
```
/api/v1/members?page=1&limit=5&sort=fullName&order=asc

/api/v1/memberships?sort=startDate&order=desc&page=2&limit=10

/api/v1/checkins?&sort=timestamp&order=desc&page=1&limit=20
```

# 🧾 Planilla de Evaluación (100 pts)
## 1) Modelos & DTOs (25 pts)

(10) Modelos correctos (GUID, campos exactos)

(10) DTOs con DataAnnotations adecuados

(5) Defaults correctos (status → active, Timestamp → now)

## 2) CRUD & HTTP (25 pts)

(10) Endpoints CRUD completos

(10) Códigos HTTP correctos (201/200/204/400/404)

(5) Errores sobrios en JSON

## 3) Listados (30 pts)

(15) Paginación (page, limit) con normalización

(15) Ordenamiento (sort, order) seguro

## 4) Respuesta estándar (10 pts)

(10) { data, meta } en todos los listados

## 5) Proceso & README (10 pts)

(5) Git flow correcto (subramas → merge)

(5) README con endpoints, ejemplos JSON y URLs de listados

# Aprobación mínima: 70 pts


