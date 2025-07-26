using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlumniSystem.Controllers
{
	[Authorize]
	public class JobPostingController : Controller
	{
		private readonly IJobPostingService _jobService;

		public JobPostingController(
			IJobPostingService jobService,
			IAlumniService alumniService)
		{
			_jobService = jobService;
		}

		// GET: /JobPosting
		public async Task<IActionResult> Index()
		{
			var jobs = await _jobService.GetAllAsync();
			var vm = jobs.Select(j => new JobPostingViewModel
			{
				Id = j.Id,
				Title = j.Title,
				Company = j.Company,
				Description = j.Description,
				Location = j.Location,
				PostedOn = j.PostedOn
			});
			return View(vm);
		}

		// GET: /JobPosting/Details/5
		public async Task<IActionResult> Details(int id)
		{
			try
			{
				var jobPosting = await _jobService.GetByIdAsync(id);
				if (jobPosting == null)
				{
					return NotFound();
				}

				return View(jobPosting);
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while loading job posting details.";
				return RedirectToAction(nameof(Index));
			}
		}

		// GET: /JobPosting/Create
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			return View();
		}

		// POST: /JobPosting/Create
		[HttpPost, ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(JobPostingViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
					return View(model);

				model.PostedOn = DateTime.Now;
				await _jobService.AddAsync(model);
				TempData["SuccessMessage"] = "Job posting created successfully.";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "An error occurred while creating the job posting. Please try again.");
				return View(model);
			}
		}

		// GET: /JobPosting/Edit/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var jobPosting = await _jobService.GetByIdAsync(id);
				if (jobPosting == null)
				{
					return NotFound();
				}

				return View(jobPosting);
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while loading the job posting for editing.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: /JobPosting/Edit/5
		[HttpPost, ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id, JobPostingViewModel model)
		{
			try
			{
				if (id != model.Id)
					return BadRequest();
				if (!ModelState.IsValid)
					return View(model);

				await _jobService.UpdateAsync(model);
				TempData["SuccessMessage"] = "Job posting updated successfully.";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "An error occurred while updating the job posting. Please try again.");
				return View(model);
			}
		}

		// GET: /JobPosting/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var jobPosting = await _jobService.GetByIdAsync(id);
				if (jobPosting == null)
				{
					return NotFound();
				}

				return View(jobPosting);
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while loading the job posting for deletion.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: /JobPosting/Delete/5
		[HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				await _jobService.DeleteAsync(id);
				TempData["SuccessMessage"] = "Job posting deleted successfully.";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while deleting the job posting. Please try again.";
				return RedirectToAction(nameof(Index));
			}
		}
	}
}