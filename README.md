# Compilar
dotnet build


# Ejecutar
dotnet run


# Limpiar y compilar
dotnet clean
dotnet build

# Esto construye (si no está ya), arranca el contenedor  y te conecta directamente a la consola interactiva.
docker compose run --rm phonestore
# Al salir, elimina el contenedor


**Requisitos:**
- .NET 10.0
- Windows/Linux/macOS