using FluentAvalonia.UI.Controls;
using Ryujinx.Ava.UI.Helpers;
using Ryujinx.HLE.HOS.Services.Account.Acc;
using System.Threading.Tasks;
using Avalonia.Controls;
using Ryujinx.Ava.Common.Locale;
using Ryujinx.Ava.UI.ViewModels;
using ProfileModel = Ryujinx.Ava.UI.Models.UserProfile;
using Ryujinx.HLE.FileSystem;
using System.Linq;
using Avalonia.Interactivity;

namespace Ryujinx.Ava.UI.Applet
{
    public partial class ProfileSelectorDialog : UserControl
    {
        private UserId _selectedUser = UserId.Null;
        public UserProfileViewModel ViewModel { get; set; }

        private ContentDialog _host;

        public ProfileSelectorDialog(UserProfile[] profiles)
        {
            ViewModel = new();
            DataContext = ViewModel;

            foreach (UserProfile profile in profiles.OrderBy(x => x.Name))
            {
                ViewModel.Profiles.Add(new ProfileModel(profile, null));
            }

            InitializeComponent();
        }

        public ProfileSelectorDialog()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void OnProfileSelect(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox list)
            {
                _selectedUser = (ViewModel.Profiles[list.SelectedIndex] as ProfileModel).UserId;
                _host.Hide(ContentDialogResult.Primary);
            }
        }

        public static async Task<UserId> ShowProfileSelector(UserProfile[] users)
        {
            ContentDialog contentDialog = new();

            ProfileSelectorDialog content = new(users) { _host = contentDialog };

            contentDialog.Title = LocaleManager.Instance[LocaleKeys.ProfileSelectorTitle];
            contentDialog.CloseButtonText = LocaleManager.Instance[LocaleKeys.InputDialogCancel];
            contentDialog.Content = content;

            UserId selected = UserId.Null;

            void Handler(ContentDialog sender, ContentDialogClosedEventArgs eventArgs)
            {
                if (eventArgs.Result == ContentDialogResult.Primary)
                {
                    selected = content._selectedUser;
                }
            }

            contentDialog.Closed += Handler;
            await ContentDialogHelper.ShowAsync(contentDialog);
            return selected;
        }
    }
}
