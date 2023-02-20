using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RadencyLibraryDomain.Entities;

namespace RadencyLibraryInfrastructure.Persistence.Configurations
{
    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Book).WithMany(x => x.Reviews).HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
