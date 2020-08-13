using Xamarin.Forms;

namespace IGPS.Models
{
    public enum FirstSetType
    {
        Name,
        Age,
        Gender
    }

    public class FirstSetInfo
    {
        public FirstSetType Type { get; set; }
        public View DataView { get; set; }
    }
}
