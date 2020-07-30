using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanManager.Models;  
using Microsoft.EntityFrameworkCore;
namespace LoanManager.Data
{
    public class LoanManMap
    {
        public LoanManMap(EntityTypeBuilder<LoanMan> tb)
        {
            tb.HasKey(o => o.Id);
            tb.Property(o => o.FullName).HasMaxLength(100);

        } 
    }
}
