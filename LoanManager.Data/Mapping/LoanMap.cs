using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanManager.Models;  
using Microsoft.EntityFrameworkCore;
namespace LoanManager.Data
{
    public class LoanMap
    {
        public LoanMap(EntityTypeBuilder<Loan> tb)
        {
            tb.HasKey(o => o.Id);
            tb.Property(o => o.Name).HasMaxLength(100);

        } 
    }
}
