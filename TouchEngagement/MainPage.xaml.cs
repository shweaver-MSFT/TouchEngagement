using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TouchEngagementSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            RedButton.PointerMoved += RedButton_PointerMoved;
            BlueButton.PointerMoved += BlueButton_PointerMoved;

            // Wire up the touch engagement logic
            FrameworkElementExtensions.TouchEngagement.SetIsTouchEngagementEnabled(RedButton, true);
        }

        private void RedButton_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("RedButton_PointerMoved");
        }

        private void BlueButton_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("BlueButton_PointerMoved");
        }
    }
}
