using System;
using System.Collections.Generic;

namespace AwoAppServices.Models
{
    public partial class Notes
    {
        public int NoteId { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
    }
}
