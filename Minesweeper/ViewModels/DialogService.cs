using System.Windows;
using MessageBox = Minesweeper.Views.MessageBox;

namespace Minesweeper.ViewModels
{
    public class DialogService : IDialogService
    {
        private readonly Window _window;
        private MessageBox _messageBox;

        public DialogService(Window window)
        {
            _window = window;
        }

        public bool? ShowMessageBox(string messageText)
        {
            _messageBox = new MessageBox {Owner = _window, MessageText = messageText};
            return _messageBox.ShowDialog();
        }

        public void CloseMessageBox(bool? dialogResult)
        {
            if (dialogResult.HasValue)
            {
                _messageBox.DialogResult = dialogResult;
            }

            _messageBox.Close();
        }
    }
}
