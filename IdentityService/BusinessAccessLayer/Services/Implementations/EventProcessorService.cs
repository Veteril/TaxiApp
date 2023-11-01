using AutoMapper;
using BAL.Dtos;
using BAL.Dtos.RatingDtos;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class EventProcessorService : IEventProcessorService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessorService(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType) 
            {
                case EventType.OrderMarkPublished:
                    AddUserRating(message);
                    break;
                default: 
                    break;
            }
        }

        private EventType DetermineEvent(string message)
        {
            var eventType = JsonSerializer.Deserialize<EventDto>(message);

            switch (eventType.Event) 
            {
                case "OrderMarkPublished":
                    return EventType.OrderMarkPublished;
                default:
                    return EventType.Undetermined;
            }
        }

        private async void AddUserRating(string ratingPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                var ratingPublishedDto = JsonSerializer.Deserialize<UserRatingPublishedDto>(ratingPublishedMessage);

                var userRating = _mapper.Map<UserRating>(ratingPublishedDto);
                userRating.Id = Guid.NewGuid();

                await repo.CreateUserRatingAsync(userRating);
                repo.SaveChanges();
            }
        }
    }

    enum EventType 
    {
        OrderMarkPublished,
        Undetermined
    }
}
