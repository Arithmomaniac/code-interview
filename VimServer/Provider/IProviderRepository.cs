using System.Linq;

namespace VimServer
{
    public interface IProviderRepository
    {
        IQueryable<Provider.Provider> GetProviders();
    }
}