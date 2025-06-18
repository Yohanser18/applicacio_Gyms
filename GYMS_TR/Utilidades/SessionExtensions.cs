using System.Text.Json;

namespace GYMS_TR.Utilidades
{
    public static class SessionExtensions // Esto que cuando le demos al voton de agregar al carrito//
    {
        //El metodo set para configural la seciones//
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        // El metodo get para obtener el valor de la seciones//
        public static T Get<T>(this ISession session, string key) 
        { 
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

    }
}
