using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<CUONSACH> GetCuonSachById(int id)
        {
            return await CuonSachDAL.Instance.GetCuonSachById(id);
        }
        public async Task<List<CUONSACH>> GetCuonSachByMaSach(int MaSach)
        {
            return await CuonSachDAL.Instance.GetCuonSachByMaSach(MaSach);
        }
        public async Task<List<CUONSACH>> GetCuonSachChuaDuocMuonByMaSach(int MaSach)
        {
            return await CuonSachDAL.Instance.GetCuonSachChuaDuocMuonByMaSach(MaSach);
        }
        public async Task<(bool, string)> AddCuonSach(CUONSACH cs)
        {
            var sach = await SachDAL.Instance.GetSachById(cs.MaSach);
            if (sach == null)
            {
                return (false, "Không thể thêm, sách không tồn tại.");
            }
            cs.TinhTrang = false;
            var res = await CuonSachDAL.Instance.AddCuonSach(cs);
            if (res.Item1 == false) return res;
            else
            {
                sach.SoLuong++;
                sach.SoLuongCon++;
                await SachDAL.Instance.UpdateSach(sach);
                return res;
            }
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
                return (false, "Cuốn sách đang được mượn, không thể xoá.");
            }
            var res = await CuonSachDAL.Instance.DeleteCuonSach(id);
            if (res.Item1 == true)
            {
                var sach = await SachDAL.Instance.GetSachById(cuonsach.MaSach);
                if (sach != null)
                {
                    sach.SoLuong--;
                    sach.SoLuongCon--;
                    await SachDAL.Instance.UpdateSach(sach);
                }
            }
            return res;
        }
    }
}
