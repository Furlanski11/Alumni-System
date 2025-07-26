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
			try
			{
				if (!ModelState.IsValid)
					return View(model);

				await _eventService.AddAsync(model);
				TempData["SuccessMessage"] = "Успешно създадено събитие.";
				return RedirectToAction(nameof(Index));
			}

			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "Грешка при създаване на събитие. Моля опитайте отново!");
				return View(model);
			}
		}

		// GET: /Event/Edit/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var e = await _eventService.GetByIdAsync(id);

			if (e == null) return NotFound();

			return View(e);
		}

		// POST: /Event/Edit/5
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EventViewModel model)
		{
			try
			{
				if (id != model.Id)
					return BadRequest();
				if (!ModelState.IsValid)
					return View(model);

				await _eventService.UpdateAsync(model);
				TempData["SuccessMessage"] = "Събитието беше редактирано успешно!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "Грешка по време на редакция на събитието. Моля опитайте отново!");
				return View(model);
			}
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
			try
			{
				await _eventService.DeleteAsync(id);
				TempData["SuccessMessage"] = "Успешно изтриване на събитие.";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Грешка при изтриване на събитие. Моля опитайте отново!";
				return RedirectToAction(nameof(Index));
			}
		}
	}
}
