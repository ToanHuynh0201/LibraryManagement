using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.DTO;

namespace LibraryManagement.DAL
{
    public class BCTongLuotMuonDAL
    {
        private static BCTongLuotMuonDAL instance;
        public static BCTongLuotMuonDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BCTongLuotMuonDAL();
                }
                return instance;
            }
        }

        public async Task<List<BCTONGLUOTMUON>> GetAllBaoCaoTongLuotMuon()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.BCTONGLUOTMUONs
                            .Include(bc => bc.BCLUOTMUONTHEOTHELOAIs)
                            .Include(bc => bc.BCLUOTMUONTHEOTHELOAIs.Select(ct => ct.THELOAI))
                            .OrderByDescending(bc => bc.Nam).ThenByDescending(bc => bc.Thang)
                            .ToListAsync();
            }
        }
        public async Task<BCTONGLUOTMUON> GetBaoCaoByThangNam(int thang, int nam)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.BCTONGLUOTMUONs
                                    .Include(bc => bc.BCLUOTMUONTHEOTHELOAIs)
                                    .Include(bc => bc.BCLUOTMUONTHEOTHELOAIs.Select(ct => ct.THELOAI))
                                    .FirstOrDefaultAsync(bc => bc.Thang == thang && bc.Nam == nam);
            }
        }
        public async Task<List<BCLUOTMUONTHEOTHELOAI>> GetBaoCaoTheLoai(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.BCLUOTMUONTHEOTHELOAIs.Include(bctl => bctl.THELOAI)
                                    .Where(bctl => bctl.MaBaoCao == id).ToListAsync();
            }
        }
        public async Task<(bool, string)> AddBaoCao(int thang, int nam)
        {
            try
            {
                using (var context = new LibraryManagementEntities())
                {
                    var baocao = await context.BCTONGLUOTMUONs.FirstOrDefaultAsync(bc => bc.Thang == thang && bc.Nam == nam);

                    if (baocao != null)
                    {
                        var baocaotheotl = context.BCLUOTMUONTHEOTHELOAIs.Where(ct => ct.MaBaoCao == baocao.id);
                        context.BCLUOTMUONTHEOTHELOAIs.RemoveRange(baocaotheotl);
                        context.BCTONGLUOTMUONs.Remove(baocao);
                    }
                    var TongLuotMuon = await context.PHIEUMUONTRAs.Where(p => p.NgayMuon.Month == thang && p.NgayMuon.Year == nam).CountAsync();

                    if (TongLuotMuon == 0) return (false, "Không thể lập báo cáo vì chưa có người mượn sách");
                    var BaoCaoTong = new BCTONGLUOTMUON
                    {
                        Thang = thang,
                        Nam = nam,
                        TongSoLuotMuon = TongLuotMuon
                    };
                    context.BCTONGLUOTMUONs.Add(BaoCaoTong);
                    await context.SaveChangesAsync();

                    var dsTheLoai = await context.THELOAIs.ToListAsync();

                   
                    foreach (var theloai in dsTheLoai)
                    {
                        var SoLuotMuonTheoTL = await context.PHIEUMUONTRAs.Where(pmt => pmt.NgayMuon.Month == thang &&
                                                     pmt.NgayMuon.Year == nam &&
                                                     pmt.CUONSACH.SACH.DAUSACH.THELOAI.id == theloai.id)
                                                     .CountAsync();
                        float tyle = TongLuotMuon > 0 ? (float)SoLuotMuonTheoTL / TongLuotMuon * 100 : 0;

                        if (tyle > 0)
                        {
                            var bctheotheloai = new BCLUOTMUONTHEOTHELOAI
                            {
                                MaBaoCao = BaoCaoTong.id,
                                MaTheLoai = theloai.id,
                                SoLuotMuon = SoLuotMuonTheoTL,
                                TyLe = (decimal?)Math.Round(tyle, 2)
                            };

                            context.BCLUOTMUONTHEOTHELOAIs.Add(bctheotheloai);
                        }
                    }

                    await context.SaveChangesAsync();
                }

                return (true, "Tạo báo cáo thành công");
            }
            catch (Exception ex)
            {
                return (false, "Tạo báo cáo thất bại: " + ex.ToString());
            }
        }
    }
}