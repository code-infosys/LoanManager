using System;

namespace LoanManager.Service
{
    public class NotificationViewModel
    {
        public int Id { get; set; }

        public string TableName { get; set; }

        public int? TableId { get; set; }

        public string Details { get; set; }

        public string ProcessToUrl { get; set; }

        public bool? IsRead { get; set; }

        public int? AddedBy { get; set; }

        public DateTime? DateAdded { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ToUserId { get; set; }

        public int? ToRoleId { get; set; }

        public string UniqueId { get; set; }


    }
}

