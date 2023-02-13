using BlazorApp.Models;
using BlazorApp.Shared.Domain;
using Blazored.LocalStorage;
using System.Text.Json;

namespace BlazorApp.Services
{
    public class JobCategoryDataService : IJobCategoryDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public JobCategoryDataService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<JobCategory> GetJobCategory(int id)
        {
            return await JsonSerializer.DeserializeAsync<JobCategory>
                 (await _httpClient.GetStreamAsync($"/api/JobCategory/{id}"),
                 new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<JobCategory>> GetJobCategories(bool refreshRequierd = false)
        {
            if (!refreshRequierd)
            {
                bool JobCategoryListExist = await _localStorageService
                    .ContainKeyAsync(LocalStorageConstants.JobCategoryListKey);

                if (JobCategoryListExist)
                {
                    return await _localStorageService
                        .GetItemAsync<List<JobCategory>>(LocalStorageConstants.JobCategoryListKey);
                }
                else
                {
                    var result = await GetJobCategoryListFromDatabaseAsync();

                    await _localStorageService.SetItemAsync(LocalStorageConstants.JobCategoryListKey, result);

                    return result;
                }
            }
            else
            {
                var result = await GetJobCategoryListFromDatabaseAsync();

                return result;
            }
        }

        private async Task<IEnumerable<JobCategory>> GetJobCategoryListFromDatabaseAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<JobCategory>>
                        (await _httpClient.GetStreamAsync($"/api/JobCategory/"),
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}