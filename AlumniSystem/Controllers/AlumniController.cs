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
		private const int PageSize = 5;

		public AlumniController(IAlumniService _service)
		{
			service = _service;
		}

		public async Task<IActionResult> Index(int page = 1)
		{
			try
			{
				var alumnis = (await service.GetAllAsync()).ToList();
				var count = alumnis.Count;

				var items = alumnis
					.Skip((page - 1) * PageSize)
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
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Грешка при зареждане на данните. Моля опитайте отново!";
				return View(new PagedResult<AlumniViewModel> { Items = new List<AlumniViewModel>(), PageNumber = 1, PageSize = PageSize, TotalItems = 0 });
			}
		}

		public async Task<IActionResult> Details(string id)
		{
			try
			{
				if (string.IsNullOrEmpty(id))
				{
					return BadRequest();
				}

				var alumni = await service.GetByIdAsync(id);
				if (alumni == null)
				{
					return NotFound();
				}

				// Security check: Users can view any profile (details are public)
				// But we'll add a flag to show if they're viewing their own profile
				var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				ViewBag.IsOwnProfile = currentUserId == id;
				ViewBag.IsAdmin = User.IsInRole("Admin");

				return View(alumni);
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Грешка при зареждане на данни за Алумни. Моля опитайте отново!";
				return RedirectToAction(nameof(Index));
			}
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create()
		{
			// This is now only available for Admin users to manually create Alumni profiles
			var vm = new AlumniViewModel();
			return View(vm);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AlumniViewModel model)
		{
			try
			{
				// Admin can specify any UserId when creating Alumni profiles manually
				if (string.IsNullOrEmpty(model.UserId))
				{
					ModelState.AddModelError(nameof(model.UserId), "User ID е нужно за ръчно създаване на Алумни!.");
					return View(model);
				}

				// Check if Alumni profile already exists for this user
				var existingAlumni = await service.GetByIdAsync(model.UserId);
				if (existingAlumni != null)
				{
					ModelState.AddModelError(string.Empty, "Вече съществува Алумни за този потребител.");
					return View(model);
				}

				if (!ModelState.IsValid)
				{
					return View(model);
				}

				await service.AddAsync(model);
				TempData["SuccessMessage"] = "Успешно създаден Алумни профил от Админ";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, $"Изникна грешка при създаване на Алумни профил: {ex.Message}");
				return View(model);
			}
		}

		[Authorize]
		public async Task<IActionResult> Edit()
		{
			// Users can only edit their own profile
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var alumni = await service.GetByIdAsync(userId);

			if (alumni == null)
			{
				TempData["InfoMessage"] = "Все още няма Алумни профил. Трябваше да е създаден при регистрация.";
				return RedirectToAction(nameof(Index));
			}

			return View(alumni);
		}

		[Authorize]
		public async Task<IActionResult> MyProfile()
		{
			// Convenient redirect to user's own profile
			return RedirectToAction(nameof(Edit));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditUser(string id)
		{
			// Only admins can edit other users' profiles
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var alumni = await service.GetByIdAsync(id);
			if (alumni == null)
			{
				return NotFound();
			}

			ViewBag.IsAdminEdit = true;
			ViewBag.EditingUserId = id;
			return View("Edit", alumni);
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(AlumniViewModel model, string? userId = null)
		{
			try
			{
				var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(currentUserId))
				{
					return Forbid();
				}

				// Security check: Regular users can only edit their own profile
				if (!User.IsInRole("Admin"))
				{
					// Force the UserId to be the current user's ID (prevent tampering)
					model.UserId = currentUserId;

					var existingAlumni = await service.GetByIdAsync(currentUserId);
					if (existingAlumni == null)
					{
						return NotFound("Алумни профил не е намерен.");
					}

					// Ensure they can only update their own record
					model.Id = existingAlumni.Id;
				}
				else
				{
					// Admin editing: Use the userId parameter if provided, otherwise use model.UserId
					string targetUserId = !string.IsNullOrEmpty(userId) ? userId : model.UserId;

					if (string.IsNullOrEmpty(targetUserId))
					{
						ModelState.AddModelError(nameof(model.UserId), "User ID е задължително.");
						return View(model);
					}

					// Set the correct UserId in the model
					model.UserId = targetUserId;

					var existingAlumni = await service.GetByIdAsync(targetUserId);
					if (existingAlumni == null)
					{
						return NotFound("Алумни профил не е намерен.");
					}

					model.Id = existingAlumni.Id;
					ViewBag.IsAdminEdit = true;
					ViewBag.EditingUserId = targetUserId;
				}

				// Remove UserId and Id from model validation since we set them manually
				ModelState.Remove(nameof(model.UserId));
				ModelState.Remove(nameof(model.Id));

				if (!ModelState.IsValid)
				{
					return View(model);
				}

				await service.UpdateAsync(model);

				if (User.IsInRole("Admin") && model.UserId != currentUserId)
				{
					TempData["SuccessMessage"] = "Алумни профил е редактиран успешно от Админ!";
				}
				else
				{
					TempData["SuccessMessage"] = "Профилът ви е редактиран успешно!";
				}

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, $"Грешка при редактиране на алумни профил: {ex.Message}");
				return View(model);
			}
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var alumni = await service.GetByIdAsync(id);
			if (alumni == null)
			{
				return NotFound();
			}

			return View(alumni);
		}

		[HttpPost, ActionName("DeleteConfirmed")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			try
			{
				if (string.IsNullOrEmpty(id))
				{
					return BadRequest();
				}

				await service.DeleteAsync(id);
				TempData["SuccessMessage"] = "Успешно изтрит Алумни профил.";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Грешка при изтриване на Алумни профил. Моля опитайте отново!";
				return RedirectToAction(nameof(Index));
			}
		}
	}
}