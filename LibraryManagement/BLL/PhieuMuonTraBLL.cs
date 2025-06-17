using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagement.BLL
{
    public class PhieuMuonTraBLL
    {
        private static PhieuMuonTraBLL instance;
        public static PhieuMuonTraBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new PhieuMuonTraBLL();
                return instance;
            }
        }
        public async Task<PHIEUMUONTRA> GetPMTById(int id)
        {
            return await PhieuMuonTraDAL.Instance.GetPMTById(id);
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByMaPhieu(string maphieu)
        {
            return await PhieuMuonTraDAL.Instance.GetPMTByMaPhieu(maphieu);
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByMaDocGia(string madg)
        {
            return await PhieuMuonTraDAL.Instance.GetPMTByMaDocGia(madg);
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByMaCuonSach(string macuonsach)
        {
            return await PhieuMuonTraDAL.Instance.GetPMTByMaCuonSach(macuonsach);
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByTenSach(string tensach)
        {
            return await PhieuMuonTraDAL.Instance.GetPMTByTenSach(tensach);
        }
        public async Task<List<PHIEUMUONTRA>> GetAllPMT()
        {
            return await PhieuMuonTraDAL.Instance.GetAllPMT();
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByMaDG(int madg)
        {
            return await PhieuMuonTraDAL.Instance.GetPMTByMaDG(madg);
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByThoiGianMuon(int? ngay, int? thang, int? nam)
        {
            return await PhieuMuonTraDAL.Instance.GetPMTByThoiGianMuon(ngay, thang, nam);
        }
        public async Task<List<PHIEUMUONTRA>> GetPhieuMuonTre()
        {
            return await PhieuMuonTraDAL.Instance.GetPhieuMuonTre();
        }
        public async Task<(bool, string)> AddPhieuMuonTra(PHIEUMUONTRA phieumt)
        {
            DOCGIA docgia = await DocGiaDAL.Instance.GetDocGiaById(phieumt.MaDG);
            if (docgia == null) return (false, "Phiếu mượn không có độc giả.");
            CUONSACH cuonsach = await CuonSachDAL.Instance.GetCuonSachById(phieumt.MaCuonSach);
            if (cuonsach == null) return (false, "Phiếu mượn không có cuốn sách muốn mượn.");

            if(phieumt.NgayMuon == null || phieumt.HanTra == null)
            {
                return (false, "Phiếu mượn chưa có ngày mượn hoặc hạn trả");
            }
            DateTime today = DateTime.Now;

            if (phieumt.NgayMuon.Date > today.Date) return (false, "Ngày mượn không được sau ngày hôm nay");
                
            List<PHIEUMUONTRA> dspmt = await PhieuMuonTraDAL.Instance.GetPMTByMaDG(phieumt.MaDG);
            int slsachdangmuon = 0;
            foreach(PHIEUMUONTRA pmt in dspmt)
            {
                if (pmt.NgayTra == null)
                {
                    slsachdangmuon++;
                    if (pmt.HanTra.Date < today.Date)
                        return (false, "Độc giả đang có sách trả trễ, không thể mượn thêm sách mới.");
                }
            }

            int songaymuontoida = ThamSoBLL.Instance.GetThamSo().Result.SoNgayMuonToiDa;
            int sosaschmuontoida = ThamSoBLL.Instance.GetThamSo().Result.SoSachMuonToiDa;

            if (slsachdangmuon + 1 > sosaschmuontoida)
                return (false, "Độc giả đã mượn tối đa số lượng có thể mượn, hãy trả sách cũ trước khi mượn thêm sách mới.");

            phieumt.NgayTra = null;

            if(docgia.NgayHetHan.Date < phieumt.HanTra.Date)
                return (false, "Thẻ hết hạn trước hạn trả, không thể mượn.");

            if (cuonsach.TinhTrang == true)
                return (false, "Cuốn sách này đã được mượn, không thể mượn");
            return await PhieuMuonTraDAL.Instance.AddPhieuMuonTra(phieumt);
        }
        public async Task<(bool, string)> UpdatePhieuMuonTra(PHIEUMUONTRA phieumt)
        {
            PHIEUMUONTRA pmt = await PhieuMuonTraDAL.Instance.GetPMTById(phieumt.id);
            pmt.NgayTra = DateTime.Today;
            if (pmt == null)
                return (false, "Phiếu mượn không hợp lệ.");

            if (pmt.HanTra.Date < pmt.NgayMuon.Date)
                return (false, "Hạn trả phải sau hoặc bằng ngày mượn.");

            if (DateTime.Today.Date < pmt.NgayMuon.Date)
                return (false, "Ngày trả không được trước ngày mượn.");

            int songaymuon = (pmt.NgayTra - pmt.NgayMuon).Value.Days;
            int songaymuontoida = ThamSoBLL.Instance.GetThamSo().Result.SoNgayMuonToiDa;
            int tienphat = ThamSoBLL.Instance.GetThamSo().Result.TienPhatTre;

            pmt.SoNgayMuon = songaymuon;
            if (pmt.SoNgayMuon > songaymuontoida)
                pmt.TienPhat = (pmt.SoNgayMuon - songaymuontoida) * tienphat;
            else pmt.TienPhat = 0;
            return await PhieuMuonTraDAL.Instance.UpdatePhieuMuonTra(pmt);
        }
        public async Task<(bool, string)> DeletePhieuMuonTra(int id)
        {
            PHIEUMUONTRA pmt = await PhieuMuonTraDAL.Instance.GetPMTById(id);
            if (pmt == null)
                return (false, "Không tìm thấy phiếu mượn để xoá.");
            if (pmt.NgayTra == null)
                return (false, "Phiếu mượn chưa trả, không thể xoá.");
            return await PhieuMuonTraDAL.Instance.DeletePhieuMuonTra(id);
        }
    }
}
