using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagement.BLL
{
    public class PhieuThuTienPhatBLL
    {
        private static PhieuThuTienPhatBLL instance;
        public static PhieuThuTienPhatBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new PhieuThuTienPhatBLL();
                return instance;
            }
        }
        public async Task<List<PHIEUTHUTIENPHAT>> GetAllPhieuPhat()
        {
            return await PhieuThuTienPhatDAL.Instance.GetAllPhieuPhat();
        }
        public async Task<PHIEUTHUTIENPHAT> GetPhieuPhatById(int id)
        {
            return await PhieuThuTienPhatDAL.Instance.GetPhieuPhatById(id);
        }
        public async Task<List<PHIEUTHUTIENPHAT>> GetPhieuPhatByDG(int madg)
        {
            return await PhieuThuTienPhatDAL.Instance.GetPhieuPhatByDG(madg);
        }
        public async Task<(bool, string)> AddPhieuPhat(PHIEUTHUTIENPHAT phieuthu)
        {
            DOCGIA dg = await DocGiaDAL.Instance.GetDocGiaById(phieuthu.MaDG);
            if (dg == null)
                return (false, "Độc giả không hợp lệ.");
            if (phieuthu.SoTienThu <= 0)
                return (false, "Giá trị số tiền thu không hợp lệ");
            if (dg.TongNo == null) dg.TongNo = 0;
            bool apdungqdthuphat = ThamSoDAL.Instance.GetThamSo().Result.ApDungQDThuPhat;
            if (phieuthu.SoTienThu > dg.TongNo && apdungqdthuphat == true)
                return (false, "Số tiền thu không được lớn hơn tổng nợ.");
            phieuthu.NgayThu = DateTime.Now;
            return await PhieuThuTienPhatDAL.Instance.AddPhieuPhat(phieuthu);
        }
        public async Task<(bool, string)> DeletePhieuPhat(int id)
        {
            PHIEUTHUTIENPHAT pt = await PhieuThuTienPhatDAL.Instance.GetPhieuPhatById(id);
            if (pt == null)
                return (false, "Không tìm thấy phiếu thu để xoá");
            return await PhieuThuTienPhatDAL.Instance.DeletePhieuPhat(id);
        }
    }
}
