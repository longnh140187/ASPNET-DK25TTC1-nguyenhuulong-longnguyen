using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeWebsite.Web.Migrations;

[DbContext(typeof(Data.AppDbContext))]
[Migration("20260625120000_SeedDummyRecipesAndBlogs")]
public partial class SeedDummyRecipesAndBlogs : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
INSERT INTO categories (id, name, slug, color, description, `order`, status, created_at, updated_at, deleted_at)
SELECT 'c1000001-0000-4000-8000-000000000001', 'Món Việt Nam', 'mon-viet-nam', '#E85D04', 'Các món ăn truyền thống Việt Nam', 1, 1, UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM categories WHERE slug = 'mon-viet-nam' AND deleted_at IS NULL);

INSERT INTO categories (id, name, slug, color, description, `order`, status, created_at, updated_at, deleted_at)
SELECT 'c1000002-0000-4000-8000-000000000002', 'Món ăn nhanh', 'mon-an-nhanh', '#2A9D8F', 'Món ăn dễ làm, phù hợp bữa trưa', 2, 1, UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM categories WHERE slug = 'mon-an-nhanh' AND deleted_at IS NULL);

INSERT INTO categories (id, name, slug, color, description, `order`, status, created_at, updated_at, deleted_at)
SELECT 'c1000003-0000-4000-8000-000000000003', 'Tráng miệng', 'mon-trang-mieng', '#F4A261', 'Bánh, chè và món ngọt', 3, 1, UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM categories WHERE slug = 'mon-trang-mieng' AND deleted_at IS NULL);
");

        migrationBuilder.Sql(@"
INSERT INTO recipes (
    id, category_id, title, slug, thumbnail_url, summary, content,
    ingredients, steps, nutrition, cooking_time_minutes, servings, difficulty,
    is_featured, status, created_at, updated_at, deleted_at
)
SELECT
    'r1000001-0000-4000-8000-000000000001',
    (SELECT id FROM categories WHERE slug = 'mon-viet-nam' AND deleted_at IS NULL LIMIT 1),
    'Phở bò tái', 'pho-bo-tai', NULL,
    'Phở bò với nước dùng trong, thịt bò tái mềm và rau thơm đầy đủ.',
    'Phở bò là món ăn biểu tượng của ẩm thực Việt Nam. Nước dùng ninh xương bò trong vắt, thơm quế hồi, ăn kèm bánh phở, thịt bò, hành, ngò gai và chanh.',
    '[{""name"":""Bánh phở"",""quantity"":""500g""},{""name"":""Thịt bò gầu"",""quantity"":""300g""},{""name"":""Hành tây"",""quantity"":""2 củ""},{""name"":""Gừng"",""quantity"":""1 củ""},{""name"":""Quế, hồi"",""quantity"":""vài cái""}]',
    '[{""order"":1,""title"":""Ninh nước dùng"",""description"":""Luộc xương bò 3-4 giờ, vớt bọt, thêm gia vị.""},{""order"":2,""title"":""Chần bánh phở"",""description"":""Trụng bánh phở 30 giây, xếp vào tô.""},{""order"":3,""title"":""Thưởng thức"",""description"":""Xếp thịt, chan nước dùng nóng, thêm rau và gia vị.""}]',
    '{""calories"":520,""protein"":32,""carbs"":65,""fat"":14}',
    180, 2, 'MEDIUM', 1, 1, UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM recipes WHERE slug = 'pho-bo-tai' AND deleted_at IS NULL);

INSERT INTO recipes (
    id, category_id, title, slug, thumbnail_url, summary, content,
    ingredients, steps, nutrition, cooking_time_minutes, servings, difficulty,
    is_featured, status, created_at, updated_at, deleted_at
)
SELECT
    'r1000002-0000-4000-8000-000000000002',
    (SELECT id FROM categories WHERE slug = 'mon-viet-nam' AND deleted_at IS NULL LIMIT 1),
    'Bún chả Hà Nội', 'bun-cha-ha-noi', NULL,
    'Thịt nướng thơm lừn, chấm nước mắm chua ngọt, ăn kèm bún và rau sống.',
    'Bún chả Hà Nội gồm chả nướng than, nước chấm đậm đà và bún tươi. Món ăn hài hòa vị chua, mặn, ngọt và béo.',
    '[{""name"":""Thịt ba chỉ xay"",""quantity"":""400g""},{""name"":""Bún tươi"",""quantity"":""500g""},{""name"":""Nước mắm"",""quantity"":""3 muỗng""},{""name"":""Đường"",""quantity"":""2 muỗng""},{""name"":""Tỏi, ớt"",""quantity"":""vừa đủ""}]',
    '[{""order"":1,""title"":""Ướp thịt"",""description"":""Trộn thịt với nước mắm, đường, tỏi băm.""},{""order"":2,""title"":""Nướng chả"",""description"":""Nướng thịt trên than hồng đến khi vàng thơm.""},{""order"":3,""title"":""Pha nước chấm"",""description"":""Pha nước mắm, đường, chanh, tỏi, ớt.""}]',
    '{""calories"":480,""protein"":28,""carbs"":55,""fat"":18}',
    60, 3, 'EASY', 1, 1, UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM recipes WHERE slug = 'bun-cha-ha-noi' AND deleted_at IS NULL);

INSERT INTO recipes (
    id, category_id, title, slug, thumbnail_url, summary, content,
    ingredients, steps, nutrition, cooking_time_minutes, servings, difficulty,
    is_featured, status, created_at, updated_at, deleted_at
)
SELECT
    'r1000003-0000-4000-8000-000000000003',
    (SELECT id FROM categories WHERE slug = 'mon-an-nhanh' AND deleted_at IS NULL LIMIT 1),
    'Cơm tấm Sài Gòn', 'com-tam-sai-gon', NULL,
    'Cơm tấm thơm mỡ hành, sườn nướng mật ong và chả trứng.',
    'Cơm tấm Sài Gòn thường gồm cơm gạo tấm, sườn nướng, chả, bì, dưa leo và nước mắm pha.',
    '[{""name"":""Cơm tấm"",""quantity"":""400g""},{""name"":""Sườn heo"",""quantity"":""300g""},{""name"":""Trứng"",""quantity"":""2 quả""},{""name"":""Dưa leo"",""quantity"":""1 trái""},{""name"":""Nước mắm"",""quantity"":""2 muỗng""}]',
    '[{""order"":1,""title"":""Nấu cơm"",""description"":""Nấu cơm tấm với ít nước dừa.""},{""order"":2,""title"":""Nướng sườn"",""description"":""Ướp sườn mật ong, nướng vàng.""},{""order"":3,""title"":""Trình bày"",""description"":""Xếp cơm, sườn, chả, trứng và rau.""}]',
    '{""calories"":610,""protein"":35,""carbs"":72,""fat"":22}',
    45, 2, 'EASY', 0, 1, UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM recipes WHERE slug = 'com-tam-sai-gon' AND deleted_at IS NULL);

INSERT INTO recipes (
    id, category_id, title, slug, thumbnail_url, summary, content,
    ingredients, steps, nutrition, cooking_time_minutes, servings, difficulty,
    is_featured, status, created_at, updated_at, deleted_at
)
SELECT
    'r1000004-0000-4000-8000-000000000004',
    (SELECT id FROM categories WHERE slug = 'mon-viet-nam' AND deleted_at IS NULL LIMIT 1),
    'Gỏi cuốn tôm thịt', 'goi-cuon-tom-thit', NULL,
    'Gỏi cuốn tươi mát với tôm, thịt luộc, bún và rau thơm.',
    'Gỏi cuốn là món khai vị phổ biến, cuốn bánh tráng với tôm, thịt, bún, rau sống và chấm nước mắm pha.',
    '[{""name"":""Bánh tráng"",""quantity"":""20 cái""},{""name"":""Tôm"",""quantity"":""200g""},{""name"":""Thịt ba chỉ"",""quantity"":""200g""},{""name"":""Bún tươi"",""quantity"":""200g""},{""name"":""Rau sống"",""quantity"":""1 bó""}]',
    '[{""order"":1,""title"":""Luộc nguyên liệu"",""description"":""Luộc tôm, thịt và luộc bún.""},{""order"":2,""title"":""Cuốn gỏi"",""description"":""Cuốn bánh tráng với rau, bún, tôm, thịt.""},{""order"":3,""title"":""Pha nước chấm"",""description"":""Pha nước mắm chua ngọt.""}]',
    '{""calories"":320,""protein"":22,""carbs"":38,""fat"":8}',
    35, 4, 'EASY', 0, 1, UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM recipes WHERE slug = 'goi-cuon-tom-thit' AND deleted_at IS NULL);

INSERT INTO recipes (
    id, category_id, title, slug, thumbnail_url, summary, content,
    ingredients, steps, nutrition, cooking_time_minutes, servings, difficulty,
    is_featured, status, created_at, updated_at, deleted_at
)
SELECT
    'r1000005-0000-4000-8000-000000000005',
    (SELECT id FROM categories WHERE slug = 'mon-trang-mieng' AND deleted_at IS NULL LIMIT 1),
    'Chè ba màu', 'che-ba-mau', NULL,
    'Chè ba màu mát lạnh với đậu xanh, đậu đỏ và bột báng.',
    'Chè ba màu là món tráng miệng quen thuộc miền Nam, vị ngọt than, thơm lá dứa và nước cốt dừa.',
    '[{""name"":""Đậu xanh"",""quantity"":""100g""},{""name"":""Đậu đỏ"",""quantity"":""100g""},{""name"":""Bột báng"",""quantity"":""50g""},{""name"":""Đường phèn"",""quantity"":""150g""},{""name"":""Nước cốt dừa"",""quantity"":""400ml""}]',
    '[{""order"":1,""title"":""Nấu đậu"",""description"":""Nấu chín đậu xanh, đậu đỏ riêng.""},{""order"":2,""title"":""Nấu bột báng"",""description"":""Hòa bột báng, nấu trong.""},{""order"":3,""title"":""Hoàn thiện"",""description"":""Xếp lớp, rưới nước cốt dừa, thêm đá.""}]',
    '{""calories"":280,""protein"":8,""carbs"":58,""fat"":4}',
    90, 4, 'MEDIUM', 0, 1, UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM recipes WHERE slug = 'che-ba-mau' AND deleted_at IS NULL);

INSERT INTO recipes (
    id, category_id, title, slug, thumbnail_url, summary, content,
    ingredients, steps, nutrition, cooking_time_minutes, servings, difficulty,
    is_featured, status, created_at, updated_at, deleted_at
)
SELECT
    'r1000006-0000-4000-8000-000000000006',
    (SELECT id FROM categories WHERE slug = 'mon-viet-nam' AND deleted_at IS NULL LIMIT 1),
    'Canh chua cá lóc', 'canh-chua-ca-loc', NULL,
    'Canh chua cá lóc chua thanh, ăn kèm cơm nóng.',
    'Canh chua cá lóc kết hợp cà chua, dứa, cà rốt, me và rau thơm. Món ăn giúp kích thích vị giác, dễ ăn cả ngày hè.',
    '[{""name"":""Cá lóc"",""quantity"":""1 con""},{""name"":""Cà chua"",""quantity"":""2 quả""},{""name"":""Dứa"",""quantity"":""1/2 quả""},{""name"":""Me"",""quantity"":""2 muỗng""},{""name"":""Rau ngò om"",""quantity"":""1 bó""}]',
    '[{""order"":1,""title"":""Sơ chế"",""description"":""Làm sạch cá, cắt khúc.""},{""order"":2,""title"":""Nấu nước dùng"",""description"":""Nấu cà chua, dứa, me.""},{""order"":3,""title"":""Nấu cá"",""description"":""Cho cá vào nấu vừa chín tới, thêm rau.""}]',
    '{""calories"":250,""protein"":26,""carbs"":12,""fat"":9}',
    40, 4, 'MEDIUM', 0, 1, UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM recipes WHERE slug = 'canh-chua-ca-loc' AND deleted_at IS NULL);
");

        migrationBuilder.Sql(@"
INSERT INTO blogs (id, title, slug, thumbnail_url, summary, content, status, created_at, updated_at, deleted_at)
SELECT
    'b1000001-0000-4000-8000-000000000001',
    '5 mẹo nấu phở bò ngon như quán',
    '5-meo-nau-pho-bo-ngon-nhu-quan',
    NULL,
    'Bí quyết ninh nước dùng, chần bánh và chọn thịt bò giúp phở thơm ngon hơn.',
    'Nước dùng phở quyết định 80% hương vị món ăn. Hãy vớt bọt thường xuyên khi ninh xương, rang thơm gia vị trước khi cho vào nồi. Khi chần bánh phở, chỉ cần 20-30 giây để bánh mềm dai. Thịt bò nên thái mỏng, trụng qua nước sôi trước khi xếp tô.',
    'PUBLISHED', UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM blogs WHERE slug = '5-meo-nau-pho-bo-ngon-nhu-quan' AND deleted_at IS NULL);

INSERT INTO blogs (id, title, slug, thumbnail_url, summary, content, status, created_at, updated_at, deleted_at)
SELECT
    'b1000002-0000-4000-8000-000000000002',
    'Nguyên liệu sạch cho bữa cơm gia đình',
    'nguyen-lieu-sach-cho-bua-com-gia-dinh',
    NULL,
    'Cách chọn rau củ quả tươi và bảo quản đúng cách trong tủ lạnh.',
    'Ưu tiên mua rau củ theo mùa, chọn lá xanh tươi, không úa vàng. Rau thơm nên bọc giấy ẩm trước khi cất tủ lạnh. Thịt cá nên mua tại nguồn uy tín, kiểm tra mùi và độ tươi trước khi chế biến.',
    'PUBLISHED', UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM blogs WHERE slug = 'nguyen-lieu-sach-cho-bua-com-gia-dinh' AND deleted_at IS NULL);

INSERT INTO blogs (id, title, slug, thumbnail_url, summary, content, status, created_at, updated_at, deleted_at)
SELECT
    'b1000003-0000-4000-8000-000000000003',
    'Cách chọn cá tươi cho món canh',
    'cach-chon-ca-tuoi-cho-mon-canh',
    NULL,
    'Nhận biết cá tươi qua mắt, mùi và độ săn chắc của thịt.',
    'Cá tươi thường có mắt trong, vảy bám chắc, mang đỏ hoặc hồng tùy loại. Khi ấn nhẹ, thịt cá săn lại nhanh. Tránh chọn cá mắt đục, mùi tanh gắt hoặc da nhớt nhầy.',
    'PUBLISHED', UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM blogs WHERE slug = 'cach-chon-ca-tuoi-cho-mon-canh' AND deleted_at IS NULL);

INSERT INTO blogs (id, title, slug, thumbnail_url, summary, content, status, created_at, updated_at, deleted_at)
SELECT
    'b1000004-0000-4000-8000-000000000004',
    'Bí quyết nướng thịt mềm không bị khô',
    'bi-quyet-nuong-thit-mem-khong-bi-kho',
    NULL,
    'Ướp gia vị đủ thời gian và kiểm soát nhiệt độ lò giúp thịt nướng mềm, thơm.',
    'Nên ướp thịt ít nhất 30 phút với tỏi, dầu ăn và gia vị. Nướng ở nhiệt độ 180°C, lật đều hai mặt. Có thể quết mật ong hoặc nước dừa để giữ độ ẩm.',
    'PUBLISHED', UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM blogs WHERE slug = 'bi-quyet-nuong-thit-mem-khong-bi-kho' AND deleted_at IS NULL);

INSERT INTO blogs (id, title, slug, thumbnail_url, summary, content, status, created_at, updated_at, deleted_at)
SELECT
    'b1000005-0000-4000-8000-000000000005',
    'Gợi ý thực đơn 7 ngày cho gia đình bận rộn',
    'goi-y-thuc-don-7-ngay-cho-gia-dinh-ban-ron',
    NULL,
    'Thực đơn cân bằng dinh dưỡng, dễ nấu và tiết kiệm thời gian.',
    'Luân phiên món canh, món xào và món nướng trong tuần giúp bữa ăn đa dạng hơn. Chuẩn bị sẵn nguyên liệu cuối tuần, tận dụng thực phẩm tồn tủ lạnh để giảm lãng phí.',
    'DRAFT', UTC_TIMESTAMP(3), UTC_TIMESTAMP(3), NULL
WHERE NOT EXISTS (SELECT 1 FROM blogs WHERE slug = 'goi-y-thuc-don-7-ngay-cho-gia-dinh-ban-ron' AND deleted_at IS NULL);
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
DELETE FROM blogs WHERE id IN (
    'b1000001-0000-4000-8000-000000000001',
    'b1000002-0000-4000-8000-000000000002',
    'b1000003-0000-4000-8000-000000000003',
    'b1000004-0000-4000-8000-000000000004',
    'b1000005-0000-4000-8000-000000000005'
);

DELETE FROM recipes WHERE id IN (
    'r1000001-0000-4000-8000-000000000001',
    'r1000002-0000-4000-8000-000000000002',
    'r1000003-0000-4000-8000-000000000003',
    'r1000004-0000-4000-8000-000000000004',
    'r1000005-0000-4000-8000-000000000005',
    'r1000006-0000-4000-8000-000000000006'
);

DELETE FROM categories WHERE id IN (
    'c1000001-0000-4000-8000-000000000001',
    'c1000002-0000-4000-8000-000000000002',
    'c1000003-0000-4000-8000-000000000003'
);
");
    }
}
