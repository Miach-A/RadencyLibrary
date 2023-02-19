using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RadencyLibraryDomain.Entities;

namespace RadencyLibraryInfrastructure.Persistence.Configurations
{
    internal class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
