using LibraryManagement.BLL;
using LibraryManagement.DTO;
using Microsoft.Xaml.Behaviors.Core;
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
    public class AddUserViewModel : BaseViewModel
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
        public ICommand AddNewUserCM { get; set; }

        #endregion

        public AddUserViewModel()
        {
            ListNhomNguoiDung = new ObservableCollection<NHOMNGUOIDUNG>();
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });

            LoadOtherDataCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await NhomNguoiDungBLL.Instance.GetAllNhomNguoiDung());
                    ListNhomNguoiDung = new ObservableCollection<NHOMNGUOIDUNG>(data);
                    OnPropertyChanged(nameof(ListNhomNguoiDung));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách nhóm người dùng: " + ex.Message);
                }
            });

            AddNewUserCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                try
                {
                    if (NhomNguoiDungSelected != null)
                        NguoiDung.MaNhom = NhomNguoiDungSelected.id;
                    var res = await Task.Run(async () => await NguoiDungBLL.Instance.AddNguoiDung(NguoiDung));

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
        }
    }
}