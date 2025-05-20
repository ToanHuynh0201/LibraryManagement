using LibraryManagement.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
            bool isSuccess = false;
            string message = "";

            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var context = new LibraryManagementEntities())
                    {
                        context.CT_PHIEUNHAPSACH.Add(ctpns);
                        PHIEUNHAPSACH pns = await context.PHIEUNHAPSACHes.FindAsync(ctpns.SoPNS);
                        pns.TongTien += ctpns.ThanhTien;
                        await context.SaveChangesAsync();
                    }
                    var (status, msg) = await SachDAL.Instance.AddExistingSach(ctpns.MaSach, ctpns.SoLuongNhap);
                    if (!status)
                    {
                        throw new InvalidOperationException("Lỗi khi cập nhật sách: " + msg);
                    }
                    scope.Complete();
                }
                return (true, "Nhập thành công");
            }
            catch
            {
                return (false, "Nhập thất bại: " + message);
            }
        }
    }
}
