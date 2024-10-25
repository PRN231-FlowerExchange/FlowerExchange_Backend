﻿using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.RepositoryAdapter
{
    public class ServiceOrderRepository : RepositoryBase<ServiceOrder, Guid>, IServiceOrderRepository
    {

        public ServiceOrderRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
