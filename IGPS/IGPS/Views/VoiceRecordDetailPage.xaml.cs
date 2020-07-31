using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IGPS.ViewModels;
using IGPS.Models;

namespace IGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoiceRecordDetailPage : ContentPage
    {
        public VoiceRecordDetailPage(VoiceDataItem item)
        {
            InitializeComponent();

            BindingContext = new VoiceRecordDetailViewModel(item);
        }
    }
}