using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.BookCopy;
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
        private string _StatusProperties;
        public string StatusProperties
        {
            get
            {
                return _StatusProperties;
            }
            set
            {
                _StatusProperties = value;
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
        public ObservableCollection<string> StatusList { get; set; }

        private ObservableCollection<CUONSACH> _listBookCopy { get; set; }
        public ObservableCollection<CUONSACH> ListBookCopy
        {
            get
            {
                return _listBookCopy;
            }
            set
            {
                _listBookCopy = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CUONSACH> AllBookCopy { get; set; }
        public CUONSACH BookCopySelected { get; set; }
        #endregion

        #region Commands
        public ICommand LoadDataBookCopyCM { get; set; }
        public ICommand ResetSearchDataCM { get; set; }
        public ICommand SearchBookCopyCM { get; set; }
        public ICommand DeleteBookCopyCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand FilterStatus { get; set; }
        #endregion

        public BookCopyViewModel()
        {
            SearchList = new ObservableCollection<string> { "Mã sách", "Mã đầu sách", "Tên đầu sách" };
            SearchProperties = SearchList.FirstOrDefault();

            StatusList = new ObservableCollection<string> { "Tất cả", "Đã mượn", "Chưa mượn" };
            StatusProperties = StatusList.FirstOrDefault();

            LoadDataBookCopyCM = new RelayCommand<CUONSACH>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await CuonSachBLL.Instance.GetAllCuonSach());
                    ListBookCopy = new ObservableCollection<CUONSACH>(data);
                    AllBookCopy = new ObservableCollection<CUONSACH>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });

            SearchBookCopyCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    await ApplyFilters();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
            });

            FilterStatus = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    await ApplyFilters();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lọc trạng thái: " + ex.Message);
                }
            });

            ResetSearchDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                StatusProperties = "Tất cả";
                ListBookCopy = new ObservableCollection<CUONSACH>(AllBookCopy);
            });

            DeleteBookCopyCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn ẩn cuốn sách này không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await CuonSachBLL.Instance.DeleteCuonSach(BookCopySelected.id));
                    LoadDataBookCopyCM.Execute(null);
                    MessageBox.Show(res.Item2);
                }
            });

            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
        private async Task ApplyFilters()
        {
            ObservableCollection<CUONSACH> filteredData = new ObservableCollection<CUONSACH>();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                filteredData = new ObservableCollection<CUONSACH>(AllBookCopy);
            }
            else
            {
                List<CUONSACH> res = new List<CUONSACH>();

                if (SearchProperties == "Mã sách")
                {
                    res = await Task.Run(async () => await CuonSachBLL.Instance.GetCuonSachByMaSach(SearchText));
                }
                else if (SearchProperties == "Mã đầu sách")
                {
                    res = await Task.Run(async () => await CuonSachBLL.Instance.GetCuonSachByMaDauSach(SearchText));
                }
                else if (SearchProperties == "Tên đầu sách")
                {
                    res = await Task.Run(async () => await CuonSachBLL.Instance.GetCuonSachByTenDauSach(SearchText));
                }

                filteredData = new ObservableCollection<CUONSACH>(res);
            }

            if (StatusProperties != "Tất cả")
            {
                var res = filteredData.Where(cs =>
                {
                    if (StatusProperties == "Đã mượn")
                    {
                        return cs.TinhTrang == true;
                    }
                    else if (StatusProperties == "Chưa mượn")
                    {
                        return cs.TinhTrang == false;
                    }
                    return true;
                }).ToList();

                filteredData = new ObservableCollection<CUONSACH>(res);
            }

            ListBookCopy = filteredData;
        }
    }
}