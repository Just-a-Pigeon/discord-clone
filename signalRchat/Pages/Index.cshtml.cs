using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace signalRchat.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        OnPostRedirect();
    }

    public IActionResult OnPostRedirect()
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToPage("Login");

        return Page();

    }   
}