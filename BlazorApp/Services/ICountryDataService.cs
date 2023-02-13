using BlazorApp.Shared.Domain;

namespace BlazorApp.Services
{
    public interface ICountryDataService
    {
        Task<IEnumerable<Country>> GetCountries(bool refreshRequierd = false);

        Task<Country> GetCountry(int id);
    }
}