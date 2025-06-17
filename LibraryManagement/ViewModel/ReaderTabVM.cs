using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View.Reader;
using LibraryManagement.View.UserReader;
using LibraryManagement.ViewModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class ReaderTabViewModel : BaseViewModel
    {
        public ICommand GetNavigationFrameCM { get; set; }
        public ICommand ThongTinDocGiaCM { get; set; }
        public ICommand TraCuuSachCM { get; set; }

        public Frame NavigationFrame { get; set; }

        private NGUOIDUNG currentUser;

        public ReaderTabViewModel() { }
        public ReaderTabViewModel(NGUOIDUNG user)
        {
            currentUser = user;

            GetNavigationFrameCM = new RelayCommand<Frame>((p) => true, (p) => NavigationFrame = p);

            ThongTinDocGiaCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                var dsdocgia = await DocGiaBLL.Instance.GetAllDocGia();
                var docgia = dsdocgia.FirstOrDefault(dg => dg.TenDangNhap == currentUser.TenDangNhap);
                var vm1 = new ReaderInformationViewModel(docgia);
                var w1 = new ReaderPage() { DataContext = vm1 };
                NavigationFrame.Navigate(w1);
            });

            TraCuuSachCM = new RelayCommand<object>((p) => true, (p) =>
            {
                NavigationFrame.Navigate(new BookSearchPage());
            });
        }
    }
}
