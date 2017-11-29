using System;
using System.Collections.Generic;
using System.Text;

namespace TrailsNative
{
    public class Page
    {
        public string name { get; set; }
        public string slug { get; set; }
        public string coverImage { get; set; }
        public string body { get; set; }
    }

    public class News
    {
        public string name { get; set; }
        public string slug { get; set; }
        public string coverImage { get; set; }
        public string summary { get; set; }
        public string body { get; set; }
    }

    public class Event
    {
        public string name { get; set; }
        public string slug { get; set; }
        public string date { get; set; }
        public string coverImage { get; set; }
        public string summary { get; set; }
        public string body { get; set; }
    }

    public class Contents
    {
        public List<Page> pages { get; set; }
        public List<News> news { get; set; }
        public List<Event> events { get; set; }
    }

    public class RootObject
    {
        public string mapData { get; set; }
        public Contents contents { get; set; }
    }
}
