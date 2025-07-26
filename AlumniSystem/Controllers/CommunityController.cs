using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlumniSystem.Controllers
{
	[Authorize]
	public class CommunityController : Controller
	{
		private readonly ICommunityService communityService;
		private readonly IAlumniService alumniService;
		private const int PageSize = 10;

		public CommunityController(ICommunityService _communityService, IAlumniService _alumniService)
		{
			communityService = _communityService;
			alumniService = _alumniService;
		}

		// GET: /Community
		public async Task<IActionResult> Index(int page = 1)
		{
			var communities = (await communityService.GetAllAsync()).ToList();
			var count = communities.Count;

			var items = communities
				.Skip((page - 1) * PageSize)
				.Take(PageSize);

			var vm = new PagedResult<CommunityViewModel>
			{
				Items = items,
				PageNumber = page,
				PageSize = PageSize,
				TotalItems = count
			};
			return View(vm);
		}

		// GET: /Community/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var viewModel = await communityService.GetByIdAsync(id);
			if (viewModel == null)
				return NotFound();

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var currentAlumni = await alumniService.GetByIdAsync(userId);
			ViewBag.IsMember = currentAlumni != null 
				&& viewModel.Members.Any(m => m.Id == currentAlumni.Id);

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Join(int Id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			await communityService.JoinAsync(Id, userId);

			return RedirectToAction(nameof(Details), new { id = Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Leave(int Id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			await communityService.LeaveAsync(Id, userId);

			return RedirectToAction(nameof(Details), new { id = Id });
		}

		// GET: /Community/Create
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			var model = new CommunityViewModel
			{
				Members = new List<AlumniViewModel>()
			};
			return View(model);
		}

		// POST: /Community/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(CommunityViewModel model)
		{
			if (ModelState.IsValid)
			{
				await communityService.AddAsync(model);
				return RedirectToAction(nameof(Index));
			}
			return View(model);
		}

		// GET: /Community/Edit/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var community = await communityService.GetByIdAsync(id);
			if (community == null) 
				return NotFound();

			return View(community);
		}

		// POST: /Community/Edit/5
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, CommunityViewModel model)
		{
			if (id != model.Id) 
				return BadRequest();

			if (ModelState.IsValid)
			{
				await communityService.UpdateAsync(model);
				return RedirectToAction(nameof(Index));
			}

			return View(model);
		}

		// GET: /Community/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var c = await communityService.GetByIdAsync(id);
			if (c == null) return NotFound();

			var vm = new CommunityViewModel
			{
				Id = c.Id,
				Name = c.Name,
				Description = c.Description
			};
			return View(vm);
		}

		// POST: /Community/Delete/5
		[Authorize(Roles = "Admin")]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await communityService.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}