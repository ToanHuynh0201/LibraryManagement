using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.DAL;
using LibraryManagement.DTO;

namespace LibraryManagement.BLL
{
    public class PhieuNhapSachBLL
    {
        private static PhieuNhapSachBLL instance;
        public static PhieuNhapSachBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PhieuNhapSachBLL();
                }
                return instance;
            }
        }
        public async Task<List<PHIEUNHAPSACH>> GetAllPhieuNhapSach()
        {
            return await PhieuNhapSachDAL.Instance.GetAllPhieuNhapSach();
        }
        public async Task<List<PHIEUNHAPSACH>> GetPhieuNhapSachBySoPNS(string sophieu)
        {
            return await PhieuNhapSachDAL.Instance.GetPhieuNhapSachBySoPNS(sophieu);
        }
        public async Task<(bool, string)> AddPhieuNhapSach(PHIEUNHAPSACH pns)
        {
            pns.TongTien = 0;
            return await PhieuNhapSachDAL.Instance.AddPhieuNhapSach(pns);
        }
        public async Task<(bool, string)> UpdatePhieuNhapSach(PHIEUNHAPSACH pns)
        {
            var phieunhapsach = await PhieuNhapSachDAL.Instance.GetPhieuNhapSachById(pns.id);
            if (phieunhapsach == null)
            {
                return (false, "KhÔng tìm thấy phiếu nhập");
            }
            if (pns.NgayNhap > DateTime.Now)
            {
                return (false, "Ngày nhập không hợp lệ");
            }
            if (pns.TongTien < 0)
            {
                return (false, "Tổng tiền không hợp lệ");
            }
            return await PhieuNhapSachDAL.Instance.UpdatePhieuNhapSach(pns);
        }
    }
}
