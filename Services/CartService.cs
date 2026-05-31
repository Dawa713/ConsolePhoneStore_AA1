using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Services
{
    
    /// Servicio de gestión del carrito de compra.
    /// Maneja la adición, eliminación y cálculo de totales de productos en el carrito.
    
    public static class CartService
    {
        // Lista estática que almacena los artículos en el carrito como tuplas de (Teléfono, Cantidad)
        private static List<(Phone phone, int quantity)> cart = new();

       
        /// Añade un teléfono al carrito o incrementa su cantidad si ya existe.
        /// Si el producto ya está en el carrito, suma la cantidad.
       
        public static void AddToCart(Phone phone, int quantity)
        {
            var item = cart.FirstOrDefault(c => c.phone.Id == phone.Id);

            if (item.phone != null)
            {
                // Si el producto ya está en el carrito, actualizar cantidad
                cart.Remove(item);
                cart.Add((phone, item.quantity + quantity));
            }
            else
            {
                // Si no existe, añadir nuevo artículo
                cart.Add((phone, quantity));
            }
        }

        
        /// Obtiene la lista completa del carrito actual.
       
        public static List<(Phone phone, int quantity)> GetCart()
        {
            return cart;
        }

       
        /// Elimina una cantidad específica de un producto del carrito.
        /// Si la cantidad a eliminar es igual o mayor al total, elimina el artículo completamente.
       
        public static void RemoveFromCart(int phoneId, int quantity)
        {
            var item = cart.FirstOrDefault(i => i.phone.Id == phoneId);

            if (item.phone == null)
                throw new Exception("Producto no encontrado en el carrito");

            if (quantity >= item.quantity)
            {
                // Eliminar el producto completamente
                cart.Remove(item);
            }
            else
            {
                // Reducir la cantidad del producto
                int index = cart.IndexOf(item);
                cart[index] = (item.phone, item.quantity - quantity);
            }
        }

       
        /// Vacía completamente el carrito eliminando todos los productos.
     
        public static void ClearCart()
        {
            cart.Clear();
        }

    
        /// Calcula el subtotal del carrito (precio × cantidad de cada producto).
        /// No incluye IVA, solo suma los montos base.
       
        public static decimal CalculateSubtotal()
        {
            decimal total = 0;

            foreach (var item in cart)
            {
                total += item.phone.Price * item.quantity;
            }

            return total;
        }
    }
}
