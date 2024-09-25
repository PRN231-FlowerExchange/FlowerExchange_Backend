using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WalletTransaction : BaseEntity<Wallet, Guid>
    {
        public Guid WalletId { get; set; }

        public Guid TransactonId { get; set; }

        public virtual Wallet Wallet { get; set; }

        public virtual Transaction Transaction { get; set; }

        public TransDirection Type { get; set; }
    }
}
