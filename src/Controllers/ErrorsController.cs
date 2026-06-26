using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[AllowAnonymous]
[Route("errors")]
public class ErrorsController : Controller
{
    [HttpGet("{statusCode:int}")]
    public IActionResult Index(int statusCode)
    {
        var info = GetErrorInfo(statusCode);
        Response.StatusCode = statusCode;

        return View(new ErrorPageViewModel
        {
            StatusCode = statusCode,
            Title = info.Title,
            Message = info.Message,
            Icon = info.Icon,
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }

    private static (string Title, string Message, string Icon) GetErrorInfo(int statusCode) => statusCode switch
    {
        401 => (
            "401 - Chưa đăng nhập",
            "Bạn cần đăng nhập để truy cập trang này.",
            "lock"
        ),
        403 => (
            "403 - Không có quyền truy cập",
            "Tài khoản của bạn không có quyền xem nội dung này.",
            "block"
        ),
        404 => (
            "404 - Không tìm thấy trang",
            "Trang bạn tìm kiếm không tồn tại hoặc đã bị xóa.",
            "search_off"
        ),
        500 => (
            "500 - Lỗi máy chủ",
            "Đã xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại sau.",
            "error"
        ),
        502 => (
            "502 - Lỗi cổng kết nối",
            "Máy chủ tạm thời không phản hồi. Vui lòng thử lại sau.",
            "cloud_off"
        ),
        503 => (
            "503 - Dịch vụ tạm ngưng",
            "Hệ thống đang bảo trì hoặc quá tải. Vui lòng quay lại sau.",
            "construction"
        ),
        _ => (
            $"{statusCode} - Đã xảy ra lỗi",
            "Yêu cầu không thể hoàn thành. Vui lòng thử lại hoặc quay về trang chủ.",
            "warning"
        )
    };
}
