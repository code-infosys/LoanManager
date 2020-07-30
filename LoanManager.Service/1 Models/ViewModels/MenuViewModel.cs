using System;

namespace LoanManager.Service
{
    public class MenuViewModel
    {
        public int Id { get; set; }

        public string MenuText { get; set; }

        public string MenuURL { get; set; }

        public string ParentId { get; set; }

        public int? SortOrder { get; set; }

        public string MenuIcon { get; set; }


    }
}

