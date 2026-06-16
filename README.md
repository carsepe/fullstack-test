# Prueba Técnica Fullstack

Este repositorio contiene una solución fullstack para una prueba técnica, con backend en .NET y frontend en Angular.

## Estructura

```text
backend/
  src/
    FullstackTest.Api/
    FullstackTest.Application/
    FullstackTest.Domain/
    FullstackTest.Infrastructure/
  tests/
    FullstackTest.UnitTests/
frontend/
```

## Backend

Requisitos:

- .NET SDK 10

Comandos útiles:

```bash
dotnet restore backend/FullstackTest.slnx
dotnet build backend/FullstackTest.slnx
dotnet test backend/FullstackTest.slnx
dotnet run --project backend/src/FullstackTest.Api/FullstackTest.Api.csproj --launch-profile http
```

La API queda disponible en `http://localhost:5065/` y Swagger en `http://localhost:5065/swagger`.

## Frontend

El frontend es una aplicación Angular ubicada en la carpeta `frontend/`.

Requisitos:

- Node.js
- npm

Comandos útiles:

```bash
cd frontend
npm install
npm start
npm run build
npm test -- --watch=false
```

El comando `npm start` ejecuta `ng serve`. La aplicación queda disponible localmente en `http://localhost:4200/`.

## Flujo de ramas

- `main`: versión estable del proyecto.
- `dev`: rama de desarrollo para los cambios de la prueba.
