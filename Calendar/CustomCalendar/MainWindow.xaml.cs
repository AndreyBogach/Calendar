using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomCalendar.DAL.Concrete;
using CustomCalendar.DAL.Entities;
using CustomCalendar.DAL.Interface;
using CustomCalendar.ViewModel;
using Microsoft.SqlServer.Server;

namespace CustomCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly INoteRepository _repository;
        private int _noOfErrorsOnScreen = 0;
        private NoteViewModel myNote;
        private Note responseOfDB;

        public MainWindow()
        {
            InitializeComponent();
            
            myNote = new NoteViewModel();
            tbxNote.DataContext = myNote;
            tbxNoteEdit.DataContext = myNote;
            this._repository = new EFNoteRepository();

            dataGrid_load(dataGrid1, DateTime.Today);

            for (int k = 0; k < 24; k++)
            {
                cboHoursEventStart.Items.Add(k.ToString("00"));
                cboHoursEventFinish.Items.Add(k.ToString("00"));
                
                cboHoursEditStart.Items.Add(k.ToString("00"));
                cboHoursEditFinish.Items.Add(k.ToString("00"));
            }

            for (int k = 0; k < 60; k++)
            {
                cboMinutesEventStart.Items.Add(k.ToString("00"));
                cboMinutesEventFinish.Items.Add(k.ToString("00"));

                cboMinutesEditStart.Items.Add(k.ToString("00"));
                cboMinutesEditFinish.Items.Add(k.ToString("00"));
            }
        }
        
        private void dataGrid_load(DataGrid dg, DateTime day)
        {
            dg.ItemsSource = _repository.GetNotesOfDate(day).ToList();
        }

        private void Button_MouseEnter_1(object sender, RoutedEventArgs e)
        {
            popupAddEvent.IsOpen = true;
            SetTimeNov();
            tbxNote.Text = "";
        }

        private void date_Chenged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid_load(dataGrid1, (DateTime)dtpDate.SelectedDate);
            dataGrid1.Items.Refresh();
        }

        private void SetTimeNov()
        {
            cboHoursEventStart.SelectedItem = DateTime.Now.Hour.ToString("00");
            cboHoursEventFinish.SelectedItem = DateTime.Now.Hour.ToString("00");

            cboMinutesEventStart.SelectedItem = DateTime.Now.Minute.ToString("00");
            cboMinutesEventFinish.SelectedItem = DateTime.Now.Minute.ToString("00");
        }
        private void selection_note(object sender, SelectionChangedEventArgs e)
        {
            Note editNote = dataGrid1.SelectedItem as Note;
            if (editNote != null)
            {
                var editId = editNote.NoteId;
                responseOfDB = _repository.Get(editId);
                
                tbxNoteEdit.Text = responseOfDB.NoteText;
                dtpDate.DisplayDate = responseOfDB.Date;
                cboHoursEditStart.Text = responseOfDB.NoteStart.Substring(0, 2);
                cboMinutesEditStart.Text = responseOfDB.NoteStart.Substring(3);
                cboHoursEditFinish.Text = responseOfDB.NoteFinish.Substring(0, 2);
                cboMinutesEditFinish.Text = responseOfDB.NoteFinish.Substring(3);   
            }
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void Add_Event_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void Add_Event_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Note eventNote = new Note();

            if (dtpDate.SelectedDate == null)
            {
                dtpDate.SelectedDate = DateTime.Today;
            }

            eventNote.Date = (DateTime)dtpDate.SelectedDate;
            eventNote.NoteStart = cboHoursEventStart.Text + ":" + cboMinutesEventStart.Text;
            eventNote.NoteFinish = cboHoursEventFinish.Text + ":" + cboMinutesEventFinish.Text;
            eventNote.NoteText = myNote.Note.Trim();
            _repository.SaveNote(eventNote);
            tbxNote.Text = "";
          
            dataGrid_load(dataGrid1, (DateTime)dtpDate.SelectedDate);
            popupAddEvent.IsOpen = false;
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void Remove_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (responseOfDB != null)
            {
                _repository.DeleteNote(responseOfDB.NoteId);
            }

            tbxNoteEdit.Text = "";
            dataGrid_load(dataGrid1, (DateTime)dtpDate.DisplayDate);
            popupEdit.IsOpen = false;
        }

        private void Edit_MouseClick(object sender, RoutedEventArgs e)
        {
            popupEdit.IsOpen = responseOfDB != null;
        }

        private void Edit_Event_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void Edit_Event_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Note edit = new Note();

            if (dtpDate.SelectedDate == null)
            {
                dtpDate.SelectedDate = DateTime.Today;
            }

            edit.Date = dtpDate.DisplayDate;
            edit.NoteId = responseOfDB.NoteId;
            edit.NoteStart = cboHoursEditStart.Text + ":" + cboMinutesEditStart.Text;
            edit.NoteFinish = cboHoursEditFinish.Text + ":" + cboMinutesEditFinish.Text;
            edit.NoteText = myNote.Note;
            _repository.SaveNote(edit);

            tbxNoteEdit.Text = "";
            dataGrid_load(dataGrid1, (DateTime)dtpDate.DisplayDate);
            popupEdit.IsOpen = false;
        }
    }
}
