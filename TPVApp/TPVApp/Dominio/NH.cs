using NHibernate;
using NHibernate.Cfg;
using System;

public static class NH
{
    private static ISessionFactory _sessionFactory;

    public static ISessionFactory SessionFactory
    {
        get
        {
            // Verificar si la fábrica de sesiones ya está configurada
            if (_sessionFactory == null)
            {
                try
                {
                    var configuration = new Configuration();
                    configuration.Configure(); // Carga la configuración desde hibernate.cfg.xml
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al configurar NHibernate: {ex.Message}");
                    throw; // Lanza la excepción para que pueda ser manejada por el código superior
                }
            }
            return _sessionFactory;
        }
    }

    public static ISession OpenSession()
    {
        return SessionFactory.OpenSession(); // Abre una nueva sesión
    }

    // Método para liberar recursos de la fábrica de sesiones cuando ya no se necesite
    public static void Dispose()
    {
        _sessionFactory?.Dispose();
    }
}
