using LibraryManagement.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class CT_PhieuNhapSachDAL
    {
        private static CT_PhieuNhapSachDAL instance;
        public static CT_PhieuNhapSachDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CT_PhieuNhapSachDAL();
                }
                return instance;
            }
        }
        public async Task<List<CT_PHIEUNHAPSACH>> GetAllCT_PhieuNhapSach()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CT_PHIEUNHAPSACH.ToListAsync();
            }
        }
        public async Task<(bool, string)> AddCT_PhieuNhapSach(CT_PHIEUNHAPSACH ctpns)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    context.CT_PHIEUNHAPSACH.Add(ctpns);
                    await context.SaveChangesAsync();
                    return (true, "Thêm chi tiết phiếu nhập sách thành công");
                }
                catch(Exception ex)
                {
                    return (false, ex.Message);
                }
        }
    }
}
