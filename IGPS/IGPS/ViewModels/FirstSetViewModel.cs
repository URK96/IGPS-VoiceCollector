using IGPS.Models;

using System.Windows.Input;

using Xamarin.Forms;

namespace IGPS.ViewModels
{
    class FirstSetViewModel : BaseViewModel
    {
        public FirstSetInfo SetInfo { get; set; }

        public ICommand SaveInput { private set; get; }

        public FirstSetViewModel()
        {
            SetInfo = new FirstSetInfo();
            SaveInput = new Command<FirstSetInfo>((info) => { WriteUserInfo(info); });
        }

        private void WriteUserInfo(FirstSetInfo info)
        {
            var user = AppEnvironment.authService.AuthenticatedUser;

            switch (info.Type)
            {
                case FirstSetType.Initial:
                    user.Initial = (info.DataView as Entry).Text;
                    break;
                case FirstSetType.Code:
                    user.Code = int.Parse((info.DataView as Entry).Text);
                    user.FirstSetCompleted = true;
                    break;
                default:
                    return;
            }

            user.SaveUserInfo();
        }
    }
}
