# Docker

Các file triển khai Docker cho đồ án:

| File | Mô tả |
|------|--------|
| `Dockerfile` | Build image ASP.NET Core 8 |
| `docker-compose.yml` | MySQL + web app |
| `README.md` | Hướng dẫn sử dụng |

File `.dockerignore` nằm ở thư mục gốc repo (yêu cầu của Docker khi build context là root).

## Chạy nhanh

Từ thư mục gốc repo:

```bash
make up
```

Hoặc:

```bash
docker compose -f docker/docker-compose.yml up --build
```

- Website: http://localhost:5080 (mặc định; đổi bằng `make up WEB_PORT=8888`)
- MySQL: service `db` (nội bộ Docker), database `recipe_website`

App tự chạy EF migration (bao gồm seed) khi khởi động.

Dừng: `Ctrl+C` hoặc `make down`

Xóa dữ liệu: `make down-v`
