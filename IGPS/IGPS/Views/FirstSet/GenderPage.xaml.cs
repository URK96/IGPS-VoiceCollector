using IGPS.Models;
using IGPS.ViewModels;

using System;

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

            FirstSetGenderNextButton.IsEnabled = false;
            FirstSetGenderNextButton.BackgroundColor = Color.Default;
        }

        private void GenderRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as FirstSetViewModel).SetInfo.DataView = sender as RadioButton;

            FirstSetGenderNextButton.IsEnabled = true;
            FirstSetGenderNextButton.BackgroundColor = Color.OrangeRed;
        }

        private void FirstSetGenderNextButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }
    }
}