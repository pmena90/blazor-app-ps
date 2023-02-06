using BlazorApp.Shared.Domain;

namespace BlazorApp.Api.Models
{
    public interface IJobCategoryRepository
    {
        IEnumerable<JobCategory> GetAllJobCategories();

        JobCategory GetJobCategoryById(int jobCategoryId);
    }
}