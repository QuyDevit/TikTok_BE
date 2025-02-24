namespace TiktokBackend.Application.Payloads
{
    public class RegisterRequest
    {
        public class RequestOtp
        {
            public string Type { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
        }
        public class RegisterUser
        {
            public string Type { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string VerificationCode { get; set; }
        }
    }
}
