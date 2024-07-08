namespace Football.WebApi
{
    public class ChangePasswordRequest
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
