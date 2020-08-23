using Xamarin.Forms;

namespace IGPS.Models
{
    public enum FirstSetType
    {
        Initial,
        Code
    }

    public class FirstSetInfo
    {
        public FirstSetType Type { get; set; }
        public View DataView { get; set; }
    }
}
