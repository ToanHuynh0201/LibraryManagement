using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.DTO;

namespace LibraryManagement.DAL
{
    public class BCSachTraTreDAL
    {
        private static BCSachTraTreDAL instance;
        public static BCSachTraTreDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BCSachTraTreDAL();
                }
                return instance;
            }
        }
        public async Task<List<BCSACHTRATRE>> GetAllBaoCaoSachTraTre()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.BCSACHTRATREs.Include(bc => bc.CUONSACH.SACH.DAUSACH)
                             .OrderByDescending(bc => bc.Ngay)
                             .ThenByDescending(bc => bc.SoNgayTraTre).ToListAsync();
            }
        }

        public async Task<List<BCSACHTRATRE>> GetBaoCaoSachTraTreByNgay(DateTime ngay)
        {
            using (var context = new LibraryManagementEntities())
            {
                var startDate = ngay.Date;
                var endDate = ngay.AddDays(1).Date;
                return await context.BCSACHTRATREs.Include(bc => bc.CUONSACH.SACH.DAUSACH)
                                    .Where(bc => bc.Ngay >= startDate && bc.Ngay < endDate)
                                    .OrderByDescending(bc => bc.SoNgayTraTre).ToListAsync();
            }
        }

        public async Task<(bool, string)> AddBaoCaoSachTraTre(DateTime ngay)
        {
            try
            {
                using (var context = new LibraryManagementEntities())
                {
                    var startDate = ngay.Date;
                    var endDate = ngay.AddDays(1).Date;
                    var baocao = await context.BCSACHTRATREs
                        .Where(bc => bc.Ngay >= startDate && bc.Ngay < endDate)
                        .ToListAsync();
                    context.BCSACHTRATREs.RemoveRange(baocao);

                    var phieumuontre = await context.PHIEUMUONTRAs.Where(pmt => pmt.NgayTra == null && 
                                             pmt.HanTra < startDate) .ToListAsync();

                    foreach (var pmt in phieumuontre)
                    {
                        int songaytratre = (startDate - pmt.HanTra.Date).Days;

                        var bcsachtratre = new BCSACHTRATRE
                        {
                            Ngay = ngay,
                            MaCuonSach = pmt.MaCuonSach,
                            NgayMuon = pmt.NgayMuon,
                            SoNgayTraTre = songaytratre
                        };

                        context.BCSACHTRATREs.Add(bcsachtratre);
                    }

                    await context.SaveChangesAsync();
                    return (true, "Tạo báo cáo thành công");
                }
            }
            catch (Exception ex)
            {
                return (false, "Tạo báo cáo thất bại: " + ex.ToString());
            }
        }
    }
}