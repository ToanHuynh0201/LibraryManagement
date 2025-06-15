using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace LibraryManagement.ViewModel
{
    public class ReaderInformationViewModel : BaseViewModel
    {
        private DOCGIA _docgia;
        public DOCGIA docgia
        {
            get => _docgia;
            set
            {
                _docgia = value;
                OnPropertyChanged(nameof(docgia));
            }
        }

        public ObservableCollection<PHIEUMUONTRA> dsPMT { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand LoadDataCM { get; set; }
        public ReaderInformationViewModel() { }
        public ReaderInformationViewModel(DOCGIA docgiaSelected)
        {
            CloseWindowCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
            LoadDataCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                docgia = docgiaSelected;
                var data = await Task.Run(async () => await PhieuMuonTraBLL.Instance.GetPMTByMaDG(docgia.id));
                dsPMT = new ObservableCollection<PHIEUMUONTRA>(data);
            });
        }
    }
}