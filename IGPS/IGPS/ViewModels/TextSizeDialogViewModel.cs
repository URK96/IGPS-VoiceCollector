using Xamarin.Essentials;

namespace IGPS.ViewModels
{
    class TextSizeDialogViewModel : BaseViewModel
    {
        public int TextSize { get; set; }

        public TextSizeDialogViewModel()
        {
            TextSize = Preferences.Get(AppSettingKeys.TextSizeSetting, 15);
        }
    }
}
