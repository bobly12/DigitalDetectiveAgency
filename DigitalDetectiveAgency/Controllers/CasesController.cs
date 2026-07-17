using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DigitalDetectiveAgency.Services.Interfaces;
using DigitalDetectiveAgency.ViewModels;

namespace DigitalDetectiveAgency.Controllers
{
    [Authorize]
    public class CaseController : Controller
    {
        private readonly ICaseService _caseService;

        public CaseController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        // GET: /Case (Displays all available active cases)
        public async Task<IActionResult> Index()
        {
            var cases = await _caseService.GetAvailableCasesAsync();
            
            var viewModel = new CaseListViewModel
            {
                Cases = cases
            };

            return View(viewModel);
        }

        // GET: /Case/Details/{id} (Displays pre-investigation briefing)
        public async Task<IActionResult> Details(int id)
        {
            var caseDetails = await _caseService.GetCaseDetailsAsync(id);
            if (caseDetails == null) return NotFound();

            var viewModel = new CaseDetailsViewModel
            {
                CaseDetails = caseDetails,
                AlreadyAttempted = false, // Connect to historical attempts data block later
                BestScoreEarned = 0
            };

            return View(viewModel);
        }
    }
}