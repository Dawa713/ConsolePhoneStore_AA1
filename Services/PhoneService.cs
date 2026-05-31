using ConsolePhoneStore.Models;
using ConsolePhoneStore.Utils;

namespace ConsolePhoneStore.Services
{
    /// Servicio de gestión del catálogo de teléfonos.
    /// Proporciona métodos para obtener, buscar y añadir productos al catálogo.
    /// Utiliza un patrón estático para mantener los datos en memoria.
    public static class PhoneService
    {
        // Lista estática que almacena todos los teléfonos cargados desde JSON
        private static List<Phone> phones = FileService.LoadPhones();
        // Contador de IDs para asignar nuevos IDs únicos a teléfonos
        private static int nextId = phones.Any() ? phones.Max(p => p.Id) + 1 : 1;

        /// Obtiene la lista completa de todos los teléfonos disponibles.
        public static List<Phone> GetAll()
        {
            return phones;
        }

        /// Busca teléfonos por marca.
        /// Realiza una búsqueda insensible a mayúsculas/minúsculas.
        public static List<Phone> SearchByBrand(string brand)
        {
            // Usa LINQ Where para filtrar y ToList() para devolver una lista
            // ToLower() en ambas cadenas para hacer la búsqueda insensible a mayúsculas
            return phones
                .Where(p => p.Brand.ToLower().Contains(brand.ToLower()))
                .ToList();
        }

        /// Obtiene un teléfono específico por su ID.
        /// Devuelve null si no existe el teléfono.
        public static Phone? GetById(int id)
        {
            return phones.FirstOrDefault(p => p.Id == id);
        }

        /// Añade un nuevo teléfono al catálogo.
        /// Valida que los datos sean correctos antes de añadir.
        public static void AddPhone(string brand, string model, decimal price, int stock)
        {
            // Validar que el precio sea positivo
            if (price <= 0)
                throw new ArgumentException("El precio debe ser mayor que 0");

            // Validar que el stock no sea negativo
            if (stock < 0)
                throw new ArgumentException("El stock no puede ser negativo");

            // Crear nuevo teléfono con el siguiente ID disponible
            var newPhone = new Phone(nextId++, brand, model, price, stock);
            // Añadir a la lista en memoria
            phones.Add(newPhone);
            // Persistir los cambios en el archivo JSON
            FileService.SavePhones(phones);
        }

        /// Guarda la lista actual de teléfonos en el archivo JSON.
        /// Se utiliza cuando se modifica el stock después de una compra.
        public static void SavePhonesToFile()
        {
            FileService.SavePhones(phones);
        }
    }
}
