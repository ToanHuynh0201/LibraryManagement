using System.Windows.Controls;
using System.Windows.Input;
using LibraryManagement.View;
using LibraryManagement.View.Book;
using LibraryManagement.View.Borrow;
using LibraryManagement.View.Reader;
using LibraryManagement.View.Receipt;
using LibraryManagement.View.Report;

namespace LibraryManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand GetNavigationFrameCM { get; set; }
        public ICommand QuanLyDocGiaCM { get; set; }
        public ICommand QuanLySachCM { get; set; }
        public ICommand QuanLyMuonTraCM { get; set; }
        public ICommand QuanLyPhatCM { get; set; }
        public ICommand QuanLyNoiBoCM { get; set; }
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
                NavigationFrame.Navigate(new ReaderTab());
            });
            QuanLySachCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {   
                NavigationFrame.Navigate(new BookTab());
            });
            QuanLyMuonTraCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new BorrowManagement());
            });
            QuanLyPhatCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new PenaltyReceiptManagement());
            });
            ThongKeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new ReportPage());
            });
            QuanLyNoiBoCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NavigationFrame.Navigate(new InternalViewModel());
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
