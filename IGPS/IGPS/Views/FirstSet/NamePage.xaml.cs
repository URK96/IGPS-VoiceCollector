using IGPS.Models;
using IGPS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.FirstSet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NamePage : ContentPage
    {
        public NamePage()
        {
            InitializeComponent();

            (BindingContext as FirstSetViewModel).SetInfo.Type = FirstSetType.Name;
            (BindingContext as FirstSetViewModel).SetInfo.DataView = FirstSetNameInput;
        }

        private void FirstSetNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = e.NewTextValue;
            string checkText = Regex.Replace(inputText, "[^가-힣]{1,10}", "", RegexOptions.Singleline);

            if ((inputText != checkText) || string.IsNullOrEmpty(inputText))
            {
                FirstSetNameInputStatus.IsVisible = true;

                FirstSetNameNextButton.IsEnabled = false;
                FirstSetNameNextButton.BackgroundColor = Color.Default;
            }
            else
            {
                FirstSetNameInputStatus.IsVisible = false;

                FirstSetNameNextButton.IsEnabled = true;
                FirstSetNameNextButton.BackgroundColor = Color.OrangeRed;
            }
        }

        private void FirstSetNameNextButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgePage(), true);
        }
    }
}