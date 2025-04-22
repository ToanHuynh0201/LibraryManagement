using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class ThamSoDAL
    {
        private static ThamSoDAL instance;
        public static ThamSoDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ThamSoDAL();
                }
                return instance;
            }
        }

        public async Task<THAMSO> GetThamSo()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.THAMSOes.AsNoTracking().FirstOrDefaultAsync();
            }
        }

        public async Task<(bool, string)> UpdateThamSo(THAMSO ts)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    THAMSO thamso = await context.THAMSOes.AsNoTracking().FirstOrDefaultAsync();
                    thamso.TuoiDGToiDa = ts.TuoiDGToiDa;
                    thamso.TuoiDGToiThieu = ts.TuoiDGToiThieu;
                    thamso.GiaTriThe = ts.GiaTriThe;
                    thamso.KhoangCachNamXB = ts.KhoangCachNamXB;
                    thamso.SoSachMuonToiDa = ts.SoSachMuonToiDa;
                    thamso.SoNgayMuonToiDa = ts.SoNgayMuonToiDa;
                    thamso.TienPhatTre = ts.TienPhatTre;
                    thamso.ApDungQDThuPhat = ts.ApDungQDThuPhat;

                    await context.SaveChangesAsync();
                    return (true, "Cập nhật tham số thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
    }
}
