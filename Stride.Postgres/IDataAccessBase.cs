using System.Data;


namespace Stride.Postgres
{
    public interface IDataAccessBase
    {
        string ConnString { get; set; }

        Task<int> ExecuteNonQueryAsync(string queryToExec, object? parameters,
            CommandType commandType = CommandType.StoredProcedure, int? timeOut = 0);
        Task<T> ExecuteScalarAsync<T>(string queryToExec, object? parameters, CommandType commandType = CommandType.StoredProcedure);
        Task<TModel> InsertAsync<TModel>(TModel payload) where TModel : class;
        Task<IEnumerable<T>> QueryListAsync<T>(string queryToExec, object? parameters = null, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> QueryAsListAsync<T>(string queryToExec, object? parameters = null, CommandType commandType = CommandType.StoredProcedure);
        Task<T> QuerySingleOrDefaultAsync<T>(string queryToExec, object? parameters = null,
            CommandType commandType = CommandType.StoredProcedure);

        Task<T> QueryFirstOrDefaultAsync<T>(string queryToExec, object? parameters = null,
            CommandType commandType = CommandType.StoredProcedure);
        string GenerateColumnsSelectQuery<TModel>(TModel cls, string alias = "") where TModel : class;

    }
}
