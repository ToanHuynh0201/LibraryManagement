using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class OverdueBookVM : BaseViewModel
    {
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    if (value == null) _selectedDate = DateTime.Now;
                    else _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    _ = LoadReportDataAsync();
                }
            }
        }

        private ObservableCollection<BCSACHTRATRE> _listOverdueReport;
        public ObservableCollection<BCSACHTRATRE> ListOverdueReport
        {
            get => _listOverdueReport;
            set
            {
                if (_listOverdueReport != value)
                {
                    _listOverdueReport = value;
                    OnPropertyChanged(nameof(ListOverdueReport));
                }
            }
        }

        private bool _isloading;
        public bool IsLoading
        {
            get => _isloading;
            set
            {
                if (_isloading != value)
                {
                    _isloading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public ICommand CreateReportCM { get; set; }
        public ICommand LoadDataReportCM { get; set; }
        public ICommand LoadCurrentDataReportCM { get; set; }
        public OverdueBookVM()
        {
            LoadCurrentDataReportCM = new RelayCommand<object>((p) => !IsLoading, async (p) =>
            {
                ListOverdueReport = new ObservableCollection<BCSACHTRATRE>();
                SelectedDate = DateTime.Now;
            });
            CreateReportCM = new RelayCommand<object>((p) => !IsLoading, async (p) =>
            {
                try
                {
                    if (SelectedDate == null)
                    {
                        MessageBox.Show("Vui lòng chọn ngày để lập báo cáo");
                        return;
                    }

                    var result = MessageBox.Show(
                                 "Lưu ý: Nếu đã có báo cáo cùng ngày, báo cáo cũ sẽ bị xóa và thay thế bằng báo cáo mới.",
                                 "Xác nhận lập báo cáo",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        var res = await Task.Run(async () => await BaoCaoThongKeBLL.Instance.AddBaoCaoSachTraTre(SelectedDate));
                        MessageBox.Show(res.Item2);
                        if (res.Item1)
                        {
                            await LoadReportDataAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã có lỗi khi lập báo cáo, vui lòng thao tác lại : " + ex.Message.ToString());
                }
            });

            LoadDataReportCM = new RelayCommand<object>((p) => !IsLoading, async (p) =>
            {
                await LoadReportDataAsync();
            });
        }
        private async Task LoadReportDataAsync()
        {
            try
            {
                IsLoading = true;
                var data = await BaoCaoThongKeBLL.Instance.GetBaoCaoSachTraTreByNgay(SelectedDate);
                ListOverdueReport = new ObservableCollection<BCSACHTRATRE>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.ToString());
                ListOverdueReport = new ObservableCollection<BCSACHTRATRE>();
            }
            IsLoading = false;
        }
    }
}