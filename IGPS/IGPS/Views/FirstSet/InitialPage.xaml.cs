using IGPS.Models;
using IGPS.ViewModels;

using System;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.FirstSet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialPage : ContentPage
    {
        public InitialPage()
        {
            InitializeComponent();

            (BindingContext as FirstSetViewModel).SetInfo.Type = FirstSetType.Initial;
            (BindingContext as FirstSetViewModel).SetInfo.DataView = FirstSetInitialInput;

            FirstSetInitialNextButton.IsEnabled = false;
            FirstSetInitialNextButton.BackgroundColor = Color.Default;
        }

        private void FirstSetInitialInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = e.NewTextValue;
            string checkText = Regex.Replace(inputText, "[^A-Z]{1,5}", "", RegexOptions.Singleline);

            if ((inputText != checkText) || string.IsNullOrEmpty(inputText))
            {
                FirstSetInitialNextButton.IsEnabled = false;
                FirstSetInitialNextButton.BackgroundColor = Color.Default;
            }
            else
            {
                FirstSetInitialNextButton.IsEnabled = true;
                FirstSetInitialNextButton.BackgroundColor = Color.OrangeRed;
            }

            (sender as Entry).Text = inputText.ToUpper();
        }

        private void FirstSetInitialNextButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CodePage(), true);
        }
    }
}