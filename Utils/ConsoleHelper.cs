namespace ConsolePhoneStore.Utils
{
    /// Clase de utilidad para operaciones seguras con la consola.
    /// Proporciona métodos que funcionan en diferentes entornos (consola redirecciona, pipes, etc).
    public static class ConsoleHelper
    {
        /// Limpia la pantalla de forma segura.
        /// Si la salida está redireccionada (pipes, archivos), no intenta limpiar para evitar errores.
        public static void SafeClear()
        {
            try
            {
                // Comprobar si la consola no está redireccionada (está en modo interactivo)
                if (!Console.IsOutputRedirected)
                {
                    Console.Clear();
                }
            }
            catch
            {
                // Si ocurre error, simplemente ignorar (puede ocurrir en algunos entornos)
            }
        }
    }
}
