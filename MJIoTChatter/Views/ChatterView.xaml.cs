using System.Windows.Controls;

namespace MjIot.Devices.Reference.Chatter.Views
{
    /// <summary>
    /// Interaction logic for ChatterView.xaml
    /// </summary>
    public partial class ChatterView : UserControl
    {
        public ChatterView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            //if (e.ExtentHeightChange != 0 && Math.Abs(sv.VerticalOffset - sv.ScrollableHeight) < 20)
            if (sv.ScrollableHeight - sv.VerticalOffset < 20)
            {
                sv.ScrollToEnd();
            }
        }
    }
}