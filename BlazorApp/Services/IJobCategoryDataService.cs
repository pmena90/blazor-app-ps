using BlazorApp.Shared.Domain;

namespace BlazorApp.Services
{
    public interface IJobCategoryDataService
    {
        Task<IEnumerable<JobCategory>> GetJobCategories(bool refreshRequierd = false);

        Task<JobCategory> GetJobCategory(int id);
    }
}