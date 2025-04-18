using System.Windows.Controls;
using System.Windows.Input;
using LibraryManagement.View;

namespace LibraryManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand GetNavigationFrameCM { get; set; }
        public ICommand QuanLyDocGiaCM { get; set; }
        public ICommand QuanLySachCM { get; set; }
        public ICommand QuanLyNhanSuCM { get; set; }
        public ICommand ThongKeCM { get; set; }
        public ICommand TaiKhoanCM { get; set; }
        public ICommand DangXuatCM { get; set; }
        public ICommand ThoatCM { get; set; }

        public Frame NavigationFrame { get; set; }

        public MainViewModel()
        {
            GetNavigationFrameCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                NavigationFrame = p;
            });
            QuanLyDocGiaCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new ReaderManagement());
            });
            QuanLySachCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {   
                NavigationFrame.Navigate(new BookManagement());
            });
            ThongKeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            TaiKhoanCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new Account());
            });
            DangXuatCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            ThoatCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                App.Current.Shutdown();
            });
        }
    }
}
