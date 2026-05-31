## REPOSITORIO —> https://github.com/Dawa713/aa1dwec

**ConsolePhoneStore** es una tienda de teléfonos móviles desarrollada como aplicación de consola interactiva en C# con .NET 10. Gestiona un catálogo de teléfonos, clientes registrados y el historial de compras, con persistencia en ficheros JSON.

---

### 1. Arquitectura de menús y flujo de navegación

La aplicación se estructura en dos zonas diferenciadas según el estado de sesión del usuario. La navegación se realiza mediante menús numerados en consola.

**Zona pública** (sin sesión iniciada):
- Opción 1 → Ver catálogo → submenú: listar todos / buscar por marca / volver
- Opción 2 → Registro de nuevo cliente
- Opción 3 → Inicio de sesión
- Opción 0 → Salir de la aplicación

**Zona privada** (tras iniciar sesión — se muestra el nombre del usuario):
- Opción 1 → Añadir producto al carrito (muestra catálogo con stock)
- Opción 2 → Ver carrito con subtotal
- Opción 3 → Finalizar compra (subtotal + IVA 21% + confirmación)
- Opción 3 *(solo ADMIN)* → Añadir nuevo artículo al catálogo
- Opción 0 → Cerrar sesión

**Validaciones de entrada en todos los formularios:**
| Campo | Regla |
|-------|-------|
| Nombre | Obligatorio, máximo 10 caracteres |
| Email | Formato válido `usuario@dominio.ext` (Regex) |
| Contraseña (registro) | Entre 6 y 10 caracteres |
| Cantidad (carrito) | No puede superar el stock disponible |
| ID teléfono | Debe existir en el catálogo |

---

### 2. Modelo de datos con persistencia en JSON

**2 entidades principales:**

```
Customer (1) ──────────────── (N) Purchase ──────────────── (N) Phone
```

Los datos se almacenan en ficheros JSON en la carpeta `Data/` y se cargan en memoria al iniciar la aplicación. Los cambios se persisten inmediatamente tras cada operación.

**Customer** (7 atributos):
- `Id` (int), `Name` (string, máx 10 chars), `Email` (string), `Password` (string)
- `Role` (string: ADMIN / CLIENT), `CreatedAt` (DateTime), `IsActive` (bool)

**Phone** (7 atributos):
- `Id` (int), `Brand` (string), `Model` (string), `Price` (decimal)
- `Stock` (int, decrece con cada compra), `ReleaseDate` (DateTime), `IsActive` (bool)

**Carrito (en memoria, no persistido):**
- Lista de tuplas `(Phone phone, int quantity)` gestionada por `CartService`
- Se vacía automáticamente al finalizar la compra o cerrar sesión

Las compras confirmadas se guardan en `purchases.txt` con fecha, cliente, líneas de producto y total con IVA.

---

### 3. Búsqueda y filtrado

**Catálogo de teléfonos (zona pública):**
- Listar todos: muestra todos los teléfonos con ID, marca, modelo y precio
- Por marca: búsqueda insensible a mayúsculas/minúsculas usando `Contains()`

```
Buscar por marca: samsung
→ Samsung Galaxy S25 - 999,99 €
```

**Carrito (zona privada):**
- Ver carrito: muestra cada línea con `marca modelo × cantidad = subtotal`
- Calcular total: subtotal + IVA 21% con confirmación antes de procesar

Ningún filtro requiere coincidencia exacta. La búsqueda por marca devuelve todos los resultados que contengan el término introducido.

---

### 4. Autenticación con roles

**Flujo de autenticación:**
1. El usuario introduce su email y contraseña
2. `CustomerService.Login()` busca en la lista en memoria por `Email + Password + IsActive`
3. Si coincide, devuelve el objeto `Customer` y queda almacenado como `clienteLogueado`
4. El menú cambia automáticamente al modo privado mostrando el nombre del usuario
5. Si el rol es `ADMIN`, aparece la opción adicional de gestión del catálogo

**Protección por roles:**
```csharp
// Comprobación de rol admin en el menú
Menu.MostrarMenuPrivado(clienteLogueado.Name, clienteLogueado.Role == "ADMIN")

// La opción "Añadir al catálogo" solo aparece si esAdmin = true
if (esAdmin)
    Console.WriteLine("3. Añadir nuevo artículo al catálogo (ADMIN)");
```

**Usuario administrador por defecto** (se crea automáticamente si no existe ningún ADMIN en el JSON):
```
Email:      admin@store.com
Contraseña: admin123
Rol:        ADMIN
```

---

### 5. Contenedores Docker

```
Dockerfile   → imagen multi-etapa: build (SDK 10.0) + runtime (10.0)
docker-compose.yml → arranca la app con stdin/tty habilitados
```

La app requiere modo interactivo obligatoriamente (`-it`), ya que toda la interacción es por consola.

**Arranque con docker compose (recomendado):**
```bash
docker compose run --rm phonestore
```

**Arranque manual:**
```bash
docker build -t consolephonestore .
docker run -it \
  -v ${PWD}/data/Data:/app/Data \
  -v ${PWD}/data/purchases.txt:/app/purchases.txt \
  consolephonestore
```

**Volúmenes para persistencia de datos:**
| Volumen host | Ruta en contenedor | Contenido |
|---|---|---|
| `./data/Data/` | `/app/Data/` | `customers.json`, `phones.json` |
| `./data/purchases.txt` | `/app/purchases.txt` | Historial de compras |

Sin volúmenes, los datos se pierden al eliminar el contenedor. Con volúmenes, permanecen en el host entre reinicios.

**Limpieza de recursos Docker no usados:**
```powershell
# Windows
.\docker-cleanup.ps1

# Linux / Mac
bash docker-cleanup.sh
```

---

### 6. Estructura del proyecto

```
ConsolePhoneStore/
├── Models/
│   ├── Customers.cs        # Entidad cliente con validaciones
│   └── Phone.cs            # Entidad teléfono con validaciones
├── Services/
│   ├── CustomerService.cs  # Registro, login, gestión de usuarios
│   ├── PhoneService.cs     # Catálogo, búsqueda, añadir productos
│   └── CartService.cs      # Carrito: añadir, quitar, calcular total
├── Utils/
│   ├── Menu.cs             # Renderizado de menús de consola
│   ├── ConsoleHelper.cs    # SafeClear() para entornos sin terminal
│   ├── InputValidator.cs   # Validación de strings, email, contraseñas
│   └── FileService.cs      # Carga/guardado JSON y purchases.txt
├── Data/
│   ├── customers.json      # Persistencia de clientes
│   └── phones.json         # Persistencia del catálogo
├── Program.cs              # Punto de entrada y bucle principal
├── Dockerfile              # Imagen multi-etapa para contenedor
├── docker-compose.yml      # Configuración para ejecución interactiva
└── docker-cleanup.ps1/sh   # Scripts de limpieza de recursos Docker
```

---

### 7. Ejecución local

```bash
# Requisitos: .NET 10.0 SDK
dotnet restore
dotnet run
```
