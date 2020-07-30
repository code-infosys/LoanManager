using System;

namespace LoanManager.Service
{
    public class AppSettingViewModel
    {
        public int Id { get; set; }

        public string AppName { get; set; }

        public string AppShortName { get; set; }

        public string AppVersion { get; set; }

        public bool IsToggleSidebar { get; set; }

        public bool IsBoxedLayout { get; set; }

        public bool IsFixedLayout { get; set; }

        public bool IsToggleRightSidebar { get; set; }

        public string Skin { get; set; }

        public string FooterText { get; set; }

        public string Logo { get; set; }

        public string LoginPageBackground { get; set; }

        public string TimeZone { get; set; }


    }
}

