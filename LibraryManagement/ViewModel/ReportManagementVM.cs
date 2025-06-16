using System.Windows.Controls;
using System.Windows.Input;
using LibraryManagement.View;
using LibraryManagement.View.Author;
using LibraryManagement.View.Book;
using LibraryManagement.View.BookCopy;
using LibraryManagement.View.BookReceiving;
using LibraryManagement.View.BookTitle;
using LibraryManagement.View.Category;
using LibraryManagement.View.Reader;
using LibraryManagement.View.Report;

namespace LibraryManagement.ViewModel
{
    public class ReportManagementVM : BaseViewModel
    {
        public ICommand GetNavigationFrameCM { get; set; }
        public ICommand BaoCaoTheLoaiCM { get; set; }
        public ICommand BaoCaoSachTraTreCM { get; set; }

        public Frame NavigationFrame { get; set; }

        public ReportManagementVM()
        {
        GetNavigationFrameCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
        {
            NavigationFrame = p;
        });
        BaoCaoTheLoaiCM = new RelayCommand<object>((p) => { return true; }, (p) =>
        {
            NavigationFrame.Navigate(new CategoryLoanReportManagement());
        });
        BaoCaoSachTraTreCM = new RelayCommand<object>((p) => { return true; }, (p) =>
        {
            NavigationFrame.Navigate(new OverdueBooksReportManagement());
        });
        }
    }
}
