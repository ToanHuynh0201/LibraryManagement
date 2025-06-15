using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.Reader;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class ReaderTypeViewModel : BaseViewModel
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
        private LOAIDOCGIA _loaidocgia { get; set; }
        public LOAIDOCGIA loaidocgia
        {
            get
            {
                return _loaidocgia;
            }
            set
            {
                _loaidocgia = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<LOAIDOCGIA> _ListReaderType;
        public ObservableCollection<LOAIDOCGIA> ListReaderType
        {
            get
            {
                return _ListReaderType;
            }
            set
            {
                _ListReaderType = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<LOAIDOCGIA> AllReaderType { get; set; }
        private LOAIDOCGIA _readertypeSelected;
        public LOAIDOCGIA ReaderTypeSelected
        {
            get => _readertypeSelected;
            set
            {
                _readertypeSelected = value;
                ReaderTypeEdit = value != null ? new LOAIDOCGIA
                {
                    MaLoaiDG = value.MaLoaiDG,
                    TenLoaiDG = value.TenLoaiDG
                } : null;
                OnPropertyChanged(nameof(ReaderTypeSelected));
            }
        }

        private LOAIDOCGIA _readertypeEdit;
        public LOAIDOCGIA ReaderTypeEdit
        {
            get => _readertypeEdit;
            set
            {
                _readertypeEdit = value;
                OnPropertyChanged(nameof(ReaderTypeEdit));
            }
        }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataReaderTypeCM { get; set; }
        public ICommand SearchReaderTypeCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddReaderTypeCM { get; set; }
        public ICommand OpenUpdateReaderTypeCM { get; set; }
        public ICommand AddNewReaderTypeCM { get; set; }
        public ICommand ViewReaderTypeCM { get; set; }
        public ICommand UpdateReaderTypeCM { get; set; }
        public ICommand DeleteReaderTypeCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public ReaderTypeViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên tác giả" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataReaderTypeCM = new RelayCommand<LOAIDOCGIA>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await LoaiDocGiaBLL.Instance.GetAllLoaiDocGia());
                    ListReaderType = new ObservableCollection<LOAIDOCGIA>(data);
                    AllReaderType = new ObservableCollection<LOAIDOCGIA>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchReaderTypeCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        ListReaderType.Clear();
                        ListReaderType = AllReaderType;
                    }
                    else
                    {
                        if (SearchProperties == "Tên loại độc giả")
                        {
                            var res = await Task.Run(async () => await LoaiDocGiaBLL.Instance.GetLoaiDocGiaByTen(SearchText));
                            ListReaderType = new ObservableCollection<LOAIDOCGIA>(res);
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
                ListReaderType = new ObservableCollection<LOAIDOCGIA>(AllReaderType);
            });
            OpenAddReaderTypeCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                loaidocgia = new LOAIDOCGIA();
                var w1 = new AddReaderTypeWindow();
                w1.ShowDialog();
            });
            OpenUpdateReaderTypeCM = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                if (p.IsEnabled) ReaderTypeSelected = null;
                p.IsEnabled = !(p.IsEnabled);
            });
            AddNewReaderTypeCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await LoaiDocGiaBLL.Instance.AddLoaiDocGia(loaidocgia));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataReaderTypeCM.Execute(null);
                    p.Close();
                }
            });
            UpdateReaderTypeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                ReaderTypeSelected.TenLoaiDG = ReaderTypeEdit.TenLoaiDG;
                var res = await Task.Run(async () => await LoaiDocGiaBLL.Instance.UpdateLoaiDocGia(ReaderTypeSelected));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataReaderTypeCM.Execute(null);
                }
            });
            DeleteReaderTypeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa loại độc giả này không?", "Xác nhận xóa",
                                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await LoaiDocGiaBLL.Instance.DeleteLoaiDocGia(ReaderTypeSelected.id));
                    LoadDataReaderTypeCM.Execute(null);
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
