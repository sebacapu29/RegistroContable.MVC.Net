namespace RegistroContable.Infraestructura.Interfaces
{
    public interface IRepositorioUsuarios
    {
        Task<int> ObtenerUsuarioId();
    }
}
