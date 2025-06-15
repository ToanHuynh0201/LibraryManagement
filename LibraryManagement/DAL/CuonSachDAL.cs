using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LibraryManagement.DAL
{
    public class CuonSachDAL
    {
        private static CuonSachDAL instance;
        public static CuonSachDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CuonSachDAL();
                }
                return instance;
            }
        }
        public async Task<List<CUONSACH>> GetAllCuonSach()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.AsNoTracking().Include(cs => cs.SACH).Include(cs => cs.SACH.DAUSACH).Where(cs => cs.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<List<CUONSACH>> GetAllCuonSachChuaMuon()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.AsNoTracking().Include(cs => cs.SACH).Include(cs => cs.SACH.DAUSACH).Where(cs => cs.IsDeleted == false && cs.TinhTrang == false).ToListAsync();
            }
        }
        public async Task<List<CUONSACH>> GetAllCuonSachChuaMuonByMaCuonSach(string macuonsach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.AsNoTracking().Include(cs => cs.SACH).Include(cs => cs.SACH.DAUSACH).Where(cs => cs.IsDeleted == false && cs.TinhTrang == false && cs.MaCuonSach.Contains(macuonsach)).ToListAsync();
            }
        }
        public async Task<CUONSACH> GetCuonSachById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.Include(cs => cs.SACH).Include(cs => cs.SACH.DAUSACH).AsNoTracking()
                .FirstOrDefaultAsync(cs => cs.id == id && cs.IsDeleted == false);
            }
        }
        public async Task<List<CUONSACH>> GetCuonSachByMaSach(int masach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.Include(cs => cs.SACH).Include(cs => cs.SACH.DAUSACH).AsNoTracking()
                .Where(cs => cs.MaSach == masach && cs.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<List<CUONSACH>> GetCuonSachChuaDuocMuonByMaSach(int masach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.Include(cs => cs.SACH).Include(cs => cs.SACH.DAUSACH).AsNoTracking()
                .Where(cs => cs.MaSach == masach && cs.TinhTrang == false
                && cs.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<List<CUONSACH>> GetCuonSachByMaSach(string masach)
        {
            using (var context = new LibraryManagementEntities())
            {
                List<SACH> dssach = await context.SACHes.Where(s => s.MaSach.Contains(masach)).ToListAsync();
                List<int> maSachList = dssach.Select(s => s.id).ToList();
                List<CUONSACH> dscs = await context.CUONSACHes.Include(cs => cs.SACH).Include(cs => cs.SACH.DAUSACH)
                                            .Where(cs => maSachList.Contains(cs.MaSach) && cs.IsDeleted == false)
                                            .ToListAsync();
                return dscs;
            }
        }
        public async Task<List<CUONSACH>> GetCuonSachByMaDauSach(string madausach)
        {
            using (var context = new LibraryManagementEntities())
            {
                List<DAUSACH> dsdausach = await context.DAUSACHes.Where(ds => ds.MaDauSach.Contains(madausach)).ToListAsync();
                List<int> maDauSachList = dsdausach.Select(ds => ds.id).ToList();
                List<CUONSACH> dscs = await context.CUONSACHes.Include(cs => cs.SACH).Include(cs => cs.SACH.DAUSACH)
                                            .Where(cs => maDauSachList.Contains(cs.SACH.MaDauSach) && cs.IsDeleted == false)
                                            .ToListAsync();
                return dscs;
            }
        }
        public async Task<List<CUONSACH>> GetCuonSachByTenDauSach(string tendausach)
        {
            using (var context = new LibraryManagementEntities())
            {
                List<DAUSACH> dsdausach = await context.DAUSACHes.Where(ds => ds.TenDauSach.Contains(tendausach)).ToListAsync();
                List<int> maDauSachList = dsdausach.Select(ds => ds.id).ToList();
                List<CUONSACH> dscs = await context.CUONSACHes.Include(cs => cs.SACH).Include(cs => cs.SACH.DAUSACH)
                                            .Where(cs => maDauSachList.Contains(cs.SACH.MaDauSach) && cs.IsDeleted == false)
                                            .ToListAsync();
                return dscs;
            }
        }
        public async Task<(bool, string)> UpdateCuonSach(CUONSACH cs)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var cuonsach = await context.CUONSACHes.FindAsync(cs.id);
                    cuonsach.TinhTrang = cs.TinhTrang;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật trạng thái cuốn sách thành công.");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
        public async Task<(bool, string)> DeleteCuonSach(int id)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var cuonsach = await context.CUONSACHes.FindAsync(id);
                    var sach = await context.SACHes.FindAsync(cuonsach.MaSach);
                    cuonsach.IsDeleted = true;
                    sach.SoLuong--;
                    sach.SoLuongCon--;
                    await context.SaveChangesAsync();
                    return (true, "Đã ẩn cuốn sách.");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
    }
}
