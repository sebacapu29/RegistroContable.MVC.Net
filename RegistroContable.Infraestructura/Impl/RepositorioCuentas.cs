using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RegistroContable.Entities;
using RegistroContable.Infraestructura.Interfaces;
using System.Data;

namespace RegistroContable.Infraestructura.Impl
{
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string? _connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            _connectionString = configuration!.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Cuenta>(@"SELECT c.Id, c.Nombre, c.Balance, c.Descripcion, tc.Nombre As TipoCuenta
                                                        FROM Cuentas c
                                                        INNER JOIN TipoCuentas tc ON tc.Id = c.TipoCuentaId
                                                        WHERE tc.UsuarioId = @UsuarioId
                                                        ORDER BY tc.Orden", new { usuarioId });
            throw new NotImplementedException();
        }

        public async Task Crear(Cuenta cuenta)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Cuentas (Nombre, TipoCuentaId, Descripcion, Balance) VALUES (@Nombre, @TipoCuentaId, @Descripcion, @Balance) SELECT SCOPE_IDENTITY();", cuenta);
                cuenta.Id = id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
