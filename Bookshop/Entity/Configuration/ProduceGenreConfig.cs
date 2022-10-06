using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Bookshop.Entity.Configuration
{
    public class ProduceGenreConfig : IEntityTypeConfiguration<ProductGenre>
    {
        public void Configure(EntityTypeBuilder<ProductGenre> builder)
        {
            builder.HasKey(p => new { p.ProductId, p.GenreId });

            builder.HasOne(p => p.Product).WithMany(p => p.ProductGenres).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Genre).WithMany(p => p.ProductGenres).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
