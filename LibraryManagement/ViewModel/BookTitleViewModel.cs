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
    public class BookTitleTitleViewModel : BaseViewModel
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
        private DAUSACH _dausach { get; set; }
        public DAUSACH dausach
        {
            get
            {
                return _dausach;
            }
            set
            {
                _dausach = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<DAUSACH> _ListBookTitles;
        public ObservableCollection<DAUSACH> ListBookTitles
        {
            get
            {
                return _ListBookTitles;
            }
            set
            {
                _ListBookTitles = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<DAUSACH> AllBookTitles { get; set; }
        public DAUSACH BookTitleSeleted { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataBookTitleCM { get; set; }
        public ICommand SearchBookTitleCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddBookTitleCM { get; set; }
        public ICommand OpenUpdateBookTitleCM { get; set; }
        public ICommand AddNewBookTitleCM { get; set; }
        public ICommand ViewBookTitleCM { get; set; }
        public ICommand UpdateBookTitleCM { get; set; }
        public ICommand DeleteBookTitleCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public BookTitleTitleViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên đầu s" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataBookTitleCM = new RelayCommand<DAUSACH>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await DauSachBLL.Instance.GetAllDauSach());
                    ListBookTitles = new ObservableCollection<DAUSACH>(data);
                    AllBookTitles = new ObservableCollection<DAUSACH>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchBookTitleCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        ListBookTitles.Clear();
                        ListBookTitles = AllBookTitles;
                    }
                    else
                    {
                        if (SearchProperties == "Mã đầu sách")
                        {
                            var res = await Task.Run(async () => await DauSachBLL.Instance.GetDauSachByMa(SearchText));
                            ListBookTitles = new ObservableCollection<DAUSACH>(res);
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
                ListBookTitles = new ObservableCollection<DAUSACH>(AllBookTitles);
            });
            OpenAddBookTitleCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                dausach = new DAUSACH();
                var w1 = new AddBookTitleWindow();
                w1.ShowDialog();
            });
            OpenUpdateBookTitleCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new EditBookTitleInformationWindow();
                w1.ShowDialog();
            });
            AddNewBookTitleCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await DauSachBLL.Instance.AddDauSach(dausach));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataBookTitleCM.Execute(null);
                    p.Close();
                }
            });
            ViewBookTitleCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new BookTitleInformtationWindow();
                w1.ShowDialog();
            });
            UpdateBookTitleCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await DauSachBLL.Instance.UpdateDauSach(BookTitleSeleted));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataBookTitleCM.Execute(null);
                    p.Close();
                }
            });
            DeleteBookTitleCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa đầu sách này không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await DauSachBLL.Instance.DeleteDauSach(BookTitleSeleted.id));
                    LoadDataBookTitleCM.Execute(null);
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
