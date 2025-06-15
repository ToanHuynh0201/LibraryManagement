using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class PhieuThuTienPhatDAL
    {
        private static PhieuThuTienPhatDAL instance;
        public static PhieuThuTienPhatDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new PhieuThuTienPhatDAL();
                return instance;
            }
        }
        public async Task<List<PHIEUTHUTIENPHAT>> GetAllPhieuPhat()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUTHUTIENPHATs.Include(pt => pt.DOCGIA).ToListAsync();
            }
        }
        public async Task<PHIEUTHUTIENPHAT> GetPhieuPhatById(int id)
        {
            using(var context = new LibraryManagementEntities())
            {
                return await context.PHIEUTHUTIENPHATs.Include(pt => pt.DOCGIA).Where(pt => pt.id == id).FirstOrDefaultAsync();
            }
        }
        public async Task<List<PHIEUTHUTIENPHAT>> GetPhieuPhatByDG(int madg)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.PHIEUTHUTIENPHATs.Include(pt => pt.DOCGIA).Where(pp => pp.MaDG == madg).ToListAsync();
            }
        }
        public async Task<(bool, string)> AddPhieuPhat(PHIEUTHUTIENPHAT phieuthu)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    DOCGIA dg = await context.DOCGIAs.FindAsync(phieuthu.MaDG);
                    if (dg.TongNo == null) dg.TongNo = 0;
                    dg.TongNo -= phieuthu.SoTienThu;
                    context.PHIEUTHUTIENPHATs.Add(phieuthu);
                    await context.SaveChangesAsync();
                    return (true, "Đã thu.");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
        public async Task<(bool, string)> DeletePhieuPhat(int id)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    PHIEUTHUTIENPHAT pt = await context.PHIEUTHUTIENPHATs.FindAsync(id);
                    context.PHIEUTHUTIENPHATs.Remove(pt);
                    await context.SaveChangesAsync();
                    return (true, "Đã xoá.");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
    }
}
