using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class BookSearchViewModel : BaseViewModel
    {
        #region Properties
        private string _SearchText;
        public string SearchText
        {
            get => _SearchText;
            set
            {
                _SearchText = value;
                OnPropertyChanged();
                FilterBooks();
            }
        }

        private string _SelectedTheLoai;
        public string SelectedTheLoai
        {
            get => _SelectedTheLoai;
            set
            {
                _SelectedTheLoai = value;
                OnPropertyChanged();
                FilterBooks();
            }
        }

        private string _SelectedTinhTrang;
        public string SelectedTinhTrang
        {
            get => _SelectedTinhTrang;
            set
            {
                _SelectedTinhTrang = value;
                OnPropertyChanged();
                FilterBooks();
            }
        }

        private ObservableCollection<SACH> _ListBooks;
        public ObservableCollection<SACH> ListBooks
        {
            get => _ListBooks;
            set
            {
                _ListBooks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SACH> AllBooks { get; set; } = new ObservableCollection<SACH>();
        public ObservableCollection<string> ListTheLoai { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ListTinhTrang { get; set; } = new ObservableCollection<string>
        {
            "Tất cả", "Vẫn còn", "Đã mượn hết"
        };

        public ICommand LoadDataBookCM { get; set; }
        public ICommand ViewBookCM { get; set; }

        public SACH BookSeleted { get; set; }
        #endregion

        public BookSearchViewModel()
        {
            LoadDataBookCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    var allBooks = await SachBLL.Instance.GetAllSach();
                    AllBooks = new ObservableCollection<SACH>(allBooks);
                    await LoadTheLoaiAsync();
                    FilterBooks();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Lỗi tải dữ liệu sách: " + ex.Message);
                }
            });
        }
        private async Task LoadTheLoaiAsync()
        {
            var list = await TheLoaiBLL.Instance.GetAllTheLoai();
            ListTheLoai.Clear();
            ListTheLoai.Add("Tất cả");
            foreach (var item in list)
                ListTheLoai.Add(item.TenTheLoai);

            SelectedTheLoai = "Tất cả";
            SelectedTinhTrang = "Tất cả";
        }

        private void FilterBooks()
        {
            var filtered = AllBooks.AsEnumerable();

            // Lọc theo tên đầu sách
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(s => s.DAUSACH?.DisplayName?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Lọc theo thể loại
            if (!string.IsNullOrWhiteSpace(SelectedTheLoai) && SelectedTheLoai != "Tất cả")
            {
                filtered = filtered.Where(s => s.DAUSACH?.THELOAI?.TenTheLoai == SelectedTheLoai);
            }

            // Lọc theo tình trạng
            if (SelectedTinhTrang == "Vẫn còn")
            {
                filtered = filtered.Where(s => s.SoLuongCon > 0);
            }
            else if (SelectedTinhTrang == "Đã mượn hết")
            {
                filtered = filtered.Where(s => s.SoLuongCon == 0);
            }

            ListBooks = new ObservableCollection<SACH>(filtered);
        }
    }
}
