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
    public class BookReceivingFormViewModel : BaseViewModel
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
        private ObservableCollection<PHIEUNHAPSACH> _RecevingForm;
        public ObservableCollection<PHIEUNHAPSACH> RecevingForm
        {
            get
            {
                return _RecevingForm;
            }
            set
            {
                _RecevingForm = value;
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

        public BookReceivingFormViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên phiêu nhập sách" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataReceivingFormCM = new RelayCommand<PHIEUNHAPSACH>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await PhieuNhapSachBLL.Instance.GetAllPhieuNhapSach());
                    RecevingForm = new ObservableCollection<PHIEUNHAPSACH>(data);
                    AllReceivingForms = new ObservableCollection<PHIEUNHAPSACH>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
           
            ResetDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                RecevingForm = new ObservableCollection<PHIEUNHAPSACH>(AllReceivingForms);
            });
            OpenAddReceivingFormCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                phieunhap = new PHIEUNHAPSACH();
                var w1 = new AddReceivingFormWindow();
                w1.ShowDialog();
            });
            OpenUpdateReceivingFormCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new EditReceivingFormInformationWindow();
                w1.ShowDialog();
            });
            AddNewReceivingFormCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await PhieuNhapSachBLL.Instance.AddPhieuNhapSach(phieunhap));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataReceivingFormCM.Execute(null);
                    p.Close();
                }
            });
            ViewReceivingFormCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new ReceivingFormInformtationWindow();
                w1.ShowDialog();
            });
            UpdateReceivingFormCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await PhieuNhapSachBLL.Instance.UpdatePhieuNhapSach(ReceivingFormSeleted));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataReceivingFormCM.Execute(null);
                    p.Close();
                }
            });
            
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
