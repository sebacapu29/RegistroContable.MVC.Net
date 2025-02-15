using RegistroContable.Infraestructura.Interfaces;

namespace RegistroContable.Infraestructura.Impl
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        public Task<int> ObtenerUsuarioId()
        {
            return Task.FromResult(1);
        }
    }
}
