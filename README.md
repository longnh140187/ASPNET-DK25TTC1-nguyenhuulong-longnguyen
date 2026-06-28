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
├── Makefile        # make up / make down — chạy Docker nhanh
├── .dockerignore   # Docker build (context = thư mục gốc)
├── setup/          # .env.example, script seed migration
├── src/            # ASP.NET Core MVC
├── docker/         # Dockerfile, docker-compose.yml
├── progress-report/
├── thesis/
│   ├── doc/
│   ├── pdf/
│   ├── html/
│   ├── abs/
│   └── refs/
├── soft/
```

## Hướng dẫn chạy

### Chạy nhanh (chỉ cần Docker)

Yêu cầu: [Docker Desktop](https://www.docker.com/products/docker-desktop/) (hoặc Docker Engine + Compose).

```bash
make up
```

Hoặc không dùng Makefile:

```bash
docker compose -f docker/docker-compose.yml up --build
```

Sau khi container `web` sẵn sàng, mở trình duyệt: **http://localhost:5080**

(Nếu port 5080 bị chiếm: `make up WEB_PORT=8888` rồi mở `http://localhost:8888`.)

Lệnh trên tự động:
- Tạo và chạy MySQL
- Build và chạy website ASP.NET Core
- Áp dụng migration + seed dữ liệu demo (danh mục, công thức, blog, admin)

Dừng stack: `Ctrl+C`, hoặc `make down`. Xóa cả dữ liệu DB: `make down-v`.

Chi tiết thêm: thư mục `docker/`.

### Chạy local (dotnet CLI)

#### 1. Cấu hình môi trường

```bash
cp setup/.env.example .env
```

Chỉnh `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER`, `DB_PASSWORD` nếu MySQL không dùng giá trị mặc định trong `setup/.env.example`.

#### 2. Cài EF tools (lần đầu)

```bash
cd src
dotnet tool restore
```

#### 3. Tạo database và seed dữ liệu mẫu

```bash
cd src
dotnet ef database update
```

Migration `SeedFullDummyData` sẽ import toàn bộ dữ liệu demo (danh mục, công thức, blog, admin).

> Khi chạy qua Docker Compose, bước `dotnet ef database update` không cần thiết — app tự migrate khi khởi động.

#### 4. Chạy website

```bash
dotnet run
```

Mở trình duyệt tại: `http://localhost:5000`

### Đăng nhập Admin

- URL: `/manage`
- Username: `admin`
- Password: mật khẩu đã cấu hình khi tạo tài khoản admin ban đầu

### Cập nhật lại dữ liệu seed (khi chỉnh data trên máy dev)

Sau khi chỉnh sửa nội dung trong database, chạy lại:

```bash
python3 setup/generate-seed-migration.py
cd src && dotnet ef database update
```

Script sẽ export các bản ghi đang active (`deleted_at IS NULL`) vào migration `SeedFullDummyData`.
