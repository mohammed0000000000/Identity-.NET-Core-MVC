using IdentityDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityDemo.Data.Config
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder) {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(emp => emp.Name).HasColumnType("VARCHAR").HasMaxLength(50);
            //decimal
            builder.Property(emp => emp.Salary).HasColumnType("DECIMAL");
            builder.ToTable("Employees");
        }
    }
}
