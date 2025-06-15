using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.BookReceiving;
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
    public class BookReceivingViewModel : BaseViewModel
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
        private DateTime? _SelectedDate;
        public DateTime? SelectedDate
        {
            get
            {
                return _SelectedDate;
            }
            set
            {
                _SelectedDate = value;
                OnPropertyChanged();
                _ = ApplyFilters();
            }
        }
        private PHIEUNHAPSACH _phieunhap { get; set; }
        public PHIEUNHAPSACH phieunhap
        {
            get
            {
                return _phieunhap;
            }
            set
            {
                _phieunhap = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<PHIEUNHAPSACH> _ReceivingForm;
        public ObservableCollection<PHIEUNHAPSACH> ReceivingForm
        {
            get
            {
                return _ReceivingForm;
            }
            set
            {
                _ReceivingForm = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PHIEUNHAPSACH> AllReceivingForms { get; set; }
        public PHIEUNHAPSACH ReceivingFormSeleted { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataReceivingFormCM { get; set; }
        public ICommand SearchReceivingFormCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddReceivingFormCM { get; set; }
        public ICommand OpenUpdateReceivingFormCM { get; set; }
        public ICommand AddNewReceivingFormCM { get; set; }
        public ICommand ViewReceivingFormCM { get; set; }
        public ICommand UpdateReceivingFormCM { get; set; }
        public ICommand DeleteReceivingFormCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public BookReceivingViewModel()
        {
            SearchList = new ObservableCollection<string> { "Số phiếu nhập" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataReceivingFormCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await PhieuNhapSachBLL.Instance.GetAllPhieuNhapSach());
                    ReceivingForm = new ObservableCollection<PHIEUNHAPSACH>(data);
                    AllReceivingForms = new ObservableCollection<PHIEUNHAPSACH>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });

            SearchReceivingFormCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
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

            ResetDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                SelectedDate = null;
                ReceivingForm = new ObservableCollection<PHIEUNHAPSACH>(AllReceivingForms);
            });

            ViewReceivingFormCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (ReceivingFormSeleted != null)
                {
                    var vm1 = new BookReceivingInformationViewModel(ReceivingFormSeleted);
                    Window w1 = new ReceivingFormInformtationWindow() { DataContext = vm1 };
                    w1.ShowDialog();
                }
            });

            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
        private async Task ApplyFilters()
        {
            ObservableCollection<PHIEUNHAPSACH> filteredData = new ObservableCollection<PHIEUNHAPSACH>();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                filteredData = new ObservableCollection<PHIEUNHAPSACH>(AllReceivingForms);
            }
            else
            {
                List<PHIEUNHAPSACH> res = new List<PHIEUNHAPSACH>();

                if (SearchProperties == "Số phiếu nhập")
                {
                    res = await Task.Run(async () => await PhieuNhapSachBLL.Instance.GetPhieuNhapSachBySoPNS(SearchText));
                }

                filteredData = new ObservableCollection<PHIEUNHAPSACH>(res);
            }

            if (SelectedDate.HasValue)
            {
                var res = filteredData.Where(pns =>
                    pns.NgayNhap.Date == SelectedDate.Value.Date
                ).ToList();

                filteredData = new ObservableCollection<PHIEUNHAPSACH>(res);
            }

            ReceivingForm = filteredData;
        }
    }
}