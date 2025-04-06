using Npgsql;

namespace DiscordClone.Api.Utils;

public class DataSourceBuilder 
{
    public static NpgsqlDataSource Build(IConfiguration configuration)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = configuration.GetValue<string>("Postgres:Host"),
            Port = configuration.GetValue<int>("Postgres:Port"),
            Database = configuration.GetValue<string>("Postgres:Database"),
            Username = configuration.GetValue<string>("Postgres:Username"),
            SslMode = SslMode.Prefer
        };

        var password = configuration.GetValue("Postgres:Password", string.Empty);
        if (!string.IsNullOrWhiteSpace(password))
        {
            connectionStringBuilder.Password = password;
        }
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionStringBuilder.ConnectionString);

        return dataSourceBuilder.Build();
    }
}