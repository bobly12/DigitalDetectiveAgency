using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Services.Interfaces;
using DigitalDetectiveAgency.ViewModels;

namespace DigitalDetectiveAgency.Controllers
{
    [Authorize] // Enforces login using our smart cookie routing rules
    public class DashboardController : Controller
    {
        private readonly ICaseService _caseService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ICaseService caseService, UserManager<ApplicationUser> userManager)
        {
            _caseService = caseService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var availableCases = await _caseService.GetAvailableCasesAsync();

            // Mapping your real data into the presentation layer ViewModel
            var viewModel = new DashboardViewModel
            {
                PlayerName = user.UserName ?? "Detective",
                TotalCasesAttempted = 0, // Hook up to ICaseAttemptRepository later
                TotalCasesSolved = 0,    // Hook up to ICaseAttemptRepository later
                TotalScore = 0,          // Hook up to ICaseAttemptRepository later
                RecentCases = availableCases.Take(3).ToList() // Show top 3 cases as a preview
            };

            return View(viewModel);
        }
    }
}