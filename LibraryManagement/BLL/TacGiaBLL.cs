using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.DAL;
using LibraryManagement.DTO;

namespace LibraryManagement.BLL
{
    public class TacGiaBLL
    {
        private static TacGiaBLL instance;
        public static TacGiaBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TacGiaBLL();
                }
                return instance;
            }
        }
        public async Task<List<TACGIA>> GetAllTacGia()
        {
            return await TacGiaDAL.Instance.GetAllTacGia();
        }
        public async Task<TACGIA> GetTacGiaById(int id)
        {
            return await TacGiaDAL.Instance.GetTacGiaById(id);
        }
        public async Task<List<TACGIA>> GetTacGiaByMa(string MaTG)
        {
            return await TacGiaDAL.Instance.GetTacGiaByMa(MaTG);
        }
        public async Task<List<TACGIA>> GetTacGiaByTen(string TenTG)
        {
            return await TacGiaDAL.Instance.GetTacGiaByTen(TenTG);
        }
        public async Task<(bool, string)> AddTacGia(TACGIA tg)
        {
            return await TacGiaDAL.Instance.AddTacGia(tg);
        }
        public async Task<(bool, string)> UpdateTacGia(TACGIA tg)
        {
            var tacgia = await TacGiaDAL.Instance.GetTacGiaById(tg.id);
            if(tacgia == null)
            {
                return (false, "Tác giả không tồn tại");
            }
            return await TacGiaDAL.Instance.UpdateTacGia(tg);
        }
        public async Task<(bool, string)> DeleteTacGia(int id)
        {
            var tacgia = await TacGiaDAL.Instance.GetTacGiaById(id);
            if (tacgia == null)
            {
                return (false, "Tác giả không tồn tại");
            }
            return await TacGiaDAL.Instance.DeleteTacGia(id);
        }
    }
}
