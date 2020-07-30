using System;
using System.Collections.Generic;
using System.Text;

namespace IGPS.Models
{
    public enum MenuItemType
    {
        Home,
        RecordList,
        Setting,
        About
    }

    public class MainMenuItem
    {
        public MenuItemType Id { get; set; }
        public string Title { get; set; }
    }
}
