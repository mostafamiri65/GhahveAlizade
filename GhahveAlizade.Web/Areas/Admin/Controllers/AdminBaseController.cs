using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]  
[Route("admin/[controller]/[action]")]
public abstract class AdminBaseController : MvcBaseController
{
}