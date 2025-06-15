using LibraryManagement.BLL;
using LibraryManagement.DAL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.Book;
using LibraryManagement.View.BookTitle;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class BookViewModel : BaseViewModel
    {
        #region Properties
        private string _SearchText;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                _SearchText = value;
                OnPropertyChanged();
            }
        }
        private string _SearchProperties;
        public string SearchProperties
        {
            get
            {
                return _SearchProperties;
            }
            set
            {
                _SearchProperties = value;
                OnPropertyChanged();
            }
        }
        private SACH _sach { get; set; }
        public SACH sach
        {
            get
            {
                return _sach;
            }
            set
            {
                _sach = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<SACH> _ListBooks;
        public ObservableCollection<SACH> ListBooks
        {
            get
            {
                return _ListBooks;
            }
            set
            {
                _ListBooks = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<SACH> AllBooks { get; set; }
        public SACH BookSeleted { get; set; }
        #endregion

        #region Commands
        public ICommand LoadDataBookCM { get; set; }
        public ICommand SearchBookCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand ResetSearchDataCM { get; set; }
        public ICommand OpenAddBookCM { get; set; }
        public ICommand OpenAddExistingBookCM { get; set; }
        public ICommand AddNewBookCM { get; set; }
        public ICommand ViewBookCM { get; set; }
        public ICommand UpdateBookCM { get; set; }
        public ICommand DeleteBookCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public BookViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên đầu sách" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataBookCM = new RelayCommand<SACH>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await SachBLL.Instance.GetAllSach());
                    ListBooks = new ObservableCollection<SACH>(data);
                    AllBooks = new ObservableCollection<SACH>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchBookCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        ListBooks.Clear();
                        ListBooks = AllBooks;
                    }
                    else
                    {
                        if (SearchProperties == "Tên đầu sách")
                        {
                            var res = await Task.Run(async () => await SachBLL.Instance.GetSachByTenDauSach(SearchText));
                            ListBooks = new ObservableCollection<SACH>(res);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
            });
            ResetSearchDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                ListBooks = new ObservableCollection<SACH>(AllBooks);
            });
            OpenAddBookCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                var vm1 = new AddBookViewModel();
                vm1.OnSuccess = () => LoadDataBookCM.Execute(null);
                var w1 = new AddBookWindow { DataContext = vm1 };
                w1.ShowDialog();
            });
            OpenAddExistingBookCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                var vm1 = new AddExistingBookViewModel();
                vm1.OnSuccess = () => LoadDataBookCM.Execute(null);
                var w1 = new AddExistingBookWindow { DataContext = vm1 };
                w1.ShowDialog();
            });
            ViewBookCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (BookSeleted != null)
                {
                    var vm1 = new BookInformationViewModel(BookSeleted);
                    var w1 = new BookInformtationWindow { DataContext = vm1 };
                    w1.ShowDialog();
                }
            });
            DeleteBookCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn ẩn sách này không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await SachBLL.Instance.DeleteSach(BookSeleted.id));
                    LoadDataBookCM.Execute(null);
                    MessageBox.Show(res.Item2);
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
