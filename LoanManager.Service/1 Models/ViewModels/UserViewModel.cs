using System;

namespace LoanManager.Service
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public DateTime? DateAdded { get; set; }

        public DateTime? DateModified { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public bool IsActive { get; set; }

        public string ChangePasswordCode { get; set; }

        public bool IsConfirm { get; set; }

        public string OnTime { get; set; }

        public string ClockTest { get; set; }

        public string About { get; set; }


    }
}

