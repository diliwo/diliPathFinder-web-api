﻿using DiliBeneficiary.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiliBeneficiary.Infrastructure.Persistence.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.Property(l => l.Id).HasColumnName("LanguageId");
            builder.Property(l => l.Name).IsRequired().HasMaxLength(60);
            builder.HasIndex(l => l.Name).IsUnique();
        }
    }
}
