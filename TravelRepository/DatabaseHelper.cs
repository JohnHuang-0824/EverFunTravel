using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using TravelRepository.Table;

namespace TravelRepository
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            var conn = new SqlConnection(this._connectionString);
            return conn;
        }

        public T? Query<T>(string sql, object? param = null, CommandType? commandType = null)
        {
            using var conn = GetConnection();
            return conn.QueryFirstOrDefault<T>(sql, param, commandType: commandType);
        }

        public async Task<T?> QueryAsync<T>(string sql, object? param = null, CommandType? commandType = null)
        {
            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<T>(sql, param, commandType: commandType);
        }
        public List<T> QueryList<T>(string sql, object? param = null, CommandType? commandType = null)
        {
            using var conn = GetConnection();
            return conn.Query<T>(sql, param, commandType: commandType).ToList();
        }

        public async Task<List<T>> QueryListAsync<T>(string sql, object? param = null, CommandType? commandType = null)
        {
            using var conn = GetConnection();
            return (await conn.QueryAsync<T>(sql, param, commandType: commandType)).ToList();
        }
        public bool Execute(string sql, object? param = null)
        {
            using var conn = GetConnection();
            return conn.Execute(sql, param) > 0;
        }

        public async Task<bool> ExecuteAsync(string sql, object? param = null)
        {
            using var conn = GetConnection();
            return await conn.ExecuteAsync(sql, param) > 0;
        }
        public T Get<T>(dynamic id) where T : class
        {
            using var conn = GetConnection();
            return SqlMapperExtensions.Get<T>(conn, id);
        }
        public long Insert<T>(T entity, string? uid, IDbConnection conn, IDbTransaction? transaction = null) where T : class
        {
            if (entity is _BaseTable bt)
            {
                if (!string.IsNullOrEmpty(uid))
                {
                    bt.CreateId = uid;
                }
                bt.CreateTime = DateTime.Now;
                bt.ModifyTime = DateTime.Now;
                bt.ModifyId = bt.ModifyId;
            }
            return SqlMapperExtensions.Insert(conn, new T[] { entity }, transaction);
        }
    }
}
