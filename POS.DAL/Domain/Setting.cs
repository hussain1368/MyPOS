using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class Setting
    {
        public int Id { get; set; }
        public string LayoutName { get; set; }
        public string Language { get; set; }
        public string AppTitle { get; set; }
        public byte CalendarType { get; set; }
        public bool IsActive { get; set; }
    }
}
