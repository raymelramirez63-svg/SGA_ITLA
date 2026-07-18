namespace SGA_ITLA.Domain.Enums
{
    public enum EstadoConductor
    {
        Disponible = 1,    // Listo para que se le asigne un viaje
        EnRuta = 2,        // Actualmente manejando un autobús
        DePermiso = 3,     // Permiso médico o vacaciones
        Suspendido = 4,    // Sanción administrativa
        Inactivo = 5       // Ya no labora en la institución
    }
}