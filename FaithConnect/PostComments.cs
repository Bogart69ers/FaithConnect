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
    
    public partial class PostComments
    {
        public int id { get; set; }
        public Nullable<int> postId { get; set; }
        public Nullable<int> groupId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
        public string comment { get; set; }
    
        public virtual Groups Groups { get; set; }
        public virtual Post Post { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
