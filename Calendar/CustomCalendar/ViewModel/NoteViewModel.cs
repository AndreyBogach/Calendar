using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCalendar.ViewModel
{
    public class NoteViewModel: IDataErrorInfo
    {
        public int NoteId { get; set; }
        public string Note { get; set; }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Note")
                {
                    if (string.IsNullOrWhiteSpace(Note))
                        result = "Please enter your note";
                    if (string.Concat(Note).Length > 50)
                        result = "Your event cannot contain more than 50 symbols";
                }
                return result;
            }
        }
    }
}
