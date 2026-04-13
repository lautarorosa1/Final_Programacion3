PROYECTO FULL STACK

Aplicación web desarrollada con arquitectura cliente-servidor, utilizando Vue.js para el frontend y C# con .NET para el backend, junto con SQL Server como base de datos.

TECNOLOGÍAS UTILIZADAS

Frontend:

Vue.js
HTML5
CSS3
JavaScript

Backend:

C# (.NET)
ASP.NET Core Web API
Entity Framework Core

Base de datos:

SQL Server
Migraciones con Entity Framework

ESTRUCTURA DEL PROYECTO

/mi-proyecto
|-- frontend (Aplicación Vue)
|-- backend (API en C#)
|-- README.txt

INSTALACIÓN Y EJECUCIÓN

Clonar el repositorio

git clone https://github.com/tuusuario/tu-repo.git
cd tu-repo

BACKEND (C# / .NET)

Requisitos:

.NET SDK instalado
SQL Server instalado

Pasos:

Ir a la carpeta del backend:
cd backend
Configurar la cadena de conexión en el archivo appsettings.json:

"ConnectionStrings": {
"DefaultConnection": "TU_CONNECTION_STRING"
}

Aplicar migraciones para crear la base de datos:
dotnet ef database update
Ejecutar el backend:
dotnet run

El backend se ejecutará en:
https://localhost:xxxx

FRONTEND (VUE)

Requisitos:

Node.js instalado

Pasos:

Ir a la carpeta del frontend:
cd frontend
Instalar dependencias:
npm install
Ejecutar la aplicación:
npm run serve

El frontend se ejecutará en:
http://localhost:8080

BASE DE DATOS

Este proyecto utiliza Entity Framework Core con migraciones, por lo que no es necesario importar scripts SQL manualmente.

La base de datos se genera automáticamente ejecutando:
dotnet ef database update

CONFIGURACIÓN

Antes de ejecutar el proyecto:

Configurar correctamente la conexión a SQL Server
Verificar que el backend esté en ejecución antes del frontend
Ajustar las URLs de la API en el frontend si es necesario

FUNCIONALIDADES

Alta de datos
Listado de información
Comunicación entre frontend y backend mediante API REST
Persistencia de datos en SQL Server

HERRAMIENTAS RECOMENDADAS

Visual Studio o Visual Studio Code
SQL Server Management Studio
Postman

AUTOR

Tu Nombre

LICENCIA

Proyecto de uso académico.