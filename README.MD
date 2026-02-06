# Real State App
Real State App una aplicación para el manejo y ventas de propiedades inmobiliarias.

Cada tipo de usuario en esta aplicación tiene acceso
a un conjunto propio de funcionalidades, que mencionaremos mas adelante.

## Tecnologías utilizadas
- **Lenguaje:** C#
- **FrontEnd:** Razor pages, Boostrap, Boostrap icons y HTML.
- **Backend:** ASP.NET MVC, Entity framework, JWT (para la API), Identity (para el sistema de usuarios)
- **Arquitectura:** Onion.

Notas: Para el desarrollo utilizamos SQL server, sin embargo, cualquier otra base datos que soporte EF debería funcionar sin problemas.

## Roles del sistema.
### Administrador
El administrador se encarga de monitorear el estado del sistema. Aqui un resumen de sus capacidades en la aplicación.

- Crear, habilitar/deshabilitar y eliminar cuentas de otros administradores.
- Crear, habilitar/deshabilitar y eliminar cuentas de desarrolladores de la APP.
- Habilitar/Deshabilitar y eliminar cuentas de los agentes.
- Crear, modificar y eliminar los distintos tipos de propiedades que se gestionan en la aplicación.
- Crear, modificar y eliminar los distintos tipos de ventas que se gestionan en la aplicación.
- Crear, modificar y eliminar las distintas mejoras que pueden ser asociadas a una propiedad en la aplicación.

### Agentes.
Los agentes son usuarios que pueden registrarse en la APP con este rol. Estos pueden crear, administrar y vender propiedades. Aquí un resumen de todas sus capacidades:
- Crear, modificar, eliminar sus propiedades.
- Aceptar / rechazar ofertas por dichas propiedades
- Actualizar su perfil en la aplicación.

### Clientes
Los clientes tambien son usuarios que pueden registrarse en la APP con este rol. Sus capacidades son:
- Ver y filtrar las propiedades registradas en la aplicación.
- Marcar propiedades como favoritos
- Realizar ofertas para comprar cierta propiedad al agente que la administra.

### Desarrolladores.
Los desarrolladores son un rol usando exclusivamente en la API de la aplicación, el WebApp no permite el loggeo de usuarios desarrolladores.

### invitados
Los usuarios no autenticados en la APP solamente pueden ver y filtrar las propiedades publicadas en la aplicación.

## Endpoints de la API

La API de **Real State App** está construida siguiendo los estándares REST y se encuentra documentada bajo la especificación OpenAPI 3.0.4. A continuación, se detallan los recursos disponibles:

### Account (Cuentas y Seguridad)
| Método | Endpoint | Descripción | Parámetros de Ruta |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/v1/Account/login` | Autentica un usuario y devuelve un token JWT. | N/A |
| `POST` | `/api/v1/Account/register-admin` | Registra un nuevo administrador (soporta carga de imagen). | N/A |
| `POST` | `/api/v1/Account/register-dev` | Registra un nuevo desarrollador (soporta carga de imagen). | N/A |
| `POST` | `/api/v1/Account/get-reset-token` | Genera y envía un token de recuperación de contraseña vía email. | N/A |
| `POST` | `/api/v1/Account/change-password` | Restablece la contraseña utilizando el token provisto. | N/A |

### Agent (Agentes)
| Método | Endpoint | Descripción | Parámetros de Ruta |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/v1/Agent` | Lista todos los agentes y su cantidad de propiedades. | N/A |
| `GET` | `/api/v1/Agent/{id}` | Obtiene el perfil detallado de un agente específico. | `{id}`: ID único del agente (string) |
| `GET` | `/api/v1/Agent/{id}/properties` | Lista todas las propiedades gestionadas por un agente. | `{id}`: ID único del agente (string) |
| `POST` | `/api/v1/Agent/change-status` | Activa o desactiva el estado de un agente. | N/A |

### Property (Propiedades)
| Método | Endpoint | Descripción | Parámetros de Ruta |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/v1/Property` | Lista todas las propiedades incluyendo info del agente. | N/A |
| `GET` | `/api/v1/Property/by-id/{id}` | Obtiene los detalles de una propiedad por su ID. | `{id}`: ID de la propiedad (int) |
| `GET` | `/api/v1/Property/by-code/{code}` | Busca una propiedad mediante su código único de sistema. | `{code}`: Código de propiedad (string) |

### Improvement (Mejoras/Amenidades)
| Método | Endpoint | Descripción | Parámetros de Ruta |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/v1/Improvement` | Lista todas las mejoras registradas. | N/A |
| `POST` | `/api/v1/Improvement` | Registra una nueva mejora en el sistema. | N/A |
| `PUT` | `/api/v1/Improvement` | Actualiza la información de una mejora existente. | N/A |
| `GET` | `/api/v1/Improvement/by-id/{id}` | Obtiene una mejora específica por ID. | `{id}`: ID de la mejora (int) |
| `DELETE` | `/api/v1/Improvement/{id}` | Elimina una mejora del sistema. | `{id}`: ID de la mejora (int) |

### PropertyType (Tipos de Propiedad)
| Método | Endpoint | Descripción | Parámetros de Ruta |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/v1/PropertyType` | Lista todos los tipos de propiedades. | N/A |
| `POST` | `/api/v1/PropertyType` | Crea una nueva categoría de propiedad. | N/A |
| `PUT` | `/api/v1/PropertyType` | Actualiza un tipo de propiedad existente. | N/A |
| `GET` | `/api/v1/PropertyType/{id}` | Obtiene un tipo de propiedad por ID. | `{id}`: ID del tipo (int) |
| `DELETE` | `/api/v1/PropertyType/{id}/delete` | Elimina un tipo de propiedad. | `{id}`: ID del tipo (int) |

### SaleType (Tipos de Venta)
| Método | Endpoint | Descripción | Parámetros de Ruta |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/v1/SaleType` | Lista los tipos de venta (Alquiler, Venta, etc.). | N/A |
| `POST` | `/api/v1/SaleType` | Registra un nuevo tipo de venta. | N/A |
| `PUT` | `/api/v1/SaleType` | Actualiza un tipo de venta existente. | N/A |
| `GET` | `/api/v1/SaleType/by-id/{id}` | Obtiene un tipo de venta específico por ID. | `{id}`: ID del tipo de venta (int) |
| `DELETE` | `/api/v1/SaleType/{id}` | Elimina un tipo de venta del sistema. | `{id}`: ID del tipo de venta (int) |

---
> **Nota de Seguridad:** La mayoría de los endpoints de gestión requieren autenticación mediante un token JWT enviado en el encabezado `Authorization: Bearer <token>`.





## Como ejecutar este Software?
1. Clone este repositorio en su maquína local.
2. Dirijase a `src/presentation/RealStateApp/appsettings.json` y a `src/presentation/RealStateApi/appsettings.json` y rellene las secciones de `connection strings`, `MailSettings` y `JWTsettings`.
3. Ya habiendo rellenado los connection strings con la información necesaria para conectarse a su BD, y teniendo dicha BD ya en servicio, proceda a ejecutar el comando `dotnet ef database update` tanto en el proyecto de `source/infrastructure/RealStateApp.Infrastructure.Persistence` como en `source/infrastructure/RealStateApp.Infrastructure.identity` para ejecutar todas las migraciones de la base de datos;
4. En este punto, la aplicacion ya esta en condiciones para ejecutarse. Ejecute el comando `dotnet run` especificando con el parametro `--project` el proyecto a ejecutar (sea el Web APP o la API) o ejecute la aplicación en el IDE de preferencia.

Nota: tiene que tener instalado el Runtime de .NET 9.0 para este proyecto.