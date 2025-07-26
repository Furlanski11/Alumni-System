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
			var j = await _jobService.GetByIdAsync(id);
			if (j == null) return NotFound();

			var vm = new JobPostingViewModel
			{
				Id = j.Id,
				Title = j.Title,
				Company = j.Company,
				Description = j.Description,
				Location = j.Location,
				PostedOn = j.PostedOn
			};
			return View(vm);
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
			if (!ModelState.IsValid)
				return View(model);

			var job = new JobPostingViewModel
			{
				Title = model.Title,
				Company = model.Company,
				Description = model.Description,
				Location = model.Location,
				PostedOn = model.PostedOn,
			};

			await _jobService.AddAsync(job);
			return RedirectToAction(nameof(Index));
		}

		// GET: /JobPosting/Edit/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var j = await _jobService.GetByIdAsync(id);
			if (j == null) return NotFound();

			var vm = new JobPostingViewModel
			{
				Id = j.Id,
				Title = j.Title,
				Company = j.Company,
				Description = j.Description,
				Location = j.Location,
				PostedOn = j.PostedOn,
			};
			return View(vm);
		}

		// POST: /JobPosting/Edit/5
		[HttpPost, ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id, JobPostingViewModel model)
		{
			if (id != model.Id)
				return BadRequest();
			if (!ModelState.IsValid)
				return View(model);

			var j = await _jobService.GetByIdAsync(id);
			if (j == null) return NotFound();

			j.Title = model.Title;
			j.Company = model.Company;
			j.Description = model.Description;
			j.Location = model.Location;
			j.PostedOn = model.PostedOn;

			await _jobService.UpdateAsync(j);
			return RedirectToAction(nameof(Index));
		}

		// GET: /JobPosting/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var j = await _jobService.GetByIdAsync(id);
			if (j == null) return NotFound();

			var vm = new JobPostingViewModel
			{
				Id = j.Id,
				Title = j.Title,
				Company = j.Company,
				Description = j.Description,
				Location = j.Location,
				PostedOn = j.PostedOn
			};
			return View(vm);
		}

		// POST: /JobPosting/Delete/5
		[HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var j = await _jobService.GetByIdAsync(id);
			if (j == null) return NotFound();

			await _jobService.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}