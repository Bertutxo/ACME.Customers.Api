namespace ACME.Customers.Core.Interfaces
{
    /// <summary>
    /// Define el contrato para la unidad de trabajo, encargado de
    /// coordinar las transacciones de repositorios y asegurar
    /// la consistencia en la base de datos.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Persiste todos los cambios pendientes en el contexto.
        /// </summary>
        /// <returns>
        /// Una tarea que representa la operación asíncrona
        /// y devuelve el número de registros afectados.
        /// </returns>
        Task<int> CompleteAsync();
    }
}