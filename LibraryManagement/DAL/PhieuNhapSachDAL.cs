using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class PhieuNhapSachDAL
    {
        private static PhieuNhapSachDAL instance;
        public static PhieuNhapSachDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PhieuNhapSachDAL();
                }
                return instance;
            }
        }
        public async Task<List<PHIEUNHAPSACH>> GetAllPhieuNhapSach()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUNHAPSACHes.Where(pns => pns.TongTien > 0).ToListAsync();
            }
        }
        public async Task<PHIEUNHAPSACH> GetPhieuNhapSachById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUNHAPSACHes.FindAsync(id);
            }
        }
        public async Task<List<PHIEUNHAPSACH>> GetPhieuNhapSachBySoPNS(string sopns)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUNHAPSACHes.Where(pns => pns.SoPNS.Contains(sopns) && pns.TongTien > 0).ToListAsync();
            }
        }
        public async Task<List<PHIEUNHAPSACH>> GetPhieuNhapSachByNgayNhap(DateTime ngaynhap)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUNHAPSACHes.Where(pns => pns.NgayNhap == ngaynhap && pns.TongTien > 0).ToListAsync();
            }
        }
        public async Task<(bool, string)> AddPhieuNhapSach(PHIEUNHAPSACH pns)
        {
            using (var context = new LibraryManagementEntities())
            {
                pns.TongTien = 0;
                context.PHIEUNHAPSACHes.Add(pns);
                await context.SaveChangesAsync();
                return (true, "Thêm phiếu nhập sách thành công");
            }
        }
        public async Task<(bool, string)> UpdatePhieuNhapSach(PHIEUNHAPSACH pns)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    PHIEUNHAPSACH phieunhap = await context.PHIEUNHAPSACHes.FindAsync(pns.id);
                    phieunhap.NgayNhap = pns.NgayNhap;
                    phieunhap.TongTien = pns.TongTien;
                    return (true, "Cập nhật phiếu nhập thành công");
                }
                catch(Exception ex)
                {
                    return(false, ex.Message);
                }
        }
    }
}
