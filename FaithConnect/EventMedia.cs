//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FaithConnect
{
    using System;
    using System.Collections.Generic;
    
    public partial class EventMedia
    {
        public int id { get; set; }
        public Nullable<int> eventId { get; set; }
        public string mediaType { get; set; }
        public string mediaFile { get; set; }
        public Nullable<System.DateTime> uploadedAt { get; set; }
        public Nullable<int> uploadedBy { get; set; }
    
        public virtual Event Event { get; set; }
    }
}
