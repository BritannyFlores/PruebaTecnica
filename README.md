# Prueba Técnica/Práctica — Arquitectura Microservicio



**Autora:** Britanny Flores

**Fecha:** 13/09/2026



---



## Descripción general



Sistema bancario compuesto por 2 microservicios independientes desarrollados en .NET 8, que se comunican de forma asíncrona mediante RabbitMQ. El proyecto sigue los principios de Clean Architecture y aplica el patrón Repository, con manejo centralizado de excepciones, pruebas automatizadas y despliegue completo en contenedores Docker.



## Arquitectura



El sistema está dividido en 2 microservicios, cada uno con su propia base de datos y ciclo de vida independiente:



- **Cliente.Service** — gestiona las entidades `Persona` y `Cliente`. Expone un CRUD completo.

- **Cuenta.Service** — gestiona las entidades `Cuenta` y `Movimiento`. Expone operaciones CRU (crear, leer, actualizar) y el reporte de estado de cuenta. Mantiene una réplica local de los clientes (`ClienteReferencia`) sincronizada de forma asíncrona.

- **Shared.Contracts** — proyecto compartido con los eventos de integración (`ClienteCreado`, `ClienteActualizado`, `ClienteEliminado`) usados por ambos microservicios.



### Comunicación asíncrona



Cliente.Service publica eventos cada vez que se crea, actualiza o elimina un cliente. Cuenta.Service los consume mediante MassTransit + RabbitMQ y actualiza su tabla local `ClientesReferencia`, evitando así que ambos microservicios se acoplen por llamadas HTTP síncronas directas.



Cliente.Service → (evento) → RabbitMQ → (consumo) → Cuenta.Service





### Cada microservicio sigue una arquitectura por capas



Domain → Entidades puras, sin dependencias externas

Application → Casos de uso (Services), DTOs, interfaces, excepciones de negocio

Infrastructure → EF Core, repositorios concretos, MassTransit (publisher/consumers)

Api → Controllers, middleware de excepciones, configuración

Tests → Pruebas unitarias y de integración





## Tecnologías utilizadas



- **.NET 8** / ASP.NET Core Web API

- **Entity Framework Core 8** (Code First, SQL Server)

- **MassTransit 8.3.1 + RabbitMQ** (comunicación asíncrona)

- **xUnit + FluentAssertions** (pruebas unitarias e integración)

- **Docker / Docker Compose** (despliegue de todos los servicios)

- **Postman** (validación de endpoints)



## Requerimiento y funcionalidades implementadas



| Funcionalidad | Estado | Descripción |
|---|---|---|
| F1 | OK | CRUD completo de Cliente; CRU de Cuenta y Movimiento |
| F2 | OK | Registro de movimientos con actualización automática de saldo |
| F3 | OK | Validación de saldo no disponible al registrar un retiro |
| F4 | OK | Reporte de estado de cuenta por rango de fechas y cliente |
| F5 | OK | Prueba unitaria sobre la entidad de dominio Cliente |
| F6 | OK | Prueba de integración sobre el endpoint `/clientes` |
| F7 | OK | Despliegue completo en contenedores Docker |



Adicionalmente se implementó:

- Manejo centralizado de excepciones (middleware) en ambos microservicios

- Patrón Repository + Service (separación de responsabilidades)

- Comunicación asíncrona real entre microservicios (no simulada)

- Validaciones de negocio explícitas (identificación duplicada, número de cuenta duplicado, cliente inexistente)



## Requisitos previos



- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y corriendo

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (solo si se desea aplicar migraciones o correr pruebas fuera de Docker)

- Puertos libres antes de levantar el proyecto:



| Puerto | Servicio |
|---|---|
| `5001` | Cliente.Api |
| `5002` | Cuenta.Api |
| `1433` | SQL Server |
| `5672` | RabbitMQ (AMQP) |
| `15672` | RabbitMQ (panel de administración) |



> Si alguno de estos puertos está en uso en tu máquina, edita `docker-compose.yml` y cambia el número de la izquierda en el mapeo de puertos correspondiente (ej. `"5001:8080"` → `"5010:8080"`), y actualiza la variable `cliente_url` o `cuenta_url` en la colección de Postman con el nuevo puerto.



## Cómo levantar el proyecto



### 1. Clonar el repositorio



```bash

git clone https://github.com/BritannyFlores/PruebaTecnica.git

cd PruebaTecnica

```



### 2. Levantar todos los servicios con Docker Compose



```bash

docker-compose up --build

```



Esto construye las imágenes de ambos microservicios y levanta 4 contenedores: `sqlserver`, `rabbitmq`, `cliente-api`, `cuenta-api`.



Espera unos segundos a que todos los servicios terminen de inicializar (SQL Server puede tardar 20-30 segundos adicionales en estar listo para aceptar conexiones).



### 3. Aplicar las migraciones (solo la primera vez)



En otra terminal, desde la raíz del proyecto:



```bash

dotnet ef database update --project src/Cliente.Service/Cliente.Infrastructure --startup-project src/Cliente.Service/Cliente.Api --connection "Server=localhost,1433;Database=ClienteDB;User Id=sa;Password=PruebaTecnica2026!;TrustServerCertificate=True;"



dotnet ef database update --project src/Cuenta.Service/Cuenta.Infrastructure --startup-project src/Cuenta.Service/Cuenta.Api --connection "Server=localhost,1433;Database=CuentaDB;User Id=sa;Password=PruebaTecnica2026!;TrustServerCertificate=True;"

```



Esto crea las bases de datos `ClienteDB` y `CuentaDB` con todas sus tablas dentro del contenedor de SQL Server.



> Alternativamente, el archivo `BaseDatos.sql` incluido en la raíz del proyecto contiene el script completo y puede ejecutarse directamente desde SQL Server Management Studio conectándose a `localhost,1433` con usuario `sa` y la contraseña indicada arriba.



## Endpoints disponibles



**Cliente.Api:** `http://localhost:5001` — Swagger: `http://localhost:5001/swagger`

**Cuenta.Api:** `http://localhost:5002` — Swagger: `http://localhost:5002/swagger`



| Recurso | Método | Endpoint | Descripción |
|---|---|---|---|
| Clientes | GET | `/clientes` | Lista todos los clientes |
| Clientes | GET | `/clientes/{id}` | Obtiene un cliente por Id |
| Clientes | POST | `/clientes` | Crea un cliente |
| Clientes | PUT | `/clientes/{id}` | Actualiza un cliente |
| Clientes | DELETE | `/clientes/{id}` | Elimina un cliente |
| Cuentas | GET | `/cuentas` | Lista todas las cuentas |
| Cuentas | GET | `/cuentas/{numeroCuenta}` | Obtiene una cuenta por número |
| Cuentas | POST | `/cuentas` | Crea una cuenta |
| Cuentas | PUT | `/cuentas/{numeroCuenta}` | Actualiza una cuenta |
| Movimientos | GET | `/movimientos/{numeroCuenta}` | Lista movimientos de una cuenta |
| Movimientos | POST | `/movimientos` | Registra un movimiento |
| Reportes | GET | `/reportes?fechaInicio=...\&fechaFin=...\&cliente=...` | Genera el estado de cuenta |



## Validación con Postman



La colección se encuentra en `postman/PruebaTecnica.postman_collection.json` e incluye sus propias variables (`cliente_url`, `cuenta_url`, `clienteId`, `numeroCuenta`) — no requiere configuración adicional al importarla.



### Orden recomendado de ejecución

La colección usa datos fijos (identificación y número de cuenta), pensada para ejecutarse **una sola vez sobre una base de datos limpia**:

1. Carpeta `1. Clientes` — ejecutar únicamente las requests **01 a 06** en este punto (dejar `07 - Eliminar cliente` para el final)
2. Carpeta `2. Cuentas` (requests 01 a 06, en orden)
3. Carpeta `3. Movimientos` (requests 01 a 04, en orden)
4. Carpeta `4. Reportes`
5. Carpeta `1. Clientes`, request `07 - Eliminar cliente` (al final, una vez completadas todas las pruebas anteriores)

> Varias requests están diseñadas intencionalmente para demostrar validaciones de negocio y devuelven un código de error esperado (identificadas en su nombre, por ejemplo *"identificacion duplicada - debe fallar"*). Esto es el comportamiento correcto, no un fallo del sistema.

> Si la colección se ejecuta una segunda vez sin reiniciar la base de datos, las requests de creación (`01`) también devolverán error de duplicado — la validación seguirá siendo correcta, pero conviene reiniciar los datos (`docker-compose down -v` seguido de `docker-compose up --build` y aplicar migraciones de nuevo) para repetir el flujo completo desde cero.

> **Importante:** `07 - Eliminar cliente` debe ejecutarse al final de toda la colección. Si se corre justo después de `01 - Crear cliente`, las carpetas de Cuentas, Movimientos y Reportes fallarán porque el cliente ya no existirá en `ClientesReferencia`.


## Pruebas automatizadas



```bash

dotnet test src/Cliente.Service/Cliente.Tests

```



Incluye:

- **3 pruebas unitarias** sobre la entidad de dominio `Cliente` (herencia, valores por defecto, asignación de propiedades)

- **2 pruebas de integración** sobre el endpoint `POST /clientes` y `GET /clientes/{id}`, usando una base de datos en memoria



## Script de base de datos



El archivo `BaseDatos.sql`, ubicado en la raíz del proyecto, contiene el script completo de creación de ambas bases de datos (`ClienteDB` y `CuentaDB`), generado directamente desde las migraciones de Entity Framework Core.



## Estructura del proyecto



PruebaTecnica/

├── docker-compose.yml

├── BaseDatos.sql

├── README.md

├── postman/

│ └── PruebaTecnica.postman_collection.json

└── src/

├── Shared.Contracts/

│ └── Events/ - Eventos compartidos entre microservicios

├── Cliente.Service/

│ ├── Cliente.Domain/ - Entidades: Persona, Cliente

│ ├── Cliente.Application/ - Services, DTOs, Interfaces, Exceptions

│ ├── Cliente.Infrastructure/ - DbContext, Repositories, MassTransit

│ ├── Cliente.Api/ - Controllers, Middleware, Dockerfile

│ └── Cliente.Tests/ - Pruebas unitarias e integración

└── Cuenta.Service/

├── Cuenta.Domain/ - Entidades: Cuenta, Movimiento, ClienteReferencia

├── Cuenta.Application/ - Services, DTOs, Interfaces, Exceptions

├── Cuenta.Infrastructure/ - DbContext, Repositories, MassTransit, Consumers

├── Cuenta.Api/ - Controllers, Middleware, Dockerfile

└── Cuenta.Tests/





## Consideraciones de diseño



- **Herencia TPT (Table Per Type):** `Cliente` hereda de `Persona` en tablas separadas (`Personas` y `Clientes`), relacionadas por `PersonaId`.

- **Saldo calculado dinámicamente:** el saldo de una cuenta se deriva del último movimiento registrado (o del saldo inicial si no hay movimientos), evitando inconsistencias entre un campo de saldo almacenado y el historial real de transacciones.

- **DTOs separados por operación:** se usan DTOs distintos para creación, actualización y lectura, evitando exponer directamente las entidades de dominio en la API.

- **Réplica de solo lectura:** `ClienteReferencia` en Cuenta.Service no es dueña de la verdad del cliente — se sincroniza exclusivamente vía eventos, respetando el principio de que cada microservicio es dueño de sus propios datos.






