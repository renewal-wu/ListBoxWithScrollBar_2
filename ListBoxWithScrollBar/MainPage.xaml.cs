using ListBoxWithScrollBar.SampleData;
using Microsoft.Phone.Controls;

namespace ListBoxWithScrollBar
{
    public partial class MainPage : PhoneApplicationPage
    {
        public TitleTextContentTextViewModel Model;
        public MainPage()
        {
            InitializeComponent();
            Model = new TitleTextContentTextViewModel();
            this.DataContext = Model;
        }
    }
}