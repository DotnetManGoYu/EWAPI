using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace EWA.Sugar
{
    public static class SqlsugarSetup
    {
        public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration, string dbName = "db_master")
        {
            var configConnection = new ConnectionConfig()
            {
                DbType = SqlSugar.DbType.MySql,
                ConnectionString = configuration.GetConnectionString(dbName),
                IsAutoCloseConnection = true,
            };
            SqlSugarScope sqlSugar = new SqlSugarScope(configConnection);
            services.AddSingleton<ISqlSugarClient>(sqlSugar);
        }
    }
}