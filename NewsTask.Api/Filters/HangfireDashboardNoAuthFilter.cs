using Hangfire.Dashboard;

namespace NewsTask.Api.Filters
{
    public class HangfireDashboardNoAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
        {
            return true;
        }
    }
}
