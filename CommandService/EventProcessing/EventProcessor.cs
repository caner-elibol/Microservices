using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scope;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper )
        {
            _scope = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string msg)
        {
            var eventType = DetermineEvent(msg);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    addPlatform(msg);
                    break;

                default:
                    break;
            }
        }
        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine($"--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Publish Event Detected");
                    return EventType.PlatformPublished;

                default:
                    Console.WriteLine("--> Couldn't determine event type");
                    return EventType.Undetermined;
                    

            }
        }
        private void addPlatform(string pPMsg)
        {
            using (var scope = _scope.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var pPDto = JsonSerializer.Deserialize<PlatformPublishedDto>(pPMsg);

                try
                {
                    var plat = _mapper.Map<Platform>(pPDto);
                    if (!repo.ExternalPlatformExist(plat.ExternalID))
                    {
                        Console.WriteLine("--> Platform added!..");
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exist..");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Couldn't add Platform to DB : {ex.Message}");
                    throw;
                }

            }
        }

    }
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
