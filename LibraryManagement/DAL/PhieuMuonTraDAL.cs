using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class PhieuMuonTraDAL
    {
        private static PhieuMuonTraDAL instance;
        public static PhieuMuonTraDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new PhieuMuonTraDAL();
                return instance;
            }
        }
        public async Task<PHIEUMUONTRA> GetPMTById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUMUONTRAs.Include(pmt => pmt.CUONSACH.SACH.DAUSACH).
                                     Include(pmt => pmt.DOCGIA).FirstOrDefaultAsync(pmt => pmt.id == id);
            }
        }
        public async Task<List<PHIEUMUONTRA>> GetAllPMT()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUMUONTRAs.Include(pmt => pmt.CUONSACH.SACH.DAUSACH).
                                     Include(pmt => pmt.DOCGIA).ToListAsync();
            }
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByMaPhieu(string maphieu)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUMUONTRAs.Include(pmt => pmt.CUONSACH.SACH.DAUSACH).
                                     Include(pmt => pmt.DOCGIA).Where(pmt => pmt.SoPhieu.Contains(maphieu)).ToListAsync();
            }
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByMaDocGia(string madocgia)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUMUONTRAs.Include(pmt => pmt.CUONSACH.SACH.DAUSACH).
                                     Include(pmt => pmt.DOCGIA).Where(pmt => pmt.DOCGIA.MaDG.Contains(madocgia)).ToListAsync();
            }
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByMaCuonSach(string macuonsach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUMUONTRAs.Include(pmt => pmt.CUONSACH.SACH.DAUSACH).
                                     Include(pmt => pmt.DOCGIA).Where(pmt => pmt.CUONSACH.MaCuonSach.Contains(macuonsach)).ToListAsync();
            }
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByTenSach(string tensach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUMUONTRAs.Include(pmt => pmt.CUONSACH.SACH.DAUSACH).
                                     Include(pmt => pmt.DOCGIA).Where(pmt => pmt.CUONSACH.SACH.DAUSACH.TenDauSach.Contains(tensach)).ToListAsync();
            }
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByMaDG(int madg)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUMUONTRAs.Where(pmt => pmt.MaDG == madg).Include(pmt => pmt.CUONSACH.SACH.DAUSACH).
                                     Include(pmt => pmt.DOCGIA).ToListAsync();
            }
        }
        public async Task<List<PHIEUMUONTRA>> GetPMTByThoiGianMuon(int? ngay, int? thang, int? nam)
        {
            using (var context = new LibraryManagementEntities())
            {
                var query = context.PHIEUMUONTRAs
                                    .Include(pmt => pmt.CUONSACH.SACH.DAUSACH)
                                    .Include(pmt => pmt.DOCGIA)
                                    .AsQueryable();

                if (ngay != null)
                    query = query.Where(pmt => pmt.NgayMuon.Day == ngay.Value);

                if (thang != null)
                    query = query.Where(pmt => pmt.NgayMuon.Month == thang.Value);

                if (nam != null)
                    query = query.Where(pmt => pmt.NgayMuon.Year == nam.Value);

                return await query.ToListAsync();
            }
        }

        public async Task<List<PHIEUMUONTRA>> GetPhieuMuonTre()
        {
            using (var context = new LibraryManagementEntities())
            {
                DateTime today = DateTime.Now;
                return await context.PHIEUMUONTRAs.Where(pmt => pmt.NgayTra == null
                && pmt.HanTra < today).Include(pmt => pmt.CUONSACH.SACH.DAUSACH).Include(pmt => pmt.DOCGIA).ToListAsync();
            }
        }
        public async Task<(bool, string)> AddPhieuMuonTra(PHIEUMUONTRA phieumt)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    context.PHIEUMUONTRAs.Add(phieumt);
                    CUONSACH cuonsach = await context.CUONSACHes.FindAsync(phieumt.MaCuonSach);
                    SACH sach = await context.SACHes.FindAsync(cuonsach.MaSach);
                    sach.SoLuongCon--;
                    cuonsach.TinhTrang = true;
                    await context.SaveChangesAsync();
                    return (true, "Mượn sách thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Mượn sách thất bại: " + ex.Message);
                }
        }
        public async Task<(bool, string)> UpdatePhieuMuonTra(PHIEUMUONTRA phieumt)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    PHIEUMUONTRA pmt = await context.PHIEUMUONTRAs.FindAsync(phieumt.id);
                    pmt.HanTra = phieumt.HanTra;
                    if(phieumt.NgayTra != null)
                    {
                        CUONSACH cuonsach = await context.CUONSACHes.FindAsync(pmt.MaCuonSach);
                        SACH sach = await context.SACHes.FindAsync(cuonsach.MaSach);
                        sach.SoLuongCon++;
                        cuonsach.TinhTrang = false;
                        pmt.NgayTra = phieumt.NgayTra;
                        pmt.SoNgayMuon = phieumt.SoNgayMuon;
                        pmt.TienPhat = phieumt.TienPhat;
                        DOCGIA dg = await context.DOCGIAs.FindAsync(pmt.MaDG);
                        dg.TongNo += pmt.TienPhat;
                    }
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Cập nhật thất bại: " + ex.Message);
                }
        }
        public async Task<(bool, string)> DeletePhieuMuonTra(int id)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    PHIEUMUONTRA pmt = await context.PHIEUMUONTRAs.FindAsync(id);
                    context.PHIEUMUONTRAs.Remove(pmt);                 
                    await context.SaveChangesAsync();
                    return (true, "Xoá thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Xoá thất bại: " + ex.Message);
                }
        }
    }
}
