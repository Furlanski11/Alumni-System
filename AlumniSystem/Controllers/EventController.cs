using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlumniSystem.Controllers
{
	[Authorize]
	public class EventController : Controller
	{
		private readonly IEventService _eventService;
		private const int PageSize = 10;

		public EventController(IEventService eventService)
		{
			_eventService = eventService;
		}

		// GET: /Event
		public async Task<IActionResult> Index(int page = 1)
		{
			var events = (await _eventService.GetAllAsync()).ToList();
			var count = events.Count();

			var items = events
				.Skip((page - 1) * PageSize)
				.Take(PageSize);

			var vm = new PagedResult<EventViewModel>
			{
				Items = items,
				PageNumber = page,
				PageSize = PageSize,
				TotalItems = count
			};

			return View(vm);
		}

		// GET: /Event/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var e = await _eventService.GetByIdAsync(id);
			if (e == null) return NotFound();
			var vm = new EventViewModel
			{
				Id = e.Id,
				Title = e.Title,
				Description = e.Description,
				Date = e.Date,
				Location = e.Location
			};
			return View(vm);
		}

		// GET: /Event/Create
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			return View();
		}

		// POST: /Event/Create
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EventViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			await _eventService.AddAsync(model);
			return RedirectToAction(nameof(Index));
		}

		// GET: /Event/Edit/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var e = await _eventService.GetByIdAsync(id);
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var currentUser = await _eventService.GetByIdAsync(int.Parse(userId));
			if (e == null) return NotFound();

			if(!User.IsInRole("Admin")
				)
			{
				return Forbid();
			}

			return View(e);
		}

		// POST: /Event/Edit/5
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EventViewModel model)
		{
			if (id != model.Id)
				return BadRequest();
			if (!ModelState.IsValid)
				return View(model);

			var e = new EventViewModel
			{
				Id = model.Id,
				Title = model.Title,
				Description = model.Description,
				Date = model.Date,
				Location = model.Location
			};
			await _eventService.UpdateAsync(e);
			return RedirectToAction(nameof(Index));
		}

		// GET: /Event/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var e = await _eventService.GetByIdAsync(id);
			if (e == null) return NotFound();
			var vm = new EventViewModel
			{
				Id = e.Id,
				Title = e.Title,
				Description = e.Description,
				Date = e.Date,
				Location = e.Location
			};
			return View(vm);
		}

		// POST: /Event/Delete/5
		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _eventService.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
