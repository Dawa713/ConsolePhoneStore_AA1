using ConsolePhoneStore.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsolePhoneStore.Utils
{
    /// <summary>
    /// Servicio de persistencia de datos a archivos JSON y txt.
    /// Maneja la carga y guardado de clientes, teléfonos y registros de compras.
    /// </summary>
    public static class FileService
    {
        // Rutas de los archivos de datos
        private static readonly string customersPath = "Data/customers.json";
        private static readonly string phonesPath = "Data/phones.json";

        // ==================== GESTIÓN DE CLIENTES ====================

 
        /// Carga la lista de clientes desde el archivo JSON.
        /// Si el archivo no existe, devuelve una lista vacía.
        /// Usa PropertyNameCaseInsensitive para permitir variaciones en mayúsculas.
 
        public static List<Customer> LoadCustomers()
        {
            List<Customer> customers = new();

            if (!File.Exists(customersPath))
                return customers;

            try
            {
                string json = File.ReadAllText(customersPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                customers = JsonSerializer.Deserialize<List<Customer>>(json, options) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar clientes: {ex.Message}");
            }

            return customers;
        }

 
        /// Guarda la lista completa de clientes en el archivo JSON.
        /// Sobrescribe el archivo anterior con los datos actualizados.
        /// Utiliza WriteIndented para formato legible.
 
        public static void SaveCustomers(List<Customer> customers)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(customers, options);
                File.WriteAllText(customersPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar clientes: {ex.Message}");
            }
        }

        // ==================== GESTIÓN DE TELÉFONOS ====================

 
        /// Carga la lista de teléfonos desde el archivo JSON.
        /// Si el archivo no existe, devuelve una lista vacía.
        /// Usa PropertyNameCaseInsensitive para deserialización flexible.
 
        public static List<Phone> LoadPhones()
        {
            List<Phone> phones = new();

            if (!File.Exists(phonesPath))
                return phones;

            try
            {
                string json = File.ReadAllText(phonesPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                phones = JsonSerializer.Deserialize<List<Phone>>(json, options) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar teléfonos: {ex.Message}");
            }

            return phones;
        }

 
        /// Guarda la lista completa de teléfonos en el archivo JSON.
        /// Sobrescribe el archivo anterior con los datos actualizados del catálogo.
        /// Utiliza WriteIndented para formato legible.
 
        public static void SavePhones(List<Phone> phones)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(phones, options);
                File.WriteAllText(phonesPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar teléfonos: {ex.Message}");
            }
        }

        // ==================== GESTIÓN DE COMPRAS ====================

        /// Registra una compra completada en el archivo purchases.txt.
        /// Append=true permite agregar múltiples compras al mismo archivo.
        /// Registra fecha, cliente, detalles de productos y total.
        public static void SavePurchase(
            string customerEmail,
            List<(Phone phone, int quantity)> cart,
            decimal total)
        {
            string path = "purchases.txt";

            using StreamWriter writer = new(path, append: true);

            decimal subtotal = cart.Sum(i => i.phone.Price * i.quantity);
            decimal iva = subtotal * 0.21m;

            writer.WriteLine("=================================");
            writer.WriteLine($"  CONSOLE PHONE STORE");
            writer.WriteLine($"  Fecha: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            writer.WriteLine($"  Cliente: {customerEmail}");
            writer.WriteLine("---------------------------------");

            foreach (var item in cart)
            {
                decimal lineTotal = item.phone.Price * item.quantity;
                writer.WriteLine($"  {item.phone.Brand} {item.phone.Model}");
                writer.WriteLine($"    {item.quantity} x {item.phone.Price:F2}€ = {lineTotal:F2}€");
            }

            writer.WriteLine("---------------------------------");
            writer.WriteLine($"  Subtotal:    {subtotal:F2}€");
            writer.WriteLine($"  IVA (21%):   {iva:F2}€");
            writer.WriteLine($"  TOTAL:       {total:F2}€");
            writer.WriteLine("=================================");
            writer.WriteLine();
        }
    }
}
