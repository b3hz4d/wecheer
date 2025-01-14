using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WeCheerImageApp.Api.Models;

namespace WeCheerImageApp.Api.Services
{
    public interface IImageEventService
    {
        void AddEvent(ImageEvent imageEvent);
        ImageEvent GetLatestEvent();
        int GetEventCountLastHour();
    }

    public class ImageEventService : IImageEventService
    {
        private readonly ConcurrentQueue<ImageEvent> _events;

        public ImageEventService()
        {
            _events = new ConcurrentQueue<ImageEvent>();
        }

        public void AddEvent(ImageEvent imageEvent)
        {
            imageEvent.ReceivedAt = DateTime.UtcNow;
            _events.Enqueue(imageEvent);
        }

        public ImageEvent GetLatestEvent()
        {
            return _events.LastOrDefault();
        }

        public int GetEventCountLastHour()
        {
            var hourAgo = DateTime.UtcNow.AddHours(-1);
            return _events.Count(e => e.ReceivedAt >= hourAgo);
        }
    }
} 