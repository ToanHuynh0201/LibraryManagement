using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class RuleViewModel : BaseViewModel
    {
        #region Properties
        private THAMSO _currentRule;
        public THAMSO CurrentRule
        {
            get
            {
                return _currentRule;
            }
            set
            {
                _currentRule = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<bool> ApDungList { get; set; }
        public bool SelectedApDung { get; set; }
        #endregion

        #region Commands
        public ICommand LoadRuleCM { get; set; }
        public ICommand UpdateRuleCM { get; set; }
        #endregion

        public RuleViewModel()
        {
            ApDungList = new ObservableCollection<bool>()
            {
                    true,
                    false
            };
            LoadRuleCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                CurrentRule = await ThamSoBLL.Instance.GetThamSo();
                if (CurrentRule.ApDungQDThuPhat) SelectedApDung = true;
                else SelectedApDung = false;
                OnPropertyChanged(nameof(SelectedApDung));
            });
            UpdateRuleCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                CurrentRule.ApDungQDThuPhat = SelectedApDung;
                var res = await ThamSoBLL.Instance.UpdateThamSo(CurrentRule);
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadRuleCM.Execute(null);
                }
            });
        }
    }
}
