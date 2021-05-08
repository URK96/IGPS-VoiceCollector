using AiForms.Dialogs.Abstractions;

using IGPS.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.Dialogs
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TextSizeDialogView : DialogView
	{
		public TextSizeDialogView ()
		{
			InitializeComponent ();

			BindingContext = new TextSizeDialogViewModel();

			TextSizeUpDown.Value = (BindingContext as TextSizeDialogViewModel).TextSize;
		}

		private void NoButton_Clicked(object sender, EventArgs e)
		{
			DialogNotifier.Cancel();
		}

		private void YesButton_Clicked(object sender, EventArgs e)
		{
			DialogNotifier.Complete();
		}

        private void TextSizeUpDown_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
			int size = Convert.ToInt32((double)e.Value);

			TextSizePreviewLabel.FontSize = size;

			Preferences.Set(AppSettingKeys.TextSizeSetting, size);
		}
    }
}