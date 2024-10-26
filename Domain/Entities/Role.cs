using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public class Role : IdentityRole<Guid>, IEntityWitkKey<Guid>
    {
        [Key]
        public override Guid Id { get; set; } = default!;

        [NotMapped]
        public RoleType RoleType { get; set; }

        public override string Name
        {
            get => base.Name;
            set => base.Name = RoleType.ToString() ?? throw new ArgumentNullException(nameof(RoleType), "RoleType must be set"); //get from the RoleType automatically
        }

        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }

    }
}
