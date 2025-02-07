using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RegistroContable.Domain;
using RegistroContable.Infraestructura.Interfaces;

namespace RegistroContable.Infraestructura.Impl
{
    public class RepositorioTipoCuentas : IRepositorioTipoCuentas
    {
        private readonly string? _connectionString;
        public RepositorioTipoCuentas(IConfiguration configuration)
        {
            _connectionString = configuration!.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuentas tipoCuentas)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var id = await connection.QuerySingleAsync<int>($"@INSERT INTO TipoCuentas (Nombre, UsuarioId, Orden) " +
                                                    $"Values (@Nombre, @UsuarioId, 0);" +
                                                    $"SELECT SCOPE_IDENTITY();", tipoCuentas);
                tipoCuentas.Id = id;
            }
            catch (Exception ex)
            {
                throw;
            }          
        }
    }
}
