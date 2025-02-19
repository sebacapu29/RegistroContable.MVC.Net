using RegistroContable.Entities;
using RegistroContable.MVC.Models;

namespace RegistroContable.MVC.Helpers
{
    public static class MapperHelper
    {
        public static TipoCuentas MappTipoCuentaVMToDTO(TipoCuentaViewModel tipoCuentaViewModel) 
            => new TipoCuentas { Nombre = tipoCuentaViewModel.Nombre, Orden = tipoCuentaViewModel.Orden, UsuarioId = tipoCuentaViewModel.UsuarioId };
        
        public static IEnumerable<TipoCuentaViewModel> MappTipoCuentaDTOToVM(IEnumerable<TipoCuentas> tipoCuentas) =>
            tipoCuentas.Select(tc => new TipoCuentaViewModel { Id = tc.Id, Nombre = tc.Nombre, Orden = tc.Orden, UsuarioId = tc.UsuarioId });

        public static Cuenta MappCuentaVMToDTO(CuentaViewModel cuentaViewModel) 
            => new Cuenta { Balance = cuentaViewModel.Balance, Descripcion = cuentaViewModel.Descripcion, Id = cuentaViewModel.Id, Nombre = cuentaViewModel.Nombre, TipoCuentaId = cuentaViewModel.TipoCuentaId};
        public static CuentaViewModel MappCuentaDTOToVM(Cuenta cuentaDto)
            => new CuentaViewModel { Balance = cuentaDto.Balance, Descripcion = cuentaDto.Descripcion, Id = cuentaDto.Id, Nombre = cuentaDto.Nombre, TipoCuentaId = cuentaDto.TipoCuentaId };

    }
}
