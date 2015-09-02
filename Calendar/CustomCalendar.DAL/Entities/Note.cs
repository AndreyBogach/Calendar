using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCalendar.DAL.Entities
{
    public class Note
    {
        public int NoteId { get; set; }

        public DateTime Date { get; set; }

        public string NoteStart { get; set; }

        public string NoteFinish { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string NoteText { get; set; }
    }
}
