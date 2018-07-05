using SwiftKampus.Models;
using SwiftKampus.Services;
using System;
using System.Collections.Generic;

namespace SwiftKampus.ViewModels
{
    public class Timetable {
        public List<List<string>> events = new List<List<String>>{
            new List<String> { "Name", "Building", "Tue Jul 03 2018 10:00:00 GMT+0100 (British Summer Time)", "Tue Jul 03 2018 10:30:00 GMT+0100 (British Summer Time)" },
            new List<String> { "Name", "Building", "Tue Jul 03 2018 10:30:00 GMT+0100 (British Summer Time)", "Tue Jul 03 2018 11:30:00 GMT+0100 (British Summer Time)" },
        };
        public List<List<string>> TimetableList()
        {
            return this.events;
        }
    }
}