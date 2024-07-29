using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.Repository
{
    public class EventManager
    {
        private readonly BaseRepository<Event> _eventRepository;

        public EventManager()
        {
            _eventRepository = new BaseRepository<Event>();
        }

        public IEnumerable<Event> GetEventsByGroupId(int groupId)
        {
            return _eventRepository._table.Where(e => e.groupId == groupId).ToList();
        }

        public void AddEvent(Event ev, ref string errorMsg)
        {
            _eventRepository.Create(ev, out errorMsg);
        }
    }
}