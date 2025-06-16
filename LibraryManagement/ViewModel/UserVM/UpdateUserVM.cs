using LibraryManagement.BLL;
using LibraryManagement.DTO;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class UpdateUserViewModel : BaseViewModel
    {
        #region Properties

        private NGUOIDUNG _nguoiDung = new NGUOIDUNG();

        public NGUOIDUNG NguoiDung
        {
            get => _nguoiDung;
            set
            {
                _nguoiDung = value;
                OnPropertyChanged(nameof(NguoiDung));
            }
        }

        private NHOMNGUOIDUNG _nhomnguoidungSelected;
        public NHOMNGUOIDUNG NhomNguoiDungSelected
        {
            get { return _nhomnguoidungSelected; }
            set
            {
                _nhomnguoidungSelected = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NHOMNGUOIDUNG> ListNhomNguoiDung { get; set; }

        public Action OnSuccess { get; set; }
        #endregion

        #region Commands
        public ICommand LoadOtherDataCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand UpdateUserCM { get; set; }

        #endregion
        public UpdateUserViewModel() { }
        public UpdateUserViewModel(NGUOIDUNG UserSelected)
        {
            ListNhomNguoiDung = new ObservableCollection<NHOMNGUOIDUNG>();
            NguoiDung = UserSelected;
            OnPropertyChanged(nameof(NGUOIDUNG));
            LoadOtherDataCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await NhomNguoiDungBLL.Instance.GetAllNhomNguoiDung());
                    ListNhomNguoiDung = new ObservableCollection<NHOMNGUOIDUNG>(data);
                    OnPropertyChanged(nameof(ListNhomNguoiDung));
                    foreach (var nnd in ListNhomNguoiDung)
                        if (nnd.id == NguoiDung.MaNhom) NhomNguoiDungSelected = nnd;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách nhóm người dùng: " + ex.Message);
                }
            });

            UpdateUserCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                try
                {
                    if (NhomNguoiDungSelected != null)
                        NguoiDung.MaNhom = NhomNguoiDungSelected.id;

                    var res = await Task.Run(async () => await NguoiDungBLL.Instance.UpdateNguoiDung(NguoiDung));

                    MessageBox.Show(res.Item2);
                    if (res.Item1)
                    {
                        OnSuccess?.Invoke();
                        p.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm người dùng: " + ex.Message);
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });

        }
    }
}