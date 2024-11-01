using Application.PostFlower.DTOs;
using Application.UsersAccount.DTOs;
using Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsersAccount.Queries.GetUserAccountQuery
{
    public class GetUserAccountQuery : IRequest<List<UserAccountViewDTO>>
    {
        public PaginateRequest PaginateRequest { get; set; }
    }

    public class GetUserAccountQueryHandler : IRequestHandler<GetUserAccountQuery, List<UserAccountViewDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserAccountQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public Task<List<UserAccountViewDTO>> Handle(GetUserAccountQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
