using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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
            QuanLyDocGiaCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                
            });
            QuanLySachCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {

            });
            ThongKeCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {

            });
            TaiKhoanCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {

            });
            DangXuatCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {

            });
            ThoatCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {

            });
        }
    }
}
