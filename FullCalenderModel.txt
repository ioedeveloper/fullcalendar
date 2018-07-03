using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwiftKampus.Models
{
    public class FullCalender
    {
        public int FullCalenderId { get; set; }
        public string Building { get; set; }
        public string CourseTitle { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}