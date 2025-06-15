using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DTO
{
    public partial class DOCGIA
    {
        public string DisplayName => $"{MaDG} ({TenDG})";
        public override string ToString()
        {
            return DisplayName.ToString();
        }
        public string DisplayTongNo
        {
            get
            {
                if (TongNo == null || TongNo == 0)
                    return "Không nợ";

                if (TongNo < 0)
                    return $"Còn dư: {Math.Abs(TongNo.Value):n0}";

                return $"Còn nợ: {TongNo.Value:n0}";
            }
        }
    }
}
