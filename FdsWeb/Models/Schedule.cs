using System;
using System.ComponentModel.DataAnnotations;

namespace FdsWeb.Models{
    public class Schedule{
        public virtual int Id { get; set; }

        [DataType(DataType.DateTime)]
        public virtual DateTime DateTime { get; set; }

        public virtual int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}