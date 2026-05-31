using ConsolePhoneStore.Models;
using ConsolePhoneStore.Utils;

namespace ConsolePhoneStore.Services
{
    /// <summary>
    /// Servicio de gestión de clientes/usuarios.
    /// Maneja el registro, login y recuperación de datos de usuarios.
    /// Utiliza un patrón estático para mantener los datos en memoria.
    /// </summary>
    public static class CustomerService
    {
        // Lista estática que almacena todos los clientes cargados desde JSON
        private static List<Customer> customers = FileService.LoadCustomers();
        // Contador de IDs para asignar nuevos IDs únicos a clientes
        private static int nextId = customers.Any() ? customers.Max(c => c.Id) + 1 : 1;

 
        /// Constructor estático que se ejecuta una sola vez al cargar la clase.
        /// Crea un usuario administrador por defecto si no existe.
        /// </summary>
        static CustomerService()
        {
            // Crear ADMIN por defecto si no existe uno en los datos
            if (!customers.Any(c => c.Role == "ADMIN"))
            {
                customers.Add(new Customer(
                    nextId++,
                    "admin",
                    "admin@store.com",
                    "admin123",
                    "ADMIN"
                ));
                // Guardar los cambios en el archivo JSON
                FileService.SaveCustomers(customers);
            }
        }

        /// Registra un nuevo cliente en el sistema.
        /// Valida que no exista otro usuario con el mismo email.
        public static void Register(string name, string email, string password)
        {
            // Verificar que el email no esté ya registrado
            if (customers.Any(c => c.Email == email))
                throw new ArgumentException("Ya existe un usuario con ese email");

            // Crear nuevo cliente con el siguiente ID disponible
            Customer customer = new Customer(nextId++, name, email, password);
            // Añadir a la lista en memoria
            customers.Add(customer);
            // Persistir los cambios en el archivo JSON
            FileService.SaveCustomers(customers);
        }

        /// Autentica un usuario verificando su nombre y contraseña.
        /// Devuelve el cliente si las credenciales son correctas, null si no.
        public static Customer? Login(string name, string password)
        {
            try
            {
                // Validar que los parámetros no estén vacíos
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
                    return null;

                // Buscar el cliente que coincida con el nombre y contraseña
                // También verifica que el cliente esté activo
                Customer? customer = customers.FirstOrDefault(
                    c => c.Email == name && c.Password == password && c.IsActive
                );

                return customer;
            }
            catch
            {
                // Si ocurre cualquier error, devolvemos null (credenciales inválidas)
                return null;
            }
        }

        /// Obtiene la lista completa de todos los clientes registrados.
        public static List<Customer> GetAll()
        {
            return customers;
        }
    }
}
