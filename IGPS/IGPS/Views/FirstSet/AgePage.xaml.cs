using IGPS.Models;
using IGPS.ViewModels;

using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.FirstSet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgePage : ContentPage
    {
        public AgePage()
        {
            InitializeComponent();

            (BindingContext as FirstSetViewModel).SetInfo.Type = FirstSetType.Age;
            (BindingContext as FirstSetViewModel).SetInfo.DataView = FirstSetAgeInput;
        }

        private void FirstSetAgeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = e.NewTextValue;

            if (string.IsNullOrEmpty(inputText))
            {
                FirstSetAgeInputStatus.IsVisible = true;

                FirstSetAgeNextButton.IsEnabled = false;
                FirstSetAgeNextButton.BackgroundColor = Color.Default;
            }
            else
            {
                FirstSetAgeInputStatus.IsVisible = false;

                FirstSetAgeNextButton.IsEnabled = true;
                FirstSetAgeNextButton.BackgroundColor = Color.OrangeRed;
            }
        }

        private void FirstSetAgeNextButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GenderPage(), true);
        }
    }
}