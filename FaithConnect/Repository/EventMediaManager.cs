using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Models;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class EventMediaManager
    {
        private readonly BaseRepository<EventMedia> _eventMediaRepo;

        public EventMediaManager()
        {
            _eventMediaRepo = new BaseRepository<EventMedia>();
        }

       
        public List<EventMedia> ListMediaByEventId(int? eventId)
        {
            if (eventId == null)
                return new List<EventMedia>();

            return _eventMediaRepo._table.Where(m => m.eventId == eventId).ToList();
        }

       
        public ErrorCode CreateEventMedia(EventMedia media, ref string err)
        {
            return _eventMediaRepo.Create(media, out err);
        }

       
        public ErrorCode UpdateEventMedia(EventMedia media, ref string err)
        {
            return _eventMediaRepo.Update(media.id, media, out err);
        }

      
        public ErrorCode DeleteEventMedia(int? id, ref string err)
        {
            return _eventMediaRepo.Delete(id, out err);
        }
    }
}