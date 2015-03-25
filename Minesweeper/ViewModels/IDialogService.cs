namespace Minesweeper.ViewModels
{
    public interface IDialogService
    {
        bool? ShowMessageBox(string messageText);
        void CloseMessageBox(bool? dialogResult = null);
    }
}
