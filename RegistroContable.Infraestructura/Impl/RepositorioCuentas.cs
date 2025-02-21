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

        public async Task Actualizar(Cuenta cuenta)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(@"UPDATE Cuentas SET Nombre = @Nombre, TipoCuentaId = @TipoCuentaId, Descripcion = @Descripcion, Balance = @Balance) WHERE Id = @Id;", cuenta);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(@"DELETE Cuentas WHERE Id = @Id;", new {id});
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Cuenta>(@"SELECT c.Id, c.Nombre, c.Balance, c.Descripcion, tc.Nombre As TipoCuenta
                                                        FROM Cuentas c
                                                        INNER JOIN TipoCuentas tc ON tc.Id = c.TipoCuentaId
                                                        WHERE tc.UsuarioId = @UsuarioId
                                                        ORDER BY tc.Orden", new { usuarioId });
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

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(@"SELECT c.Id, c.Nombre, c.Balance, c.Descripcion, tc.Id
                                                        FROM Cuentas c
                                                        INNER JOIN TipoCuentas tc ON tc.Id = c.TipoCuentaId
                                                        WHERE tc.UsuarioId = @UsuarioId AND c.Id = @Id
                                                        ORDER BY tc.Orden", new { id, usuarioId }) ?? new();
        }
    }
}
