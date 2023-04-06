using System.Data;

namespace TravelRepository
{
    public interface IDatabaseHelper
    {
        IDbConnection GetConnection();
        public T? Query<T>(string sql, object? param = null, CommandType? commandType = null);
        public Task<T?> QueryAsync<T>(string sql, object? param = null, CommandType? commandType = null);

        public List<T> QueryList<T>(string sql, object? param = null, CommandType? commandType = null);
        public Task<List<T>> QueryListAsync<T>(string sql, object? param = null, CommandType? commandType = null);
        public bool Execute(string sql, object? param = null);
        public Task<bool> ExecuteAsync(string sql, object? param = null);
        public T Get<T>(dynamic id) where T : class;
        public long Insert<T>(T entity, string? uid, IDbConnection conn, IDbTransaction? transaction = null) where T : class;

    }
}