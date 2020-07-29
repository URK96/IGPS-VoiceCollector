using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGPS.Models;
using IGPS.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.FirstSet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenderPage : ContentPage
    {
        public GenderPage()
        {
            InitializeComponent();

            (BindingContext as FirstSetViewModel).SetInfo.Type = FirstSetType.Gender;
        }

        private void GenderRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as FirstSetViewModel).SetInfo.DataView = sender as RadioButton;

            FirstSetGenderNextButton.BackgroundColor = Color.OrangeRed;
        }

        private void FirstSetGenderNextButton_Clicked(object sender, EventArgs e)
        {
            var builder = new StringBuilder();
            var user = UserInfo.LoadUserInfo();

            builder.AppendLine($"이름 : {user.Name}");
            builder.AppendLine($"나이 : {user.Age}");
            builder.Append($"성별 : {(user.Gender == GenderType.Male ? "남" : "여")}");

            DependencyService.Get<IToast>().Show(builder.ToString());
        }
    }
}