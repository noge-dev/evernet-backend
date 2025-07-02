using Evernet.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evernet.WebApi.Data.Configurations;

public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
{
    public void Configure(EntityTypeBuilder<VerificationCode> builder)
    {
        builder.HasOne(uvc => uvc.User)
            .WithMany(u => u.VerificationCodes)
            .HasForeignKey(uvc => uvc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}