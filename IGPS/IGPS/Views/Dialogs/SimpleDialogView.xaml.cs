using AiForms.Dialogs.Abstractions;

using IGPS.ViewModels;

using System;

using Xamarin.Forms.Xaml;

namespace IGPS.Views.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleDialogView : DialogView
    {
        public SimpleDialogView()
        {
            InitializeComponent();

            BindingContext = new SimpleDialogViewModel();
        }

        private void NoButton_Clicked(object sender, EventArgs e)
        {
            DialogNotifier.Cancel();
        }

        private void YesButton_Clicked(object sender, EventArgs e)
        {
            DialogNotifier.Complete();
        }
    }
}