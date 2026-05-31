using System.Text.RegularExpressions;

namespace ConsolePhoneStore.Utils
{
    /// <summary>
    /// Clase de utilidad para validación segura de entradas del usuario.
    /// Proporciona métodos para leer y validar diferentes tipos de datos con restricciones.
    /// </summary>
    public static class InputValidator
    {
 
        /// Lee una cadena no vacía del usuario con límite de caracteres.
        /// Repite hasta que el usuario ingrese datos válidos.
 
        public static string ReadNonEmptyString(string message, int maxLength)
        {
            string? input;

            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("❌ No puede estar vacío");
                    continue;
                }

                if (input.Length > maxLength)
                {
                    Console.WriteLine($"❌ Máximo {maxLength} caracteres");
                    continue;
                }

                return input;

            } while (true);
        }

 
        /// Lee una contraseña con validación de longitud (mín y máx).
        /// Se usa en el registro de usuarios.
 
        public static string ReadPassword(string message, int min, int max)
        {
            string? input;

            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("❌ La contraseña no puede estar vacía");
                    continue;
                }

                if (input.Length < min || input.Length > max)
                {
                    Console.WriteLine($"❌ Debe tener entre {min} y {max} caracteres");
                    continue;
                }

                return input;

            } while (true);
        }

 
        /// Lee una contraseña sin validación de longitud.
        /// Se usa en el login para mayor flexibilidad.
 
        public static string ReadPasswordLogin(string message)
        {
            string? input;

            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("❌ La contraseña no puede estar vacía");
                    continue;
                }

                return input;

            } while (true);
        }

 
        /// Lee y valida un email usando expresión regular.
        /// Verifica formato: usuario@dominio.extensión
 
        public static string ReadValidEmail(string message)
        {
            string? input;
            // Regex para validar formato básico de email: algo@algo.algo
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("❌ El email no puede estar vacío");
                    continue;
                }

                if (!regex.IsMatch(input))
                {
                    Console.WriteLine("❌ Formato de email no válido");
                    continue;
                }

                return input;

            } while (true);
        }
    }
}
