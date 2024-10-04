using Domain.Entities;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PostFlower.Queries.GetService
{
    public class GetServiceQuery : IRequest<List<Service>>
    {
    }

    public class GetServiceQueryHandle : IRequestHandler<GetServiceQuery, List<Service>>
    {
        private IServiceRepository _serviceRepository;

        private readonly ILogger<GetServiceQueryHandle> _logger;

        public GetServiceQueryHandle(IServiceRepository serviceRepository, ILogger<GetServiceQueryHandle> logger)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
        }

        public async Task<List<Service>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<Service> list = await _serviceRepository.GetAllAsync();
                if (list == null)
                {
                    throw new Exception("service is null");
                }
                return list.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
