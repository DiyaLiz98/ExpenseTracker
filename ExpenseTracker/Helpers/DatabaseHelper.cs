using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

public static class DatabaseHelper
{
    private static string _connectionString;

    
    public static void Initialize(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public static async Task<List<T>> ExecuteQueryAsync<T>(string storedProcName) where T : new()
    {
        return await ExecuteQueryAsync<T>(storedProcName, null);
    }

    public static async Task<List<T>> ExecuteQueryAsync<T>(string storedProcName, Dictionary<string, object> parameters) where T : new()
    {
        List<T> results = new List<T>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        T obj = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            PropertyInfo prop = typeof(T).GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                            if (prop != null && !reader.IsDBNull(i))
                            {
                                object value = reader.GetValue(i);
                                prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType), null);
                            }
                        }
                        results.Add(obj);
                    }
                }
            }
        }

        return results;
    }

    public static async Task<T> ExecuteScalarAsync<T>(string storedProcName, Dictionary<string, object> parameters)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                var result = await cmd.ExecuteScalarAsync();
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }
    }
    
    public static async Task ExecuteNonQueryAsync(string storedProcName, Dictionary<string, object> parameters)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
