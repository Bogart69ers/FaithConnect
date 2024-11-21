using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;

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

        public ErrorCode CreateEvent(Event ev, ref string errMsg)
        {
            try
            {
                ev.eventId = Utilities.gUid;
                ev.date_created = DateTime.Now;
                ev.status = 0;
                return _eventRepository.Create(ev, out errMsg);
            }
            catch (Exception ex)
            {
                // Log error
                // logger.LogError(ex, "Error creating group");
                errMsg = ex.Message;
                return ErrorCode.Error;
            }
        }
    }
}