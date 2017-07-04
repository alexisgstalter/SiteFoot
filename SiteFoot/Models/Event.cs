using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFoot.Models
{
    public class Event
    {
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string start;

        public string Start
        {
            get { return start; }
            set { start = value; }
        }
        private string end;

        public string End
        {
            get { return end; }
            set { end = value; }
        }
        private bool allDay;

        public bool AllDay
        {
            get { return allDay; }
            set { allDay = value; }
        }
        public Event()
        {

        }
    }
}