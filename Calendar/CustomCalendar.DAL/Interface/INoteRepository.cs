using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomCalendar.DAL.Entities;

namespace CustomCalendar.DAL.Interface
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetAll();
        IEnumerable<Note> GetNotesOfDate(DateTime date);
        Note Get(int id);
        IQueryable<Note> Notes { get; }
        void SaveNote(Note note);
        Note DeleteNote(int noteId);
    }
}
