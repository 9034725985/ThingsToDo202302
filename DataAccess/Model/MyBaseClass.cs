using System;
using System.Diagnostics;

namespace DataAccess.Model
{
    public class MyBaseClass
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedByName { get; set; } = "";
        public string ModifiedByName { get; set; } = "";
        public Stopwatch? Stopwatch { get; set; }
    }
}