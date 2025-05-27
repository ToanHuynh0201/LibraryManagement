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
    public class BookManagementViewModel : BaseViewModel
    {
        public ICommand GetNavigationFrameCM { get; set; }
        public ICommand QuanLySachCM { get; set; }
        public ICommand QuanLyDauSachCM { get; set; }
        public ICommand QuanLyCuonSachCM { get; set; }
        public ICommand QuanLyNhapSachCM { get; set; }
        public ICommand QuanLyTacGiaCM { get; set; }
        public ICommand QuanLyTheLoaiCM { get; set; }

        public Frame NavigationFrame { get; set; }

        public BookManagementViewModel()
        {
            GetNavigationFrameCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                NavigationFrame = p;
            });
            QuanLySachCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new BookManagement());
            });
            QuanLyDauSachCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new BookTitleManagement());
            });
            QuanLyCuonSachCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new BookCopyManagement());
            });
            QuanLyNhapSachCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new BookReceivingManagement());
            });
            QuanLyTheLoaiCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new CategoryManagement());
            });
            QuanLyTacGiaCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new AuthorManagement());
            });
        }
    }
}
