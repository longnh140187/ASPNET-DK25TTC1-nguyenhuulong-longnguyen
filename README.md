# Website Công Thức Nấu Ăn

## Giới thiệu

Website chia sẻ công thức nấu ăn, món ăn Việt Nam và quốc tế.

## Công nghệ sử dụng

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- MySQL
- Bootstrap 5
- GitHub

## Chức năng

- Xem danh sách công thức nấu ăn
- Xem chi tiết món ăn
- Tìm kiếm món ăn theo từ khóa
- Lọc món ăn theo danh mục
- Khu vực quản trị (Admin) để quản lý nội dung
- Đăng nhập quản trị

## Thành viên

- Nguyễn Hữu Long
- Email: longnh140187@tvu-onschool.edu.vn
- Điện thoại: 033.2044.609

## Cấu trúc repository

```
.
├── README.md
├── setup/
├── src/
├── progress-report/
├── thesis/
│   ├── doc/
│   ├── pdf/
│   ├── html/
│   ├── abs/
│   └── refs/
├── soft/
└── docker/
```

## Hướng dẫn chạy

```bash
cd src
dotnet restore
dotnet run
```

Mở trình duyệt tại: `http://localhost:5000`
