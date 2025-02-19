using RegistroContable.Entities;

namespace RegistroContable.Infraestructura.Interfaces
{
    public interface IRepositorioCuentas
    {
        Task Crear(Cuenta cuenta);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
    }
}
