using Notes.Models;

namespace Notes.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class NotePage : ContentPage
{
    public NotePage()
    {
        InitializeComponent();

        var appDataPath = FileSystem.AppDataDirectory;
        var randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

        LoadNote(Path.Combine(appDataPath, randomFileName));
    }

    public string ItemId
    {
        set => LoadNote(value);
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Note note)
            await File.WriteAllTextAsync(note.Filename, TextEditor.Text);

        await Shell.Current.GoToAsync("..");
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Note note)
            // Delete the file.
            if (File.Exists(note.Filename))
                File.Delete(note.Filename);

        await Shell.Current.GoToAsync("..");
    }

    private void LoadNote(string fileName)
    {
        var noteModel = new Note
        {
            Filename = fileName
        };

        if (File.Exists(fileName))
        {
            noteModel.Date = File.GetCreationTime(fileName);
            noteModel.Text = File.ReadAllText(fileName);
        }

        BindingContext = noteModel;
    }
}