using System;
using System.Security.Cryptography;
using System.Text;

namespace CapaPresentacion.Utilidades
{
    public class Encriptacion
    {
        // 🔹 Genera hash con salt (nuevo formato)
        public static string EncriptarContraseña(string contraseña)
        {
            // Generar un salt aleatorio de 16 bytes
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Generar hash usando PBKDF2 (más seguro que SHA256 directo)
            var pbkdf2 = new Rfc2898DeriveBytes(contraseña, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 256 bits

            // Guardar en formato "salt:hash"
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        // 🔹 Verifica contraseña con salt
        public static bool VerificarContraseña(string contraseñaIngresada, string claveAlmacenada)
        {
            try
            {
                string[] partes = claveAlmacenada.Split(':');
                if (partes.Length != 2)
                    return false; // formato inválido

                byte[] salt = Convert.FromBase64String(partes[0]);
                byte[] hashAlmacenado = Convert.FromBase64String(partes[1]);

                // Generar hash con el mismo salt
                var pbkdf2 = new Rfc2898DeriveBytes(contraseñaIngresada, salt, 10000, HashAlgorithmName.SHA256);
                byte[] hashIngresado = pbkdf2.GetBytes(32);

                // Comparar byte a byte
                for (int i = 0; i < hashAlmacenado.Length; i++)
                {
                    if (hashAlmacenado[i] != hashIngresado[i])
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 🔹 Compatibilidad con contraseñas antiguas (solo SHA256 plano)
        public static string EncriptarContraseñaAntigua(string contraseña)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
