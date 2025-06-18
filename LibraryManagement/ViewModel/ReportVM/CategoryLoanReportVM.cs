using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View.Category;
using LibraryManagement.View.Report;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class CategoryLoanReportVM : BaseViewModel
    {
        private ObservableCollection<BCTONGLUOTMUON> _listReports;
        public ObservableCollection<BCTONGLUOTMUON> ListReports
        {
            get => _listReports;
            set
            {
                if (_listReports != value)
                {
                    _listReports = value;
                    OnPropertyChanged(nameof(ListReports));
                }
            }
        }

        private BCTONGLUOTMUON _selectedReport;
        public BCTONGLUOTMUON SelectedReport
        {
            get => _selectedReport;
            set
            {
                if (_selectedReport != value)
                {
                    _selectedReport = value;
                    OnPropertyChanged(nameof(SelectedReport));
                }
            }
        }

        public ObservableCollection<int> ListMonths { get; set; }
        public ObservableCollection<int> ListYears { get; set; }

        private int _selectedMonth;
        public int SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged(nameof(SelectedMonth));
                }
            }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                }
            }
        }

        public ICommand LoadDataCM { get; set; }
        public ICommand AddReportCM { get; set; }
        public ICommand ViewReportDetailCM { get; set; }

        public CategoryLoanReportVM()
        {
            ListMonths = new ObservableCollection<int>(Enumerable.Range(1, 12));
            ListYears = new ObservableCollection<int>(Enumerable.Range(2000, DateTime.Now.Year - 2000 + 1).Reverse());
            SelectedMonth = DateTime.Now.Month;
            SelectedYear = DateTime.Now.Year;

            LoadDataCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                var reportData = await BaoCaoThongKeBLL.Instance.GetAllBaoCaoTongLuotMuon();
                ListReports = new ObservableCollection<BCTONGLUOTMUON>(reportData);
            });

            AddReportCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                try
                {
                    if (SelectedMonth == 0 || SelectedYear == 0)
                    {
                        MessageBox.Show("Vui lòng chọn tháng và năm để lập báo cáo");
                        return;
                    }

                    var result = MessageBox.Show(
                                 "Xác nhận lập báo cáo",
                                 "Xác nhận",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        var res = await Task.Run(async () => await BaoCaoThongKeBLL.Instance.AddBaoCao(SelectedMonth, SelectedYear));
                        MessageBox.Show(res.Item2);
                        if (res.Item1)
                        {
                            LoadDataCM.Execute(null);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Đã có lỗi khi lập báo cáo, vui lòng thao tác lại : " + ex.Message.ToString());
                }
            });

            ViewReportDetailCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                if (SelectedReport == null)
                {
                    return;
                }
                var vm1 = new CategoryLoanReportDetailVM(SelectedReport);
                var w1 = new ReportDetailsWindow() { DataContext = vm1 };
                w1.ShowDialog();
            });
        }
    }
}