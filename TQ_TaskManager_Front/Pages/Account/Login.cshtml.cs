using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TQ_TaskManager_Back.Dtos;
using TQ_TaskManager_Front.Services;

namespace TQ_TaskManager_Front.Pages.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginRequestModel User { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _authService.LoginAsync(User);
                // Redirige a una página protegida después del login
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
