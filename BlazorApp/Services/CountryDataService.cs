using BlazorApp.Models;
using BlazorApp.Shared.Domain;
using Blazored.LocalStorage;
using System.Text.Json;

namespace BlazorApp.Services
{
    public class CountryDataService : ICountryDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public CountryDataService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<Country> GetCountry(int id)
        {
            return await JsonSerializer.DeserializeAsync<Country>
                 (await _httpClient.GetStreamAsync($"/api/country/{id}"),
                 new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Country>> GetCountries(bool refreshRequierd = false)
        {
            if (!refreshRequierd)
            {
                bool countryListExist = await _localStorageService
                    .ContainKeyAsync(LocalStorageConstants.CountryListKey);

                if (countryListExist)
                {
                    return await _localStorageService
                        .GetItemAsync<List<Country>>(LocalStorageConstants.CountryListKey);
                }
                else
                {
                    var result = await GetCountryListFromDatabaseAsync();

                    await _localStorageService.SetItemAsync(LocalStorageConstants.CountryListKey, result);

                    return result;
                }
            }
            else
            {
                var result = await GetCountryListFromDatabaseAsync();

                return result;
            }
        }

        private async Task<IEnumerable<Country>> GetCountryListFromDatabaseAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Country>>
                        (await _httpClient.GetStreamAsync($"/api/country/"),
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}