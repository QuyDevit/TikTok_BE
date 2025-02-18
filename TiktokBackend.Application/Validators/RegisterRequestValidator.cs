using System.Text.RegularExpressions;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.Payloads;

namespace TiktokBackend.Application.Validators
{
    public class RegisterRequestValidator
    {
        public static ServiceResponse<bool> ValidateOtp(RegisterRequest.RequestOtp register)
        {
            if (string.IsNullOrEmpty(register.Type))
            {
                return ServiceResponse<bool>.Fail("Loại đăng ký không được để trống!");
            }

            if (register.Type == "email")
            {
                if (string.IsNullOrEmpty(register.Email))
                    return ServiceResponse<bool>.Fail("Email không được để trống!");

                string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

                var valid = Regex.IsMatch(register.Email, regex, RegexOptions.IgnoreCase);
                if (!valid)
                    return ServiceResponse<bool>.Fail("Email không hợp lệ!");
                return ServiceResponse<bool>.Ok(true);
            }

            if (register.Type == "phone")
            {
                if (string.IsNullOrEmpty(register.PhoneNumber))
                    return ServiceResponse<bool>.Fail("Số điện thoại không được để trống!");

            }

            return ServiceResponse<bool>.Ok(true);
        }
        public static ServiceResponse<bool> Validate(RegisterRequest.RegisterUser register)
        {
            if (string.IsNullOrEmpty(register.Type))
            {
                return ServiceResponse<bool>.Fail("Loại đăng ký không được để trống!");
            }

            if (register.Type == "email")
            {
                if (string.IsNullOrEmpty(register.Email))
                    return ServiceResponse<bool>.Fail("Email không được để trống!");

                if (string.IsNullOrEmpty(register.Password))
                    return ServiceResponse<bool>.Fail("Mật khẩu không được để trống!");
            }

            if (register.Type == "phone")
            {
                if (string.IsNullOrEmpty(register.PhoneNumber))
                    return ServiceResponse<bool>.Fail("Số điện thoại không được để trống!");
            }
            if (register.DateOfBirth == default)
            {
                return ServiceResponse<bool>.Fail("Ngày sinh không hợp lệ!");
            }
            int age = DateTime.UtcNow.Year - register.DateOfBirth.Year;
            if (register.DateOfBirth.Date > DateTime.UtcNow.AddYears(-age)) age--; 
            if (age < 13)
            {
                return ServiceResponse<bool>.Fail("Ngày sinh không hợp lệ!");
            }

            if (string.IsNullOrEmpty(register.VerificationCode))
                return ServiceResponse<bool>.Fail("Mã xác thực không được để trống!");

            return ServiceResponse<bool>.Ok(true);
        }

    }
}
