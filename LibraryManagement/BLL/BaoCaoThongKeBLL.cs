using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BLL
{
    public class BaoCaoThongKeBLL
    {
        private static BaoCaoThongKeBLL instance;
        public static BaoCaoThongKeBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BaoCaoThongKeBLL();
                }
                return instance;
            }
        }
        public async Task<List<BCTONGLUOTMUON>> GetAllBaoCaoTongLuotMuon()
        {
            return await BCTongLuotMuonDAL.Instance.GetAllBaoCaoTongLuotMuon();
        }
        public async Task<BCTONGLUOTMUON> GetBaoCaoByThangNam(int thang, int nam)
        {
            return await BCTongLuotMuonDAL.Instance.GetBaoCaoByThangNam(thang, nam);
        }
        public async Task<(bool, string)> AddBaoCao(int thang, int nam)
        {
            DateTime today = DateTime.Now;
            if (nam > today.Year || (nam == today.Year && thang > today.Month))
                return (false, "Không thể lập báo cáo ở tương lai");
            return await BCTongLuotMuonDAL.Instance.AddBaoCao(thang, nam);
        }
        public async Task<List<BCLUOTMUONTHEOTHELOAI>> GetBaoCaoTheLoai(int id)
        {
            return await BCTongLuotMuonDAL.Instance.GetBaoCaoTheLoai(id);
        }
        public async Task<List<BCSACHTRATRE>> GetAllBaoCaoSachTraTre()
        {
            return await BCSachTraTreDAL.Instance.GetAllBaoCaoSachTraTre();
        }
        public async Task<List<BCSACHTRATRE>> GetBaoCaoSachTraTreByNgay(DateTime ngay)
        {
            return await BCSachTraTreDAL.Instance.GetBaoCaoSachTraTreByNgay(ngay);
        }
        public async Task<(bool, string)> AddBaoCaoSachTraTre(DateTime ngay)
        {
            DateTime today = DateTime.Now;
            if (ngay.Date > today.Date)
                return (false, "Không thể lập báo cáo ở tương lai");
            return await BCSachTraTreDAL.Instance.AddBaoCaoSachTraTre(ngay);
        }
    }
}
