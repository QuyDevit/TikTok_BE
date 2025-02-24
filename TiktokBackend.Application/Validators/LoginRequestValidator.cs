using TiktokBackend.Application.Commands.Auths;
using TiktokBackend.Application.Common;

namespace TiktokBackend.Application.Validators
{
    public class LoginRequestValidator
    {
        public static ServiceResponse<bool> Validate(LoginCommand login)
        {
            if (string.IsNullOrEmpty(login.Type))
            {
                return ServiceResponse<bool>.Fail("Loại đăng ký không được để trống!");
            }
            if (string.IsNullOrEmpty(login.UserName))
                return ServiceResponse<bool>.Fail(login.Type == "phone" ? "Vui lòng nhập số điện thoại!" : "Vui lòng nhập tài khoản");

            if (string.IsNullOrEmpty(login.Password))
                return ServiceResponse<bool>.Fail(login.Type == "phone" ? "Vui lòng nhập mã xác thực!" : "Vui lòng nhập mật khẩu!");

            return ServiceResponse<bool>.Ok(true);
        }
    }
}
