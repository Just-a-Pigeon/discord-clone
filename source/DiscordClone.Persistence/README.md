### Prerequisites
- Make sure you have the `dotnet ef` tools installed locally (see [here](https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools)).

### Adding a new migration
1) Update the source code of your domain entities (.cs files) and the associated `IEntityTypeConfiguration`
2) Generate a new EFCore migration

```
cd .\Source\DiscordClone.Persistence
dotnet ef migrations add <MyNewMigration>
```
3) Create a new `.sql` file for your migration under `AiService.DbMigrator\Scripts`. Make the Build Action is set to **Embedded Resource**.
4) Give it the same name as your `.cs` version of the migration. For example: `20231002145136_MyNewMigration.cs` -> `20231002145136_MyNewMigration.sql`
4) Generate a SQL variant of the migration, replace _MostRecentMigration_ with the name of the most recent migration. Copy the output to the `.sql` file.
```
 dotnet ef migrations script <MostRecentMigration> --no-transactions
```

5) You can safely remove the parts in the SQL script related to `[__EFMigrationsHistory]` since DbUp maintains its own list of executed scripts
6) You can apply the migration to your local database by running the **DbMigrator** project.
