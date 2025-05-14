using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
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
    public class BookCopyViewModel : BaseViewModel
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
        private CUONSACH _cuonsach { get; set; }
        public CUONSACH cuonsach
        {
            get
            {
                return _cuonsach;
            }
            set
            {
                _cuonsach = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<CUONSACH> _ListBooks;
        public ObservableCollection<CUONSACH> ListBooks
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
        public ObservableCollection<CUONSACH> AllBooks { get; set; }
        public CUONSACH BookSeleted { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataBookCM { get; set; }
        public ICommand SearchBookCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddBookCM { get; set; }
        public ICommand OpenUpdateBookCM { get; set; }
        public ICommand AddNewBookCM { get; set; }
        public ICommand ViewBookCM { get; set; }
        public ICommand UpdateBookCM { get; set; }
        public ICommand DeleteBookCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public BookCopyViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên bản copy sách" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataBookCM = new RelayCommand<CUONSACH>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await CuonSachBLL.Instance.GetAllCuonSach());
                    ListBooks = new ObservableCollection<CUONSACH>(data);
                    AllBooks = new ObservableCollection<CUONSACH>(data);
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
                        if (SearchProperties == "Mã sách")
                        {
                            var res = await Task.Run(async () => await CuonSachBLL.Instance.GetCuonSachByMaSach(int.Parse(SearchText)));
                            ListBooks = new ObservableCollection<CUONSACH>(res);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
            });
            ResetDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                ListBooks = new ObservableCollection<CUONSACH>(AllBooks);
            });
            OpenAddBookCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                cuonsach = new CUONSACH();
                var w1 = new AddBookCopyWindow();
                w1.ShowDialog();
            });
            OpenUpdateBookCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new EditBookCopyInformationWindow();
                w1.ShowDialog();
            });
          /*  AddNewBookCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await CuonSachBLL.Instance.AddCuonSach(cuonsach));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataBookCM.Execute(null);
                    p.Close();
                }
            });*/
            ViewBookCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new BookCopyInformtationWindow();
                w1.ShowDialog();
            });
            UpdateBookCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await CuonSachBLL.Instance.UpdateCuonSach(BookSeleted));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataBookCM.Execute(null);
                    p.Close();
                }
            });
            DeleteBookCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa cuốn sách này không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await CuonSachBLL.Instance.DeleteCuonSach(BookSeleted.id));
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
