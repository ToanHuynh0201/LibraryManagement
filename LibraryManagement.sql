--DROP DATABASE LibraryManagement
CREATE DATABASE LibraryManagement

GO

USE LibraryManagement

set dateformat dmy

--CREATE TABLE

GO

CREATE TABLE DOCGIA
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaDG as cast('DG' + format(id, '0000') as char(6)),
    MaLoaiDG int NOT NULL,
    TenDG nvarchar(max) NOT NULL,
    NgaySinhDG datetime NOT NULL,
    DiaChiDG nvarchar(max),
    Email varchar(max),
    NgayLapThe datetime NOT NULL,
    NgayHetHan datetime NOT NULL,
    TongNo int DEFAULT 0,
    TenDangNhap varchar(64) NOT NULL,
	IsDeleted bit DEFAULT 0
)

CREATE TABLE LOAIDOCGIA
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaLoaiDG as cast('LDG' + format(id, '000') as char(6)),
    TenLoaiDG nvarchar(max) NOT NULL
)

CREATE TABLE SACH
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaSach as cast('S' + format(id,'00000') as char(6)),
    MaDauSach int NOT NULL,
    NamXB int NOT NULL,
    NhaXB nvarchar(max) NOT NULL,
    SoLuong int DEFAULT 0,
    TriGia int NOT NULL, 
    SoLuongCon int NOT NULL,
    IsDeleted bit DEFAULT 0
)

CREATE TABLE DAUSACH
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaDauSach as cast('DS' + format(id,'00000') as char(7)),
    TenDauSach nvarchar(max) NOT NULL,
    MaTheLoai int NOT NULL,
	IsDeleted bit DEFAULT 0
)

CREATE TABLE CUONSACH
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaCuonSach as cast('CS' + format(id,'00000') as char(7)),
    MaSach int NOT NULL,
    TinhTrang bit DEFAULT 0,
    IsDeleted bit DEFAULT 0
)

CREATE TABLE THELOAI
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaTheLoai as cast('TL' + format(id, '0000') as char(6)),
    TenTheLoai nvarchar(max)
)

CREATE TABLE TACGIA
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaTG as cast('TG' + format(id, '0000') as char(6)),
    TenTG nvarchar(max) NOT NULL
)

CREATE TABLE CT_TGSACH 
(
    MaTG int,
    MaDauSach int,
    PRIMARY KEY(MaTG, MaDauSach)
)

CREATE TABLE PHIEUMUONTRA
(
    id int IDENTITY(1,1) PRIMARY KEY,
    SoPhieu as cast('PM' + FORMAT(id, '00000') as char(7)),
    MaDG int NOT NULL,
    MaCuonSach int NOT NULL,
    NgayMuon datetime NOT NULL,
    HanTra datetime NOT NULL,
    NgayTra datetime,
    SoNgayMuon int NOT NULL,
    TienPhat int DEFAULT 0
)

CREATE TABLE PHIEUTHUTIENPHAT
(
    id int IDENTITY(1,1) PRIMARY KEY,
    SoPhieuThu as cast('PT' + format(id, '00000') as char(7)),
    MaDG int NOT NULL,
    NgayThu datetime NOT NULL,
    SoTienThu int NOT NULL
)

CREATE TABLE BCSACHTRATRE
(
    Ngay datetime PRIMARY KEY,
    MaCuonSach int NOT NULL,
    NgayMuon datetime NOT NULL,
    SoNgayTraTre int DEFAULT 0
)

CREATE TABLE BCTONGLUOTMUON
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaBaoCao as cast('BCTONG' + format(id, '000') as char(9)),
    Thang int NOT NULL,
    Nam int NOT NULL,
    TongSoLuotMuon int DEFAULT 0
)

CREATE TABLE BCLUOTMUONTHEOTHELOAI
(
    MaBaoCao int,
    MaTheLoai int,
    SoLuotMuon int DEFAULT 0,
    TyLe decimal(4,2) DEFAULT 0.00,
    PRIMARY KEY(MaBaoCao, MaTheLoai)
)

CREATE TABLE PHIEUNHAPSACH
(
    id int IDENTITY(1,1) PRIMARY KEY,
    SoPNS as cast('PNS' + format(id, '0000') as char(7)),
    NgayNhap datetime NOT NULL,
    TongTien int NOT NULL
)

CREATE TABLE CT_PHIEUNHAPSACH
(
    SoPNS int, 
    MaSach int,
    SoLuongNhap int NOT NULL,
    DonGia int NOT NULL,
    ThanhTien int NOT NULL,
    PRIMARY KEY(SoPNS, MaSach)
)

CREATE TABLE NGUOIDUNG
(
    TenDangNhap varchar(64) PRIMARY KEY,
    MatKhau varchar(256) NOT NULL,
    TenNguoiDung nvarchar(max),
    MaNhom int NOT NULL
)

CREATE TABLE NHOMNGUOIDUNG
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaNhom as cast('N' + format(id, '00') as char(3)),
    TenNhom nvarchar(max)
)

CREATE TABLE CHUCNANG
(
    id int IDENTITY(1,1) PRIMARY KEY,
    MaChucNang as cast('CN' + format(id, '000') as char(5)),
    TenChucNang nvarchar(max),
    TenManHinh nvarchar(max)
)

CREATE TABLE PHANQUYEN
(
    MaNhom int,
    MaChucNang int,
    PRIMARY KEY(MaNhom, MaChucNang)
)

CREATE TABLE THAMSO
(
    TuoiDGToiDa int NOT NULL,
    TuoiDGToiThieu int NOT NULL,
    GiaTriThe int NOT NULL,
    KhoangCachNamXB int NOT NULL,
    SoSachMuonToiDa int NOT NULL,
    SoNgayMuonToiDa int NOT NULL,
    TienPhatTre int NOT NULL,
    ApDungQDThuPhat bit NOT NULL
)

--Khoá ngoại

ALTER TABLE DOCGIA ADD CONSTRAINT fk_dg_ldg FOREIGN KEY (MaLoaiDG) REFERENCES LOAIDOCGIA(id) 
ALTER TABLE DOCGIA ADD CONSTRAINT fk_dg_nd FOREIGN KEY (TenDangNhap) REFERENCES NGUOIDUNG(TenDangNhap)

ALTER TABLE DAUSACH ADD CONSTRAINT fk_tts_tl FOREIGN KEY(MaTheLoai) REFERENCES THELOAI(id)

ALTER TABLE CT_TGSACH ADD CONSTRAINT fk_tgs_tg FOREIGN KEY(MaTG) REFERENCES TACGIA(id) ON DELETE CASCADE
ALTER TABLE CT_TGSACH ADD CONSTRAINT fk_tgs_tts FOREIGN KEY(MaDauSach) REFERENCES DAUSACH(id) ON DELETE CASCADE

ALTER TABLE PHIEUMUONTRA ADD CONSTRAINT fk_pmt_dg FOREIGN KEY(MaDG) REFERENCES DOCGIA(id)
ALTER TABLE PHIEUMUONTRA ADD CONSTRAINT fk_pmt_cs FOREIGN KEY(MaCuonSach) REFERENCES CUONSACH(id)

ALTER TABLE PHIEUTHUTIENPHAT ADD CONSTRAINT fk_pttp_dg FOREIGN KEY(MaDG) REFERENCES DOCGIA(id)

ALTER TABLE BCSACHTRATRE ADD CONSTRAINT fk_bcstt_s FOREIGN KEY(MaCuonSach) REFERENCES CUONSACH(id) ON DELETE CASCADE

ALTER TABLE BCLUOTMUONTHEOTHELOAI ADD CONSTRAINT fk_blmtl_bctlm FOREIGN KEY(MaBaoCao) REFERENCES BCTONGLUOTMUON(id)
ALTER TABLE BCLUOTMUONTHEOTHELOAI ADD CONSTRAINT fk_blmtl_tl FOREIGN KEY(MaTheLoai) REFERENCES THELOAI(id) ON DELETE CASCADE

ALTER TABLE SACH ADD CONSTRAINT fk_s_tts FOREIGN KEY(MaDauSach) REFERENCES DAUSACH(id) ON DELETE CASCADE

ALTER TABLE CUONSACH add CONSTRAINT fk_cs_s FOREIGN KEY(MaSach) REFERENCES SACH(id) ON DELETE CASCADE

ALTER TABLE CT_PHIEUNHAPSACH add CONSTRAINT fk_ctpns_pns FOREIGN KEY(SoPNS) REFERENCES PHIEUNHAPSACH(id) ON DELETE CASCADE
ALTER TABLE CT_PHIEUNHAPSACH add CONSTRAINT fk_ctpns_s FOREIGN KEY(MaSach) REFERENCES SACH(id)

ALTER TABLE NGUOIDUNG ADD CONSTRAINT fk_nd_nnd FOREIGN KEY(MaNhom) REFERENCES NHOMNGUOIDUNG(id) ON DELETE CASCADE

ALTER TABLE PHANQUYEN ADD CONSTRAINT fk_pq_nnd FOREIGN KEY(MaNhom) REFERENCES NHOMNGUOIDUNG(id) ON DELETE CASCADE
ALTER TABLE PHANQUYEN ADD CONSTRAINT fk_pq_cn FOREIGN KEY(MaChucNang) REFERENCES CHUCNANG(id) ON DELETE CASCADE

--Insert sample data

GO

INSERT INTO NHOMNGUOIDUNG(TenNhom) VALUES ('Admin')
INSERT INTO NHOMNGUOIDUNG(TenNhom) VALUES (N'Thủ thư')
INSERT INTO NHOMNGUOIDUNG(TenNhom) VALUES (N'Độc giả')

GO

insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('QLND', N'Quản lý người dùng')
insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('QLDG', N'Quản lý độc giả')
insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('QLS', N'Quản lý sách')
insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('QLPMT', N'Quản lý phiếu mượn trả')
insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('QLPT', N'Quản lý phiếu thu')
insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('BCTK', N'Báo cáo - Thống kê')
insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('TDQD', N'Thay đổi quy định')
insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('TCS', N'Tra cứu sách')
insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('LSMT', N'Lịch sử mượn trả')
insert into CHUCNANG (TenChucNang, TenManHinh) VALUES ('TTDG', N'Thông tin độc giả')

GO

INSERT INTO PHANQUYEN VALUES (1, 1)
INSERT INTO PHANQUYEN VALUES (1, 2)
INSERT INTO PHANQUYEN VALUES (1, 3)
INSERT INTO PHANQUYEN VALUES (1, 4)
INSERT INTO PHANQUYEN VALUES (1, 5)
INSERT INTO PHANQUYEN VALUES (1, 6)
INSERT INTO PHANQUYEN VALUES (1, 7)

INSERT INTO PHANQUYEN VALUES (2, 2)
INSERT INTO PHANQUYEN VALUES (2, 3)
INSERT INTO PHANQUYEN VALUES (2, 4)
INSERT INTO PHANQUYEN VALUES (2, 5)
INSERT INTO PHANQUYEN VALUES (2, 6)

INSERT INTO PHANQUYEN VALUES (3, 8)
INSERT INTO PHANQUYEN VALUES (3, 9)
INSERT INTO PHANQUYEN VALUES (3, 10)

GO

INSERT INTO NGUOIDUNG VALUES ('admin', '123456', N'Admin', 1)

INSERT INTO NGUOIDUNG VALUES ('tt1', '123456', N'Thủ thư A', 2)

INSERT INTO NGUOIDUNG VALUES ('dg1', '123456', N'Ngô Quang Tiến', 3)
INSERT INTO NGUOIDUNG VALUES ('dg2', '123456', N'Bùi Lê Nhật Tri', 3)
INSERT INTO NGUOIDUNG VALUES ('dg3', '123456', N'Võ Minh Tiến', 3)
INSERT INTO NGUOIDUNG VALUES ('dg4', '123456', N'Huỳnh Nhật Toàn', 3)
INSERT INTO NGUOIDUNG VALUES ('dg5', '123456', N'Nguyễn Văn Linh', 3)

GO

INSERT INTO THELOAI VALUES (N'Sách chuyên ngành')
INSERT INTO THELOAI VALUES (N'Sách tham khảo')
INSERT INTO THELOAI VALUES (N'Giáo trình')
INSERT INTO THELOAI VALUES (N'Tiểu thuyết')
INSERT INTO THELOAI VALUES (N'Khoá luận')
INSERT INTO THELOAI VALUES (N'Khác')

GO

INSERT INTO DAUSACH (TenDauSach, MaTheLoai) VALUES (N'Công nghệ phần mềm chuyên sâu',1)
INSERT INTO DAUSACH (TenDauSach, MaTheLoai) VALUES (N'Cơ sở dữ liệu nâng cao',2)
INSERT INTO DAUSACH (TenDauSach, MaTheLoai) VALUES (N'Đặc tả hình thức',3)
INSERT INTO DAUSACH (TenDauSach, MaTheLoai) VALUES (N'Giết con chim nhại',4)
INSERT INTO DAUSACH (TenDauSach, MaTheLoai) VALUES (N'5 centimet trên giây',4)
INSERT INTO DAUSACH (TenDauSach, MaTheLoai) VALUES (N'Điều kỳ diệu ở tiệm tạp hoá Namiya',4)

GO

INSERT INTO TACGIA (TenTG) VALUES (N'Vũ Thanh Nguyên')
INSERT INTO TACGIA (TenTG) VALUES (N'Hầu Nguyễn Thành Nam')
INSERT INTO TACGIA (TenTG) VALUES (N'Nguyễn Gia Tuấn Anh')
INSERT INTO TACGIA (TenTG) VALUES (N'Khoa CNPM - Trường ĐHCNTT')
INSERT INTO TACGIA (TenTG) VALUES (N'Happer Lee')
INSERT INTO TACGIA (TenTG) VALUES (N'Shinkai Makoto')
INSERT INTO TACGIA (TenTG) VALUES (N'Higashino Keigo')

GO

INSERT INTO CT_TGSACH (MaDauSach, MaTG) VALUES (1, 4)
INSERT INTO CT_TGSACH (MaDauSach, MaTG) VALUES (2, 3)
INSERT INTO CT_TGSACH (MaDauSach, MaTG) VALUES (3, 1)
INSERT INTO CT_TGSACH (MaDauSach, MaTG) VALUES (3, 2)
INSERT INTO CT_TGSACH (MaDauSach, MaTG) VALUES (4, 5)
INSERT INTO CT_TGSACH (MaDauSach, MaTG) VALUES (5, 6)
INSERT INTO CT_TGSACH (MaDauSach, MaTG) VALUES (6, 7)

GO

INSERT INTO LOAIDOCGIA VALUES(N'Sinh viên')
INSERT INTO LOAIDOCGIA VALUES(N'Giảng viên')
INSERT INTO LOAIDOCGIA VALUES(N'Học viên cao học')
INSERT INTO LOAIDOCGIA VALUES(N'Khác')

GO

INSERT INTO DOCGIA (TenDG, NgaySinhDG, NgayLapThe, NgayHetHan, TenDangNhap, MaLoaiDG)
VALUES (N'Ngô Quang Tiến', '01/01/2000', '21/04/2025', '21/10/2025', 'dg1', 1)
INSERT INTO DOCGIA (TenDG, NgaySinhDG, NgayLapThe, NgayHetHan, TenDangNhap, MaLoaiDG)
VALUES (N'Bùi Lê Nhật Tri', '02/02/2002', '19/04/2025', '19/10/2025', 'dg2', 1)
INSERT INTO DOCGIA (TenDG, NgaySinhDG, NgayLapThe, NgayHetHan, TenDangNhap, MaLoaiDG)
VALUES (N'Võ Minh Tiến', '03/05/2005', '02/04/2025', '02/10/2025', 'dg3', 1)
INSERT INTO DOCGIA (TenDG, NgaySinhDG, NgayLapThe, NgayHetHan, TenDangNhap, MaLoaiDG)
VALUES (N'Huỳnh Nhật Toàn', '04/07/2004', '01/04/2025', '01/10/2025', 'dg4', 1)
INSERT INTO DOCGIA (TenDG, NgaySinhDG, NgayLapThe, NgayHetHan, TenDangNhap, MaLoaiDG)
VALUES (N'Nguyễn Văn Linh', '24/09/1994', '03/01/2025', '03/07/2025', 'dg5', 3)

GO 

INSERT INTO THAMSO (TuoiDGToiThieu, TuoiDGToiDa, GiaTriThe, KhoangCachNamXB, SoSachMuonToiDa, SoNgayMuonToiDa, TienPhatTre, ApDungQDThuPhat) 
VALUES(18, 55, 6, 8, 5, 4, 1000, 1)