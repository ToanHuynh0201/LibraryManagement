using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryManagement.BLL
{
    public class CuonSachBLL
    {
        private static CuonSachBLL instance;
        public static CuonSachBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CuonSachBLL();
                }
                return instance;
            }
        }
        public async Task<List<CUONSACH>> GetAllCuonSach()
        {
            return await CuonSachDAL.Instance.GetAllCuonSach();
        }
        public async Task<List<CUONSACH>> GetAllCuonSachChuaMuon()
        {
            return await CuonSachDAL.Instance.GetAllCuonSachChuaMuon();
        }
        public async Task<CUONSACH> GetCuonSachById(int id)
        {
            return await CuonSachDAL.Instance.GetCuonSachById(id);
        }
        public async Task<List<CUONSACH>> GetCuonSachChuaDuocMuonByMaCuonSach(string macuonsach)
        {
            return await CuonSachDAL.Instance.GetAllCuonSachChuaMuonByMaCuonSach(macuonsach);
        }
        public async Task<List<CUONSACH>> GetCuonSachByMaSach(int MaSach)
        {
            return await CuonSachDAL.Instance.GetCuonSachByMaSach(MaSach);
        }
        public async Task<List<CUONSACH>> GetCuonSachChuaDuocMuonByMaSach(int MaSach)
        {
            return await CuonSachDAL.Instance.GetCuonSachChuaDuocMuonByMaSach(MaSach);
        }
        public async Task<List<CUONSACH>> GetCuonSachByMaSach(string masach)
        {
            return await CuonSachDAL.Instance.GetCuonSachByMaSach(masach);
        }
        public async Task<List<CUONSACH>> GetCuonSachByMaDauSach(string madausach)
        {
            return await CuonSachDAL.Instance.GetCuonSachByMaDauSach(madausach);
        }
        public async Task<List<CUONSACH>> GetCuonSachByTenDauSach(string tendausach)
        {
            return await CuonSachDAL.Instance.GetCuonSachByTenDauSach(tendausach);
        }
        public async Task<(bool, string)> UpdateCuonSach(CUONSACH cs)
        {
            var sach = await SachDAL.Instance.GetSachById(cs.MaSach);
            if (sach == null)
            {
                return (false, "Sách không tồn tại.");
            }
            var cuonsach = await CuonSachDAL.Instance.GetCuonSachById(cs.id);
            if (cuonsach == null)
            {
                return (false, "Cuốn sách không tồn tại.");
            }
            return await CuonSachDAL.Instance.UpdateCuonSach(cs);
        }
        public async Task<(bool, string)> DeleteCuonSach(int id)
        {
            var cuonsach = await CuonSachDAL.Instance.GetCuonSachById(id);
            if (cuonsach == null || cuonsach.IsDeleted == true)
            {
                return (false, "Cuốn sách không tồn tại.");
            }
            if (cuonsach.TinhTrang == true)
            {
                return (false, "Cuốn sách đang được mượn, không thể ẩn.");
            }
            return await CuonSachDAL.Instance.DeleteCuonSach(id);
        }
    }
}
