using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.ViewModels;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;

namespace AlumniSystem.Core.Services
{
	public class EventService : IEventService
	{
		private readonly IEventRepository eventRepository;

		public EventService(IEventRepository _eventRepository)
		{
			eventRepository = _eventRepository;
		}

		public async Task AddAsync(EventViewModel model)
		{
			var ev = new Event
			{	
				Title = model.Title,
				Description = model.Description,
				Date = model.Date,
				Location = model.Location,
			};

			await eventRepository.AddAsync(ev);
		}

		public async Task DeleteAsync(int id)
		{
			var ev = eventRepository.GetByIdAsync(id);

			if (ev == null)
			{
				throw new ArgumentNullException("No event found!");
			}
			await eventRepository.DeleteAsync(id);
		}

		public async Task<IEnumerable<EventViewModel>> GetAllAsync()
		{
			var events = await eventRepository.GetAllAsync();

			return events.Select(e => new EventViewModel
			{
				Id = e.Id,
				Title = e.Title,
				Description = e.Description,
				Date = e.Date,
				Location = e.Location
			}).ToList();
		}

		public async Task<EventViewModel> GetByIdAsync(int id)
		{
			var e = await eventRepository.GetByIdAsync(id);

			return new EventViewModel
			{
				Title = e.Title,
				Description = e.Description,
				Date = e.Date,
				Location = e.Location,
			};
		}

		public async Task UpdateAsync(EventViewModel model)
		{
			var ev = new Event
			{
				Date = model.Date,
				Title = model.Title,
				Description = model.Description,
				Location = model.Location,
			};

			await eventRepository.UpdateAsync(ev);
		}
	}
}