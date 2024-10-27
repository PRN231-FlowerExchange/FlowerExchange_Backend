using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class UserConversationConfiguration : IEntityTypeConfiguration<UserConversation>
    {
        public void Configure(EntityTypeBuilder<UserConversation> builder)
        {
            builder.ToTable("UserConversation");

            builder.HasKey(uc => uc.Id);

            // User many to many Conversation
            //builder
            //     .HasKey(uc => new { uc.UserId, uc.ConversationId });

            builder
                 .HasOne(uc => uc.User)
                 .WithMany(u => u.UserConversations)
                 .HasForeignKey(uc => uc.UserId);

            builder
                 .HasOne(uc => uc.Conversation)
                 .WithMany(c => c.UserConversations)
                 .HasForeignKey(uc => uc.ConversationId);
        }
    }
}
