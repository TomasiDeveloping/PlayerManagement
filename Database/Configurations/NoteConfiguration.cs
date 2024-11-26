using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(note => note.Id);
        builder.Property(note => note.Id).ValueGeneratedNever();

        builder.Property(note => note.PlayerNote).IsRequired().HasMaxLength(500);
        builder.Property(note => note.CreatedOn).IsRequired();
        builder.Property(note => note.CreatedBy).IsRequired().HasMaxLength(150);
        builder.Property(note => note.ModifiedOn).IsRequired(false);
        builder.Property(note => note.ModifiedBy).IsRequired(false).HasMaxLength(150);
    }
}