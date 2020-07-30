using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanManager.Models;  
using Microsoft.EntityFrameworkCore;
namespace LoanManager.Data
{
    public class GeneralSettingMap
    {
        public GeneralSettingMap(EntityTypeBuilder<GeneralSetting> tb)
        {
            tb.HasKey(o => o.Id);
            tb.Property(o => o.SettingKey).HasMaxLength(50);
            tb.Property(o => o.SettingValue).HasMaxLength(2000);
            tb.Property(o => o.Description).HasMaxLength(100);
            tb.Property(o => o.SettingGroup).HasMaxLength(50);

        } 
    }
}
