using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlumniSystem.Controllers
{
	[Authorize]
	public class AlumniController : Controller
	{
		private readonly IAlumniService service;
		private const int PageSize = 10;

		public AlumniController(IAlumniService _service)
		{
			service = _service;
		}

		public async Task<IActionResult> Index(int page = 1)
		{
			var alumnis = (await service.GetAllAsync()).ToList();
			var count = alumnis.Count;

			var items = alumnis
				.Skip((page-1) * PageSize)
				.Take(PageSize);

			var vm = new PagedResult<AlumniViewModel>
			{
				Items = items,
				PageNumber = page,
				PageSize = PageSize,
				TotalItems = count
			};

			return View(vm);
		}

		public async Task<IActionResult> Details(string id)
		{
			var alumni = await service.GetByIdAsync(id);

			return View(alumni);
		}

		public IActionResult Create()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
			var vm = new AlumniViewModel { UserId = userId };
			return View(vm);
		}

		[HttpPost]
		[Authorize(Roles = "Admin, User")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AlumniViewModel model)
		{
			model.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();

			ModelState.Remove(nameof(model.UserId));

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await service.AddAsync(model);
			return RedirectToAction(nameof(Index));
		}

		[Authorize]
		public async Task<IActionResult> Edit()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var alumni = await service.GetByIdAsync(userId);

			if (alumni == null)
			{
				return NotFound();
			}

			return View(alumni);
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(AlumniViewModel model)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var vm = await service.GetByIdAsync(userId);
			if (vm == null)
			{
				return Forbid();
			}

			if(!User.IsInRole("Admin") && vm.UserId != userId)
			{
				return Forbid();
			}
			await service.UpdateAsync(model);
			return View(model);
		}

		public async Task<IActionResult> Delete(string id)
		{
			var alumni = await service.GetByIdAsync(id);

			return View(alumni);
		}

		[HttpPost, ActionName("DeleteConfirmed")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			await service.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}