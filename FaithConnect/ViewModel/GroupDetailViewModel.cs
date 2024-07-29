using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.ViewModel
{
    public class GroupDetailViewModel
    {
        public Groups Group { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Forum> Forums { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}