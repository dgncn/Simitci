using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Simitci.Order.Entities
{
    public class DomainEvent
    {
        [Key]
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventData { get; set; }
    }
}
