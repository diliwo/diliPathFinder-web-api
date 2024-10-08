﻿using DiliBeneficiary.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiliBeneficiary.Infrastructure.Persistence.Configurations
{
    public class BeneficiaryConfiguration : IEntityTypeConfiguration<Beneficiary>
    {
        public void Configure(EntityTypeBuilder<Beneficiary> builder)
        {
            builder
                .Property(r => r.Id).HasColumnName("BeneficiaryId");
            builder
                .HasKey(r => new { r.Id });
            builder
                .Property(b => b.FirstName)
                .IsRequired();
            builder
                .Property(b => b.LastName)
                .IsRequired();
            builder
                .Ignore(b => b.FullName);
            builder.Property(b => b.ReferenceNumber)
                .IsRequired();
            builder
                .OwnsOne(b => b.Address);
            builder
                .OwnsOne(b => b.NativeLanguage);
            builder
                .OwnsOne(b => b.ContactLanguage);
            builder
                .OwnsOne(b => b.Email);
            builder
                .OwnsOne(b => b.Phone);
            builder
                .OwnsOne(b => b.MobilePhone);
            builder
                .Property(b => b.SocialWorkerName);
            builder
                .Property(e => e.BirthDate).HasColumnType("date");
            builder
                .Property(e => e.StartDateInCpas).HasColumnType("date");
            builder
                .Property(e => e.EndDateInCpas).HasColumnType("date");
            //builder
            //    .Ignore(b => b.Candidates);
            builder
                .Ignore(b => b.Registrations);
        }
    }
}