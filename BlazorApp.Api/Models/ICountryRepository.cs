using BlazorApp.Shared.Domain;

namespace BlazorApp.Api.Models
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAllCountries();

        Country GetCountryById(int countryId);
    }
}