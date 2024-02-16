using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class AccountController : Controller
{
    [Authorize]
    public IActionResult Logout()
    {
        var idTokenHint = User.FindFirst("id_token")?.Value;

        // Вихід з IdentityServer
        return SignOut(new AuthenticationProperties
        {
            RedirectUri = $"{SD.AuthAPIBase}/connect/endsession?id_token_hint={idTokenHint}&post_logout_redirect_uri={Url.Action("Index", "Home", null, Request.Scheme)}"
        }, "Cookies", "oidc");
    }

    public IActionResult Login(string returnUrl = "/")
    {
        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl });
    }
}