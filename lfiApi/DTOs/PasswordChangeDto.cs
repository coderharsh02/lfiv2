﻿namespace lfiApi.DTOs
{
    public class PasswordChangeDto
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
