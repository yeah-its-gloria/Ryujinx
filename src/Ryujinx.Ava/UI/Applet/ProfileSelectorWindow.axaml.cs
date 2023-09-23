using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ryujinx.Ava.UI.Windows;
using Ryujinx.HLE.HOS.Services.Account.Acc;
using System.Threading.Tasks;

namespace Ryujinx.Ava.UI.Applet
{
    public partial class ProfileSelectorWindow : StyleableWindow
    {
        private readonly Window _owner;
        private UserId _selectedUser = UserId.Null;

        public ProfileSelectorWindow(Window owner, UserProfile[] profiles)
        {
            _owner = owner;
            DataContext = this;
            InitializeComponent();

            foreach (UserProfile profile in profiles)
            {
                AddButton(profile.Name, profile.UserId); // TODO: profile image
            }
        }

        public ProfileSelectorWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void AddButton(string name, UserId userId)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Button button = new() { Content = name, Tag = userId };

                button.Click += Button_Click;
                UserProfileStack.Children.Add(button);
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                _selectedUser = (UserId)button.Tag;
            }

            Close();
        }

        public async Task<UserId> Run()
        {
            await ShowDialog(_owner);
            return _selectedUser;
        }
    }
}
