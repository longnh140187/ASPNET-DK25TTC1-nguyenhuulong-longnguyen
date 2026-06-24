namespace RecipeWebsite.Web.ViewModels;

public class RecipeDetailViewModel
{
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string SubCategoryName { get; set; } = string.Empty;
    public int CookingTimeMinutes { get; set; }
    public string DifficultyLabel { get; set; } = string.Empty;
    public string RatingLabel { get; set; } = string.Empty;
    public bool IsPopular { get; set; }
    public IReadOnlyList<string> Ingredients { get; set; } = [];
    public IReadOnlyList<RecipeStepViewModel> Steps { get; set; } = [];
    public RecipeNutritionViewModel? Nutrition { get; set; }
    public string? ChefTip { get; set; }

    public static RecipeDetailViewModel CreateSample(string slug) => new()
    {
        Slug = slug,
        Title = "Thịt Kho Tàu Đậm Đà",
        Summary = "Một món ăn truyền thống không thể thiếu trong mâm cơm gia đình Việt, đặc biệt là dịp Tết. Thịt ba chỉ mềm tan kết hợp cùng trứng vịt thấm vị trong nước dừa tươi ngọt thanh, tạo nên hương vị đặc trưng khó quên.",
        ThumbnailUrl = "https://lh3.googleusercontent.com/aida/AP1WRLs3jJ_BFPCqPKaYBr_0CvneF4cIqs7mZ2Aa6GRiA5BD0TSaQbIKMYyX3ExScY1HBXTNZjYI0lmodgqPscMSOje3qoZUcdmFBSD4qJQykQcXJkv8rzr5rwsjFJ0ZdV3EyCox5rXgjTjksMEdm9XJM9mYlREJAK2F7wZsxklohEVklCu6foEoyURuXVVTxRg73ai5wDs6cYIsRF5OIRIpnPe6XOw3lMKYXM4YuIHpqDp_jrEtjjrHBB4q6mo",
        CategoryName = "Món chính",
        SubCategoryName = "Món mặn",
        CookingTimeMinutes = 45,
        DifficultyLabel = "Dễ",
        RatingLabel = "4.8 (1.2k lượt)",
        IsPopular = true,
        Ingredients =
        [
            "500g Thịt ba chỉ heo sạch",
            "4 Trứng vịt (hoặc trứng gà) luộc chín",
            "500ml Nước dừa xiêm tươi",
            "3 muỗng canh Nước mắm ngon",
            "2 muỗng canh Đường phèn (hoặc đường cát)",
            "3 củ Hành tím, 1 củ Tỏi băm nhuyễn",
            "Muối, Tiêu, Ớt tươi"
        ],
        Steps =
        [
            new("Sơ chế thịt và trứng", "Thịt ba chỉ rửa sạch với nước muối loãng, để ráo rồi thái miếng vuông vừa ăn (khoảng 3-4cm). Trứng vịt luộc chín, bóc vỏ sạch sẽ. Dùng nĩa hoặc tăm xăm nhẹ quanh quả trứng để khi kho thấm gia vị hơn."),
            new("Ướp thịt đậm đà", "Ướp thịt với hành tỏi băm, nước mắm, đường, tiêu và một ít hạt nêm. Để thịt thấm gia vị trong ít nhất 30 phút. Nếu có thời gian, hãy phơi thịt ra nắng khoảng 1 tiếng để mỡ thịt trong và giòn hơn sau khi kho."),
            new("Thắng nước màu (Caramel)", "Cho một ít dầu và đường vào nồi, đun lửa nhỏ cho đến khi đường tan và chuyển sang màu cánh gián đẹp mắt. Cho thịt đã ướp vào đảo đều tay cho đến khi thịt săn lại và thấm màu vàng nâu."),
            new("Kho thịt với nước dừa", "Đổ nước dừa tươi vào nồi cho ngập mặt thịt. Đun sôi rồi hạ lửa nhỏ, hớt bọt thường xuyên để nước kho được trong. Kho liu riu khoảng 20 phút thì cho trứng vào. Tiếp tục kho cho đến khi thịt mềm rục và nước hơi sánh lại."),
            new("Hoàn thiện và trình bày", "Nêm nếm lại cho vừa miệng. Tắt bếp, thêm ớt tươi và tiêu xay. Múc thịt và trứng ra đĩa, dùng kèm cơm nóng, dưa giá hoặc dưa muối để cân bằng vị béo của thịt.")
        ],
        Nutrition = new RecipeNutritionViewModel
        {
            Calories = "420 kcal",
            Protein = "28g",
            Fat = "32g",
            Carbs = "5g"
        },
        ChefTip = "Để nước kho có màu trong vắt như gương, bạn tuyệt đối không được đậy nắp nồi trong suốt quá trình kho nhé!"
    };
}

public record RecipeStepViewModel(string Title, string Description);

public class RecipeNutritionViewModel
{
    public string Calories { get; set; } = string.Empty;
    public string Protein { get; set; } = string.Empty;
    public string Fat { get; set; } = string.Empty;
    public string Carbs { get; set; } = string.Empty;
}
