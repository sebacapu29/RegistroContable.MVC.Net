using RegistroContable.Domain;

namespace RegistroContable.Infraestructura.Interfaces
{
    public interface IRepositorioTipoCuentas
    {
        Task Crear(TipoCuentas tipoCuentas);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuentas>> ObtenerTodas(int usuarioId);
    }
}
