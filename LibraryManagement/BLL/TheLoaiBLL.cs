using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BLL
{
    public class TheLoaiBLL
    {
        private static TheLoaiBLL instance;
        public static TheLoaiBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TheLoaiBLL();
                }
                return instance;
            }
        }
        public async Task<List<THELOAI>> GetAllTheLoai()
        {
            return await TheLoaiDAL.Instance.GetAllTheLoai();
        }
        public async Task<THELOAI> GetTheLoaiById(int id)
        {
            return await TheLoaiDAL.Instance.GetTheLoaiById(id);
        }
        public async Task<List<THELOAI>> GetTheLoaiByMa(string MaTheLoai)
        {
            return await TheLoaiDAL.Instance.GetTheLoaiByMa(MaTheLoai);
        }
        public async Task<List<THELOAI>> GetTheLoaiByTen(string TenTheLoai)
        {
            return await TheLoaiDAL.Instance.GetTheLoaiByTen(TenTheLoai);
        }
        public async Task<(bool, string)> AddTheLoai(THELOAI tl)
        {
            return await TheLoaiDAL.Instance.AddTheLoai(tl);
        }
        public async Task<(bool, string)> UpdateTheLoai(THELOAI tl)
        {
            var theloai = await TheLoaiDAL.Instance.GetTheLoaiById(tl.id);
            if(theloai == null)
            {
                return (false, "Thể loại không tồn tại");
            }
            return await TheLoaiDAL.Instance.UpdateTheLoai(tl);
        }
        public async Task<(bool, string)> DeleteTheLoai(int id)
        {
            var theloai = await TheLoaiDAL.Instance.GetTheLoaiById(id);
            if (theloai == null)
            {
                return (false, "Thể loại không tồn tại");
            }
            return await TheLoaiDAL.Instance.DeleteTheLoai(id);
        }
    }
}
