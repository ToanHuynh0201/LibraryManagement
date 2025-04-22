using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BLL
{
    public class ThamSoBLL
    {
        private static ThamSoBLL instance;
        public static ThamSoBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ThamSoBLL();
                }
                return instance;
            }
        }
        public async Task<THAMSO> GetThamSo()
        {
            return await ThamSoDAL.Instance.GetThamSo();
        }
        public async Task<(bool, string)> UpdateThamSo(THAMSO ts)
        {
            if (ts.TuoiDGToiDa < ts.TuoiDGToiThieu)
                return (false, "Tuổi độc giả tối đa không được nhỏ hơn tuổi độc giả tối thiểu");
            if (ts.GiaTriThe < 0)
                return (false, "Giá trị thẻ không được nhỏ hơn 0");
            if (ts.KhoangCachNamXB < 0)
                return (false, "Khoảng cách năm xuất bản không được nhỏ hơn 0");
            if (ts.SoSachMuonToiDa < 0)
                return (false, "Số sách mượn tối đa không được nhỏ hơn 0");
            if (ts.SoNgayMuonToiDa < 0)
                return (false, "Số ngày mượn tối đa không được nhỏ hơn 0");
            if (ts.TienPhatTre < 0)
                return (false, "Tiền phạt trễ không được nhỏ hơn 0");
            return await ThamSoDAL.Instance.UpdateThamSo(ts);
        }
    }
}
