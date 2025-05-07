using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

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
            SACH sach = await SachDAL.Instance.GetSachById(cuonsach.MaSach);
            if (cuonsach == null) return (false, "Phiếu mượn không sách.");

            DateTime today = DateTime.Now;
            List<PHIEUMUONTRA> dspmt = await PhieuMuonTraDAL.Instance.GetPMTByMaDG(phieumt.MaDG);
            int slsachdangmuon = 0;
            foreach(PHIEUMUONTRA pmt in dspmt)
            {
                if (pmt.NgayTra != null)
                {
                    slsachdangmuon++;
                    if (pmt.HanTra > today)
                        return (false, "Độc giả đang có sách trả trễ, không thể mượn thêm sách mới.");
                }
            }

            int songaymuontoida = ThamSoBLL.Instance.GetThamSo().Result.SoNgayMuonToiDa;
            int sosaschmuontoida = ThamSoBLL.Instance.GetThamSo().Result.SoSachMuonToiDa;

            if (slsachdangmuon > sosaschmuontoida)
                return (false, "Độc giả đã mượn tối đa số lượng có thể mượn, hãy trả sách cũ trước khi mượn thêm sách mới.");

            phieumt.NgayMuon = today;
            phieumt.HanTra = phieumt.NgayMuon.AddDays(songaymuontoida);

            if(docgia.NgayHetHan < phieumt.HanTra)
                return (false, "Thẻ hết hạn trước hạn trả, không thể mượn.");

            if (cuonsach.TinhTrang == true)
                return (false, "Cuốn sách này đã được mượn, không thể mượn");
            return await PhieuMuonTraDAL.Instance.AddPhieuMuonTra(phieumt);
        }
        public async Task<(bool, string)> UpdatePhieuMuonTra(PHIEUMUONTRA phieumt)
        {
            PHIEUMUONTRA pmt = await PhieuMuonTraDAL.Instance.GetPMTById(phieumt.id);
            if (pmt == null)
                return (false, "Phiếu mượn không hợp lệ.");
            if (phieumt.HanTra < pmt.NgayMuon)
                return (false, "Hạn trả phải sau ngày mượn.");
            if (phieumt.NgayTra != null)
            {
                if(phieumt.NgayTra < pmt.NgayMuon)
                    return (false, "Ngày trả phải sau ngày mượn.");

                int songaymuon = (phieumt.NgayTra - pmt.NgayMuon).Value.Days;
                int songaymuontoida = ThamSoBLL.Instance.GetThamSo().Result.SoNgayMuonToiDa;
                int tienphat = ThamSoBLL.Instance.GetThamSo().Result.TienPhatTre;

                phieumt.SoNgayMuon = songaymuon;
                if (phieumt.SoNgayMuon > songaymuontoida)
                    phieumt.TienPhat = (phieumt.SoNgayMuon - songaymuontoida) * tienphat;
                else phieumt.TienPhat = 0;
            }
            return await PhieuMuonTraDAL.Instance.UpdatePhieuMuonTra(phieumt);
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
