using System.Configuration;
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
using LibraryManagement.View.Users;

namespace LibraryManagement.ViewModel
{
    public class UserTabViewModel : BaseViewModel
    {
        public ICommand GetNavigationFrameCM { get; set; }
        public ICommand QuanLyNguoiDung { get; set; }
        public ICommand QuanLyNhomNguoiDung { get; set; }
        public ICommand DanhSachChucNang { get; set; }

        public Frame NavigationFrame { get; set; }

        public UserTabViewModel()
        {
            GetNavigationFrameCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                NavigationFrame = p;
            });
            QuanLyNguoiDung = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new UserManagementPage());
            });
            QuanLyNhomNguoiDung = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new UserSettingsGroup());
            });
        }
    }
}
