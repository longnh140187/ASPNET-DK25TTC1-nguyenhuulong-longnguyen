# Setup

Tài nguyên cài đặt và chạy lại môi trường dev.

| File | Mô tả |
|------|--------|
| `.env.example` | Mẫu biến môi trường MySQL — copy ra `.env` ở thư mục gốc repo |
| `generate-seed-migration.py` | Export dữ liệu DB active vào migration `SeedFullDummyData` |

## Cấu hình môi trường local

```bash
cp setup/.env.example .env
```

Chỉnh `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER`, `DB_PASSWORD` nếu cần.

## Cập nhật seed migration

```bash
python3 setup/generate-seed-migration.py
cd src && dotnet ef database update
```
