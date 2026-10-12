using Hangfire.Dashboard;

namespace GhahveAlizade.Web.Filters;

public class HangfireAdminAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context)
	{
		var httpContext = context.GetHttpContext();

		// فقط کاربران Authenticated با نقش SuperAdmin یا Admin
		return httpContext.User.Identity?.IsAuthenticated == true
			&& (httpContext.User.IsInRole("SuperAdmin") || httpContext.User.IsInRole("Admin"));
	}
}