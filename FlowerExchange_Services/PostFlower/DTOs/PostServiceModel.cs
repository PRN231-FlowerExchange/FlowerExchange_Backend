using Application.Order.DTOs;
using Application.PlatformService.DTOs;
using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PostFlower.DTOs
{
    public class PostServiceModel
    {
        public Guid Id { get; set; }

        public PostServiceStatus Status { get; set; }

        public Guid PostId { get; set; }

        public PostModel Post { get; set; }

        public Guid ServiceId { get; set; }

        public ServiceModel Service { get; set; }

        public Guid? ServiceOrderId { get; set; }

        public ServiceOrderModel? ServiceOrder { get; set; }


    }
}
