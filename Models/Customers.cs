using System.Text.Json.Serialization;

namespace ConsolePhoneStore.Models
{
    
    /// Clase que representa un cliente/usuario de la tienda.
    /// Almacena la información personal y de autenticación del usuario.
    public class Customer
    {
        // Propiedades del cliente
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // ADMIN o CLIENT
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

      
        /// Constructor del cliente que valida los datos de entrada.
        /// El rol por defecto es "CLIENT" si no se especifica.
        /// JsonConstructor permite la deserialización JSON sin constructor sin parámetros.
       
        [JsonConstructor]
        public Customer(int id, string name, string email, string password, string role = "CLIENT")
        {
            // Validar que el nombre no esté vacío y tenga máximo 10 caracteres
            if (string.IsNullOrWhiteSpace(name) || name.Length > 10)
                throw new ArgumentException("El nombre es obligatorio y máximo 10 caracteres");

            // Validar que el email tenga un formato válido
            if (!EsEmailValido(email))
                throw new ArgumentException("Email no válido");

            // Validar que la contraseña tenga al menos 6 caracteres
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres");

            // Asignar los valores a las propiedades
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Role = role;
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        /// Método privado para validar el formato del email.
        /// Comprueba que contenga @ y al menos un punto (.).
        private bool EsEmailValido(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }
    }
}
