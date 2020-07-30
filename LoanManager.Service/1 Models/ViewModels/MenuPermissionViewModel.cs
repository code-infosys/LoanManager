using System;

namespace LoanManager.Service
{
    public class MenuPermissionViewModel
    {
        public int Id { get; set; }

        public string MenuId { get; set; }

        public string RoleId { get; set; }

        public string UserId { get; set; }

        public int? SortOrder { get; set; }

        public bool IsCreate { get; set; }

        public bool IsRead { get; set; }

        public bool IsUpdate { get; set; }

        public bool IsDelete { get; set; }

        public bool IsActive { get; set; }


    }
}

