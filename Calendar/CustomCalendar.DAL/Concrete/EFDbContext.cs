using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomCalendar.DAL.Entities;

namespace CustomCalendar.DAL.Concrete
{
    public class EFDbContext: DbContext
    {
        static EFDbContext()
        {
            Database.SetInitializer<EFDbContext>(new MyContextInitializer());
        }

        public EFDbContext()
           : base("EFDbContext")
        {

        }

        public DbSet<Note> Notes { get; set; }
    }

    public class MyContextInitializer : CreateDatabaseIfNotExists<EFDbContext>
    {
        protected override void Seed(EFDbContext db)
        {
            Note note1 = new Note();
            note1.NoteId = 1;
            note1.Date = DateTime.Today.Date;
            note1.NoteStart = DateTime.Now.ToString("hh:mm");
            note1.NoteFinish = DateTime.Now.ToString("hh:mm");
            note1.NoteText = "Test";

            db.Notes.Add(note1);
            db.SaveChanges();
            //base.Seed(db);
        }
    }
}
