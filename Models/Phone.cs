using System.Text.Json.Serialization;

namespace ConsolePhoneStore.Models
{
   
    /// Clase que representa un teléfono móvil en el catálogo de la tienda.
    /// Contiene información sobre el producto como marca, modelo, precio y stock disponible.
   
    public class Phone
    {
        // Propiedades del teléfono
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        // Stock es modificable porque disminuye cuando se hacen compras
        public int Stock { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }

      
        /// Constructor del teléfono que valida los datos de entrada.
        /// Asegura que el precio sea positivo y el stock no sea negativo.
        /// JsonConstructor permite la deserialización JSON sin constructor sin parámetros.
       
        [JsonConstructor]
        public Phone(int id, string brand, string model, decimal price, int stock)
        {
            // Validar que el precio sea válido (mayor que 0)
            if (price <= 0)
                throw new ArgumentOutOfRangeException("El precio debe ser mayor que 0");

            // Validar que el stock no sea negativo
            if (stock < 0)
                throw new ArgumentOutOfRangeException("El stock no puede ser negativo");

            // Asignar los valores a las propiedades
            Id = id;
            Brand = brand;
            Model = model;
            Price = price;
            Stock = stock;
            ReleaseDate = DateTime.Now;
            IsActive = true;
        }

        
        /// Reduce el stock del teléfono cuando se realiza una compra.
        /// Valida que la cantidad sea válida antes de restar.
       
        public void ReduceStock(int quantity)
        {
            if (quantity <= 0 || quantity > Stock)
                throw new ArgumentOutOfRangeException("Cantidad inválida");

            Stock -= quantity;
        }
    }
}
