using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagement.BLL
{
    public class DocGiaBLL
    {
        private static DocGiaBLL instance;
        public static DocGiaBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DocGiaBLL();
                }
                return instance;
            }
        }
        public async Task<List<DOCGIA>> GetAllDocGia()
        {
            return await DocGiaDAL.Instance.GetAllDocGia();
        }
        public async Task<DOCGIA> GetDocGiaById(int id)
        {
            return await DocGiaDAL.Instance.GetDocGiaById(id);
        }
        public async Task<List<DOCGIA>> GetDocGiaByMaDG(string madg)
        {
            return await DocGiaDAL.Instance.GetDocGiaByMaDG(madg);
        }
        public async Task<List<DOCGIA>> GetDocGiaByTenDG(string tendg)
        {
            return await DocGiaDAL.Instance.GetDocGiaByTenDG(tendg);
        }
        public async Task<DOCGIA> GetDocGiaByTenDangNhap(string tendangnhap)
        {
            return await DocGiaDAL.Instance.GetDocGiaByTenDangNhap(tendangnhap);
        }
        public async Task<(bool, string)> AddDocGia(DOCGIA dg)
        {
            var docgia = await NguoiDungDAL.Instance.GetNguoiDungByTenDNHasDeleted(dg.TenDangNhap);
            if (docgia != null)
            {
                return (false, "Tên đăng nhập đã tồn tại");
            }
            var res = CheckDocGia(dg);
            if (!res.Item1)
            {
                return (false, res.Item2);
            }

            int ThoiHanThe = ThamSoBLL.Instance.GetThamSo().Result.GiaTriThe;
            int TuoiDGToiThieu = ThamSoBLL.Instance.GetThamSo().Result.TuoiDGToiThieu;
            int TuoiDGToiDa = ThamSoBLL.Instance.GetThamSo().Result.TuoiDGToiDa;
            int TuoiDG = dg.NgayLapThe.Year - dg.NgaySinhDG.Year;
            if (dg.NgaySinhDG.Date > dg.NgayLapThe.AddYears(-TuoiDG)) TuoiDG--;

            if (TuoiDG < TuoiDGToiThieu || TuoiDG > TuoiDGToiDa)
            {
                return (false, $"Tuổi độc giả hiện tại là {TuoiDG}. Tuổi hợp lệ phải từ {TuoiDGToiThieu} đến {TuoiDGToiDa}.");
            }
            dg.NgayHetHan = dg.NgayLapThe.AddMonths(ThoiHanThe);

            dg.TongNo = 0;
            return await DocGiaDAL.Instance.AddDocGia(dg);
        }
        public async Task<(bool, string)> UpdateDocGia(DOCGIA dg)
        {
            var docgia = await DocGiaDAL.Instance.GetDocGiaById(dg.id);
            if (docgia == null)
            {
                return (false, "Độc giả không tồn tại");
            }
            var res = CheckDocGia(dg);
            if (!res.Item1)
            {
                return (false, res.Item2);
            }
            int ThoiHanThe = ThamSoBLL.Instance.GetThamSo().Result.GiaTriThe;
            int TuoiDGToiThieu = ThamSoBLL.Instance.GetThamSo().Result.TuoiDGToiThieu;
            int TuoiDGToiDa = ThamSoBLL.Instance.GetThamSo().Result.TuoiDGToiDa;
            int TuoiDG = dg.NgayLapThe.Year - dg.NgaySinhDG.Year;
            if (dg.NgaySinhDG.Date > dg.NgayLapThe.AddYears(-TuoiDG)) TuoiDG--;

            if (TuoiDG < TuoiDGToiThieu || TuoiDG > TuoiDGToiDa)
            {
                return (false, $"Tuổi độc giả hiện tại là {TuoiDG}. Tuổi hợp lệ phải từ {TuoiDGToiThieu} đến {TuoiDGToiDa}.");
            }

            if (dg.NgayHetHan.Date < dg.NgayLapThe.Date)
                return (false, "Ngày hết hạn thẻ phải trước ngày lập thẻ.");
            return await DocGiaDAL.Instance.UpdateDocGia(dg);
        }
        public async Task<(bool, string)> DeleteDocGia(int id)
        {
            var docgia = await DocGiaDAL.Instance.GetDocGiaById(id);
            if (docgia == null)
            {
                return (false, "Độc giả không tồn tại");
            }
            if (docgia.TongNo > 0) return (false, "Độc giả còn tiền nợ, không thể xoá");
            List<PHIEUMUONTRA> dspmt = await PhieuMuonTraDAL.Instance.GetPMTByMaDG(id);
            foreach (PHIEUMUONTRA pmt in dspmt)
                if (pmt.NgayTra == null)
                    return (false, "Độc giả đang có sách mượn chưa trả, không thể xoá");
            return await DocGiaDAL.Instance.DeleteDocGia(id);
        }
        private (bool, string) CheckDocGia(DOCGIA dg)
        {
            if (string.IsNullOrEmpty(dg.TenDG))
            {
                return (false, "Tên độc giả không được để trống");
            }
            var ldg = LoaiDocGiaBLL.Instance.GetLoaiDocGiaById(dg.MaLoaiDG);
            if(ldg.Result == null)
            {
                return (false, "Loại độc giả không tồn tại");
            }
            if(dg.NgaySinhDG == null)
            {
                return (false, "Ngày sinh không được để trống");
            }
            return (true, "");
        }
    }
}
