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

        public async Task Actualizar(TipoCuentas tipoCuentas)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(@"UPDATE TipoCuentas 
                                           SET Nombre = @Nombre
                                            Where Id = @Id", tipoCuentas);
        }
        public async Task<TipoCuentas> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync($"SELECT Id, Nombre, Orden FROM TipoCuentas WHERE Id = @Id AND @UsuarioId = @UsuarioId;", new { id, usuarioId });
        }

        public async Task Crear(TipoCuentas tipoCuentas)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var id = await connection.QuerySingleAsync<int>($"INSERT INTO TipoCuentas (Nombre, UsuarioId, Orden) " +
                                                    $"Values (@Nombre, @UsuarioId, 0);" +
                                                    $"SELECT SCOPE_IDENTITY();", tipoCuentas);
                tipoCuentas.Id = id;
            }
            catch (Exception ex)
            {
                throw;
            }          
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>($"SELECT 1 FROM TipoCuentas WHERE Nombre = @Nombre AND @UsuarioId = @UsuarioId;", new { nombre, usuarioId });
            return existe > 0;
        }

        public async Task<IEnumerable<TipoCuentas>> ObtenerTodas(int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TipoCuentas>($"SELECT Id, Nombre, Orden FROM TipoCuentas WHERE UsuarioId = @UsuarioId;",new { usuarioId });
        }

        public async Task Borrar(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"DELETE TipoCuentas                                            
                                                Where Id = @Id", id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
