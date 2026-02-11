using Accounts.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Species.Infrastructure.DbContexts;
using Volunteers.Infrastructure.DbContexts;

public static class AppExtensions 
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var contextTypes = new[]
        {
            typeof(SpeciesReadDbContext),
            typeof(SpeciesWriteDbContext),
            typeof(VolunteersReadDbContext),
            typeof(VolunteersWriteDbContext),
            typeof(AccountsDbContext)
        };

        var firstContext = scope.ServiceProvider.GetService(contextTypes[0]) as DbContext;
        if (firstContext != null)
        {
            var databaseCreator = firstContext.Database.GetService<IRelationalDatabaseCreator>();
            if (databaseCreator != null && !await databaseCreator.ExistsAsync())
            {
                app.Logger.LogInformation("Database does not exist. Creating database...");
                await databaseCreator.CreateAsync();
                app.Logger.LogInformation("Database created successfully");
            }
        }

        foreach (var contextType in contextTypes)
        {
            var context = scope.ServiceProvider.GetService(contextType) as DbContext;
            if (context == null)
            {
                app.Logger.LogWarning("DbContext {ContextType} not found", contextType.Name);
                continue;
            }

            app.Logger.LogInformation("Applying migrations for {ContextName}...", contextType.Name);
            try
            {
                await context.Database.MigrateAsync();
                app.Logger.LogInformation("Migrations applied successfully for {ContextName}", contextType.Name);
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "Error applying migrations for {ContextName}", contextType.Name);
                throw;
            }
        }
    }
}