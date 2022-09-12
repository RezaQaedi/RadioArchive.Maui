namespace RadioArchive.Maui
{
    public static class RelationalExtensions
    {

        public static MauiAppBuilder AddClientDataStore(this MauiAppBuilder construction) 
        {
            // Inject our SQLite EF data store
            construction.Services.AddDbContext<ClientDataStoreDbContext>(ServiceLifetime.Transient);

            // Add client data store for easy access/use of the backing data store
            // Make it scoped so we can inject the scoped DbContext
            construction.Services.AddTransient<IClientDataStore, ClientDataStore>();

            // Return framework for chaining
            return construction;
        }
    }
}
