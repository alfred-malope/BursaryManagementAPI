﻿namespace BusinessLogic.Models.Response
{
    public class UserManagerResponse
    {
        public string Message {  get; set; }
        public bool isSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public DateTime? ExpireDate { get; set; }
    }
}
