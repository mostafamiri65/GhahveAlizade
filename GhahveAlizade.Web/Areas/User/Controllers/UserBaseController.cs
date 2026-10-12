using GhahveAlizade.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

/// <summary>
/// BaseController برای همه‌ی Controllerهای User Area (پنل کاربری).
/// نیاز به احراز هویت داره.
/// </summary>
[Area("User")]
[Authorize]
[Route("user/[controller]/[action]")]
public abstract class UserBaseController : BaseController
{
}