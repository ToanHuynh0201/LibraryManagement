using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.DAL;
using LibraryManagement.DTO;

namespace LibraryManagement.BLL
{
    public class CT_PhieuNhapSachBLL
    {
        private static CT_PhieuNhapSachBLL instance;
        public static CT_PhieuNhapSachBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CT_PhieuNhapSachBLL();
                }
                return instance;
            }
        }
        public async Task<List<CT_PHIEUNHAPSACH>> GetAllCT_PhieuNhapSach()
        {
            return await CT_PhieuNhapSachDAL.Instance.GetAllCT_PhieuNhapSach();
        }
        public async Task<List<CT_PHIEUNHAPSACH>> GetCT_PNSByPhieuNhapSach(PHIEUNHAPSACH pns)
        {
            return await CT_PhieuNhapSachDAL.Instance.GetCT_PNSByPhieuNhapSach(pns);
        }
        public async Task<(bool, string)> AddCT_PhieuNhapSach(CT_PHIEUNHAPSACH ctpns)
        {
            var phieunhapsach = await PhieuNhapSachDAL.Instance.GetPhieuNhapSachById(ctpns.SoPNS);
            var sach = await SachDAL.Instance.GetSachById(ctpns.MaSach);
            if (phieunhapsach == null)
            {
                return (false, "Không tìm thấy phiếu nhập sách");
            }
            if (sach == null)
            {
                return (false, "Không tìm thấy sách đang nhập");
            }
            if(ctpns.SoLuongNhap <= 0)
            {
                return (false, "Số lượng sách nhập không hợp lệ");
            }
            if (ctpns.DonGia < 0)
            {
                return (false, "Giá nhập không hợp lệ");
            }
            ctpns.ThanhTien = ctpns.SoLuongNhap * ctpns.DonGia;
            return await CT_PhieuNhapSachDAL.Instance.AddCT_PhieuNhapSach(ctpns);
        }
    }
}
