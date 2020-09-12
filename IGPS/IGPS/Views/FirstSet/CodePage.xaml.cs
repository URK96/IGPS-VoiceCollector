using IGPS.Models;
using IGPS.ViewModels;

using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.FirstSet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodePage : ContentPage
    {
        public CodePage()
        {
            InitializeComponent();

            (BindingContext as FirstSetViewModel).SetInfo.Type = FirstSetType.Code;
            (BindingContext as FirstSetViewModel).SetInfo.DataView = FirstSetCodeInput;

            FirstSetCodeNextButton.IsEnabled = false;
            FirstSetCodeNextButton.BackgroundColor = Color.Default;
        }

        private void FirstSetCodeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = e.NewTextValue;

            if (string.IsNullOrEmpty(inputText))
            {
                FirstSetCodeInputStatus.IsVisible = true;
                FirstSetCodeInputStatus.Text = AppResources.UserFirstSet_InputError_Blank;

                FirstSetCodeNextButton.IsEnabled = false;
                FirstSetCodeNextButton.BackgroundColor = Color.Default;
            }
            else if (inputText.Count() < 8)
            {
                FirstSetCodeInputStatus.IsVisible = true;
                FirstSetCodeInputStatus.Text = AppResources.UserFirstSet_InputError_Count8;

                FirstSetCodeNextButton.IsEnabled = false;
                FirstSetCodeNextButton.BackgroundColor = Color.Default;
            }
            else
            {
                FirstSetCodeInputStatus.IsVisible = false;

                FirstSetCodeNextButton.IsEnabled = true;
                FirstSetCodeNextButton.BackgroundColor = Color.OrangeRed;
            }
        }

        private void FirstSetCodeNextButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }
    }
}