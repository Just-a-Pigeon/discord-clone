using System.Reflection;
using DbUp;
using DiscordClone.DbMigrator;
using Microsoft.Data.SqlClient;
using Npgsql;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:l} {Properties}{NewLine}{Exception}")
    .CreateLogger();

var connectionString = args[0];
var builder = new NpgsqlConnectionStringBuilder(connectionString);

var upgrader =
    DeployChanges.To
        .PostgresqlDatabase(builder.ConnectionString)
        .WithScriptsAndCodeEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .WithScriptNameComparer(new ProperScriptAndCodeComparer())
        .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Log.Logger.ForContext<Program>().Error(result.Error, "Migration failed");
#if DEBUG
    Console.ReadLine();
#endif
    return -1;
}

Log.Logger.ForContext<Program>().Information("Migration was successful!");
return 0;