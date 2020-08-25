using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.FirstVoiceSet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstVoiceSetInfoPage : ContentPage
    {
        public FirstVoiceSetInfoPage()
        {
            InitializeComponent();
        }

        private void FirstVoiceSetNextButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FirstVoiceSetRecordPage(), true);
        }
    }
}