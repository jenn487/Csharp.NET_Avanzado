# API de Gestión de Tareas
Esta es una Minimal API que utiliza la arquitectura Onion con Code First en Entity Framework Core. Libre de controladores ni clases adicionales, permite gestionar tareas con operaciones CRUD. Soporta programación asíncrona y manejo centralizado de errores. 


# 🧅 Capas de la arquitectura Onion
El proyecto se divide en 4 capas siguiendo la arquitectura Onion:

1. Domain (TaskManagement.Domain) Define las entidades (Tareas) y reglas del negocio. Es el núcleo del sistema.

2. Application (TaskManagement.Application) Contiene la lógica de negocio: validaciones, cálculos con Func, y notificaciones con Action.

3. Infrastructure (TaskManagement.Infrastructure) Implementa el acceso a datos con EF Core. Aquí viven los repositorios y filtros con funciones anónimas.

4. API "Presentation" (TaskManagement.API) Expone los endpoints HTTP. Usa delegados, Action y Func para procesar cada solicitud de forma flexible.


## Tecnologías
- .NET 8
- Minimal API
- Entity Framework Core (InMemory)
- Arquitectura Onion

## Opcion 1 para ejecutar
1. En la barra de direcciones del navegador, reemplaza github.com por github.dev
2. Presiona Enter y se abrirá una versión web de Visual Studio Code con el proyecto cargado.

## Opcion 2 para ejecutar
1. Clona el repositorio.
2. Ejecuta desde el proyecto `TaskManagement.API`.

dotnet run --project TaskManagement.API

