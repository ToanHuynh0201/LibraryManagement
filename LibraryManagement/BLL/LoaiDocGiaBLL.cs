using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BLL
{
    public class LoaiDocGiaBLL
    {
        private static LoaiDocGiaBLL instance;
        public static LoaiDocGiaBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoaiDocGiaBLL();
                }
                return instance;
            }
        }
        public async Task<List<LOAIDOCGIA>> GetAllLoaiDocGia()
        {
            return await LoaiDocGiaDAL.Instance.GetAllLoaiDocGia();
        }
        public async Task<LOAIDOCGIA> GetLoaiDocGiaById(int id)
        {
            return await LoaiDocGiaDAL.Instance.GetLoaiDocGiaById(id);
        }
        public async Task<List<LOAIDOCGIA>> GetLoaiDocGiaByTen(string tenldg)
        {
            return await LoaiDocGiaDAL.Instance.GetLoaiDocGiaByTen(tenldg);
        }
        public async Task<(bool, string)> AddLoaiDocGia(LOAIDOCGIA ldg)
        {
            if (String.IsNullOrEmpty(ldg.TenLoaiDG))
            {
                return (false, "Tên loại độc giả không được để trống");
            }
            return await LoaiDocGiaDAL.Instance.AddLoaiDocGia(ldg);
        }
        public async Task<(bool, string)> UpdateLoaiDocGia(LOAIDOCGIA ldg)
        {
            var loaidocgia = await LoaiDocGiaDAL.Instance.GetLoaiDocGiaById(ldg.id);
            if (loaidocgia == null)
            {
                return (false, "Loại độc giả không tồn tại");
            }
            if(String.IsNullOrEmpty(ldg.TenLoaiDG))
            {
                return (false, "Tên loại độc giả không được để trống");
            }
            return await LoaiDocGiaDAL.Instance.UpdateLoaiDocGia(ldg);
        }
        public async Task<(bool, string)> DeleteLoaiDocGia(int id)
        {
            var loaidocgia = await LoaiDocGiaDAL.Instance.GetLoaiDocGiaById(id);
            if (loaidocgia == null)
            {
                return (false, "Loại độc giả không tồn tại");
            }
            return await LoaiDocGiaDAL.Instance.DeleteLoaiDocGia(id);
        }
    }
}
