using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Entity.Configuration
{
    public class FormatConfiguration : IEntityTypeConfiguration<Format>
    {
        public void Configure(EntityTypeBuilder<Format> builder)
        {
            builder.HasOne(p => p.Book).WithMany(p => p.Type).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
