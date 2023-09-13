using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using Notes.Views;

namespace Notes.ViewModels;

internal class NotesViewModel : IQueryAttributable
{
    public NotesViewModel()
    {
        AllNotes = new(Note.LoadAll().Select(n => new NoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync);
    }

    public ObservableCollection<NoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("deleted", out var deleted))
        {
            var noteId = deleted.ToString();
            var matchedNote = AllNotes.FirstOrDefault(n => n.Identifier == noteId);

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.TryGetValue("saved", out var saved))
        {
            var noteId = saved.ToString();
            var matchedNote = AllNotes.FirstOrDefault(n => n.Identifier == noteId);

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }

            // If note isn't found, it's new; add it.
            else
            {
                AllNotes.Insert(0, new(Note.Load(noteId)));
            }
        }
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(NotePage));
    }

    private async Task SelectNoteAsync(NoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(NotePage)}?load={note.Identifier}");
    }
}