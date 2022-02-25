using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            #region AuditableEntity
            builder.Property(ae => ae.CreatedBy)
                .HasMaxLength(64)
                .IsUnicode(true);//nvarchar

            builder.Property(ae => ae.LastModifiedBy)
                .HasMaxLength(64)
                .IsUnicode(true);//nvarchar

            builder.Property(ae => ae.RowVersion)
                .HasColumnType("TimeStamp")
                .IsRowVersion();
            #endregion

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasMaxLength(128)
                .IsUnicode(true);//nvarchar

            builder.Property(u => u.Surname)
                .HasMaxLength(2)
                .IsRequired(false);
        }
    }
}
