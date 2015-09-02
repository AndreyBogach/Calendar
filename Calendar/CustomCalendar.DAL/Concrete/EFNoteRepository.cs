using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CustomCalendar.DAL.Entities;
using CustomCalendar.DAL.Interface;

namespace CustomCalendar.DAL.Concrete
{
    public class EFNoteRepository : INoteRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Note> Notes
        {
            get { return context.Notes; }
        }

        public Note DeleteNote(int noteId)
        {
            Note dbEntry = context.Notes.Find(noteId);

            if (dbEntry != null)
            {
                context.Notes.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public Note Get(int id)
        {
            return context.Notes.Find(id);
        }

        public IEnumerable<Note> GetAll()
        {
            return context.Notes;
        }

        public IEnumerable<Note> GetNotesOfDate(DateTime date)
        {
            return context.Notes.Where(x=>x.Date == date).OrderBy(p=>p.NoteStart);
        }

        public void SaveNote(Note note)
        {
            if (note.NoteId == 0)
            {
                context.Notes.Add(note);
            }

            else
            {
                Note dbEntry = context.Notes.Find(note.NoteId);
                if (dbEntry != null)
                {
                    dbEntry.Date = note.Date;
                    dbEntry.NoteStart = note.NoteStart;
                    dbEntry.NoteFinish = note.NoteFinish;
                    dbEntry.NoteText = note.NoteText;
                }
            }

            context.SaveChanges();
        }
    }
}
