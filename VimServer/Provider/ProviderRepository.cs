using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace VimServer
{
    public class ProviderRepository
    {
        static readonly Lazy<IReadOnlyCollection<Provider.Provider>> providers = new Lazy<IReadOnlyCollection<Provider.Provider>>(() =>
        {
            var assembly = Assembly.GetEntryAssembly();
            using var resourceStream = assembly.GetManifestResourceStream("VimServer.Provider.providers.json");
            var providers = JsonSerializer.DeserializeAsync<Provider.Provider[]>(resourceStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).GetAwaiter().GetResult();
            return providers;
        });

        public IQueryable<Provider.Provider> GetProviders() => providers.Value.AsQueryable();
    }
}
