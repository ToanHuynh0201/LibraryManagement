using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class AddReaderViewModel : BaseViewModel
    {
        public DOCGIA docgia { get; set; } = new DOCGIA()
        {
            NgaySinhDG = DateTime.Now.AddYears(-18),
            NgayLapThe = DateTime.Now,
        };
        private string _readertypeText { get; set; }
        public string ReaderTypeText
        {
            get => _readertypeText;
            set
            {
                if (_readertypeText != value)
                {
                    _readertypeText = value;
                    OnPropertyChanged(nameof(ReaderTypeText));
                    if (_isSearching) _ = ReaderTypeSearch();
                }
            }
        }
        private ObservableCollection<LOAIDOCGIA> _listReaderType { get; set; }
        public ObservableCollection<LOAIDOCGIA> ListReaderType
        {
            get => _listReaderType;
            set
            {
                if (_listReaderType != value)
                {
                    _listReaderType = value;
                    OnPropertyChanged(nameof(ListReaderType));
                }
            }
        }
        private LOAIDOCGIA _ReaderTypeSelected;
        public LOAIDOCGIA ReaderTypeSelected
        {
            get => _ReaderTypeSelected;
            set
            {
                _ReaderTypeSelected = value;
                OnPropertyChanged(nameof(ReaderTypeSelected));
                if (_ReaderTypeSelected != null)
                {
                    _isSearching = false;
                    ReaderTypeText = _ReaderTypeSelected.DisplayName;
                    _isSearching = true;
                }
            }
        }

        private bool _isSearching = true;
        public Action OnSuccess { get; set; }

        public ICommand AddNewReaderCM { get; set; }
        public ICommand AddAuthorToListCM { get; set; }
        public ICommand DeleteAuthorFromListCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand LoadOtherDataCM { get; set; }
        public AddReaderViewModel()
        {
            LoadOtherDataCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                var readertypedata = await LoaiDocGiaBLL.Instance.GetAllLoaiDocGia();
                ListReaderType = new ObservableCollection<LOAIDOCGIA>(readertypedata);
            });
            AddNewReaderCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                if (ReaderTypeSelected == null) docgia.MaLoaiDG = 0;
                else docgia.MaLoaiDG = ReaderTypeSelected.id;
                var res = await Task.Run(async () => await DocGiaBLL.Instance.AddDocGia(docgia));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    OnSuccess?.Invoke();
                    p.Close();
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
        }
        private async Task ReaderTypeSearch()
        {
            var res = await Task.Run(async () => await LoaiDocGiaBLL.Instance.GetLoaiDocGiaByTen(ReaderTypeText));
            ListReaderType = new ObservableCollection<LOAIDOCGIA>(res);
        }
    }
}