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
        private readonly BaseRepository<EventAttendance> _eventAtRepository;


        public EventManager()
        {
            _eventRepository = new BaseRepository<Event>();
            _eventAtRepository = new BaseRepository<EventAttendance>();
        }

        
        public IEnumerable<Event> GetEventsByGroupId(int groupId)
        {
            return _eventRepository._table.Where(e => e.groupId == groupId).ToList();
        }
        public IEnumerable<EventAttendance> GetEventAttendanceByUserId(int? userId)
        {
            return _eventAtRepository.GetAll().Where(m => m.userId == userId).ToList();
        }
 
       

        public List<Event> GetAllEvents()
        {
            return _eventRepository.GetAll();
        }
        public List<EventAttendance> GetAllEventAttendance()
        {
            return _eventAtRepository.GetAll();
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
      
        public ErrorCode MarkEventAsGoing(EventAttendance  ev, ref string errMsg)
        {
            try
            {
                var existingAttendance = _eventAtRepository._table.FirstOrDefault(a => a.eventId == ev.eventId && a.userId == ev.userId);
                if(existingAttendance == null)
                {
                    return _eventAtRepository.Create(ev, out errMsg);

                }
                else if (existingAttendance.status == 1)
                {
                    errMsg = "You have already marked yourself as 'Going' for this event.";
                    return ErrorCode.AlreadyExists;
                }
                else
                {
                    // Update the existing record to mark as "Going"
                    existingAttendance.status = 1;
                    return _eventAtRepository.Update(existingAttendance.id, existingAttendance, out errMsg); // Use your repository's Update method
                }
            }
            catch (Exception ex)
            {
                // Log error
                // logger.LogError(ex, "Error creating group");
                errMsg = ex.Message;
                return ErrorCode.Error;
            }
        }
        public ErrorCode UpdateEventStatus(int id, ref string errMsg)
        {
            try
            {
                // Retrieve the membership record by its ID
                var events = _eventRepository._table.FirstOrDefault(m => m.id == id);
                if (events != null)
                {
                    // Update the membership status
                    events.status = 1;
                    return _eventRepository.Update(id, events, out errMsg);
                }
                else
                {
                    errMsg = "Membership not found.";
                    return ErrorCode.Error;
                }
            }
            catch (Exception ex)
            {
                // Log exception if necessary
                errMsg = ex.Message;
                return ErrorCode.Error;
            }
        }
    }
}