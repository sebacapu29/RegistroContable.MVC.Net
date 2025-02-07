using RegistroContable.Domain;

namespace RegistroContable.Infraestructura.Interfaces
{
    public interface IRepositorioTipoCuentas
    {
        Task Crear(TipoCuentas tipoCuentas);
    }
}
