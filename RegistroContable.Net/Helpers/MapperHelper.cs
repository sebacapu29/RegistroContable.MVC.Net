using RegistroContable.Domain;
using RegistroContable.Net.Models;

namespace RegistroContable.MVC.Helpers
{
    public static class MapperHelper
    {
        public static TipoCuentas MappTipoCuentaVMToDTO(TipoCuentaViewModel tipoCuentaViewModel)
        {
            return new TipoCuentas { Nombre = tipoCuentaViewModel.Nombre, Orden = tipoCuentaViewModel.Orden, UsuarioId = tipoCuentaViewModel.UsuarioId };
        }
        public static IEnumerable<TipoCuentaViewModel> MappTipoCuentaDTOToVM(IEnumerable<TipoCuentas> tipoCuentas)
        {
            return tipoCuentas.Select(tc => new TipoCuentaViewModel { Id = tc.Id, Nombre = tc.Nombre, Orden = tc.Orden, UsuarioId = tc.UsuarioId });
        }
    }
}
