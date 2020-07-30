using IGPS.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
                case FirstSetType.Name:
                    user.Name = (info.DataView as Entry).Text;
                    break;
                case FirstSetType.Age:
                    user.Age = int.Parse((info.DataView as Entry).Text);
                    break;
                case FirstSetType.Gender:
                    user.Gender = (info.DataView as RadioButton).Text == AppResources.UserFirstSet_GenderPage_Male ? GenderType.Male : GenderType.Female;
                    user.FirstSetCompleted = true;
                    break;
                default:
                    return;
            }

            user.SaveUserInfo();
        }
    }
}
