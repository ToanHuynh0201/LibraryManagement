using LibraryManagement.View.BookTitle;
using System.Windows.Controls;

namespace LibraryManagement.View
{
    /// <summary>
    /// Interaction logic for BookTab.xaml
    /// </summary>
    public partial class BookTab : Page
    {
        public BookTab()
        {
            InitializeComponent();
            mainFrame.Navigate(new BookTitleManagement());
        }
    }
}
