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

namespace LibraryManagement.ViewModel
{
    public class ReaderManagementVM : BaseViewModel
    {
        public ICommand GetNavigationFrameCM { get; set; }
        public ICommand QuanLyDocGiaCM { get; set; }
        public ICommand QuanLyLoaiDocGiaCM { get; set; }

        public Frame NavigationFrame { get; set; }

        public ReaderManagementVM()
        {
            GetNavigationFrameCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                NavigationFrame = p;
            });
            QuanLyDocGiaCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new ReaderManagement());
            });
            QuanLyLoaiDocGiaCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new ReaderTypePage());
            });
        }
    }
}
