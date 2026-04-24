# Car parts: Fullstack Monorepo

## Goal

Build a fullstack monorepo with a React frontend and an ASP.NET Core backend that serves data from a DocumentDB database.

---

## Repository Structure

```
/
├── frontend/       # Vite + React + TypeScript app
└── backend/        # ASP.NET Core API
```

---

## Frontend

**Stack:** Vite · React · TypeScript · AG Grid · RTK Query

### Tasks

- Set up the project with `npm create vite@latest` using the React + TypeScript template
- Install and configure [RTK Query](https://redux-toolkit.js.org/rtk-query/overview) to fetch data from the backend API
- Display the fetched car part data in an [AG Grid](https://www.ag-grid.com/) table (community edition is fine)
- Enable inline cell editing in the grid and send updates back to the backend
- Handle loading and error states in the UI

### Structure suggestion

```
frontend/
├── src/
│   ├── api/          # RTK Query service definitions
│   ├── components/   # React components (including the grid)
│   └── main.tsx
├── index.html
└── vite.config.ts
```

---

## Backend

**Stack:** ASP.NET Core · C# · MongoDB Driver · DocumentDB

### Architecture

Follow a three-layer pattern inside the `backend/` project:

| Layer | Responsibility |
|---|---|
| **Controller** | Handles HTTP requests, maps to/from DTOs |
| **Model** | Business logic, orchestrates data access |
| **Collection** | MongoDB data access, one class per entity |

### Tasks

- Create an ASP.NET Core Web API project
- Define a `CarPart` entity (e.g. `Id`, `Name`, `PartNumber`, `Description`) and expose it via REST endpoints
- Implement GET (list), PUT (update) endpoints at minimum
- Implement a `Collection` class using the [MongoDB .NET Driver](https://www.mongodb.com/docs/drivers/csharp/) connected to DocumentDB
- Register services via dependency injection in `Program.cs`
- Use interfaces for `Collection` classes to keep the `Model` layer testable

### Structure suggestion

```
backend/
├── Controllers/
│   └── CarPartController.cs
├── Model/
│   └── CarPartModel.cs
├── Data/
│   ├── Entity/
│   │   └── CarPart.cs
│   └── Mongo/
│       ├── ICarPartCollection.cs
│       └── CarPartCollection.cs
└── Program.cs
```

---

## Getting Started

### Backend

```bash
cd backend
dotnet run
```

### Frontend

```bash
cd frontend
npm install
npm run dev
```

---

## Definition of Done

- [ ] Monorepo has both `frontend/` and `backend/` folders
- [ ] Backend exposes GET (list) and PUT (update) endpoints for `CarPart` entities backed by DocumentDB
- [ ] Frontend fetches car part data from the backend using RTK Query
- [ ] Car parts are displayed in an AG Grid table

## Optional
- Provide a function to delete parts
- Have a look at optimistic and pessimistic updates for rtk query
- [ ] Cells are editable inline in the grid and changes are persisted to the backend via RTK Query mutation
- [ ] Frontend and backend can be started independently from their respective folders
