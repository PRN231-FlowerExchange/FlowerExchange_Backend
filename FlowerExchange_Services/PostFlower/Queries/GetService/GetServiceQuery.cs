using Application.PostFlower.DTOs;
using Application.PostFlower.Services;
using Domain.Entities;
using Domain.Exceptions;
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
    public class GetServiceQuery : IRequest<List<ServiceViewDTO>>
    {
    }

    public class GetServiceQueryHandle : IRequestHandler<GetServiceQuery, List<ServiceViewDTO>>
    {
        private IServiceRepository _serviceRepository;

        private readonly ILogger<GetServiceQueryHandle> _logger;

        public GetServiceQueryHandle(IServiceRepository serviceRepository, ILogger<GetServiceQueryHandle> logger)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
        }

        public async Task<List<ServiceViewDTO>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var services = await _serviceRepository.GetAllAsync();

                if (services == null || !services.Any())
                {
                    throw new NotFoundException("No service found");
                }

                return ConvertFuction.ConvertListToList<ServiceViewDTO, Service>(services.ToList());
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                // Add additional context to the exception if needed, otherwise use "throw;" to preserve stack trace.
                throw;
            }
        }
    }
}
