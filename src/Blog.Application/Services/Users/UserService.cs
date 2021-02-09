using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Helpers;
using Blog.Application.Models;
using Blog.Application.Queries.UserProfile;
using Blog.Domain.Dtos.Auth.Response;
using Blog.Domain.Entities;
using Blog.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public DataSourceResult SearchUsers(SearchUsersQuery request)
        {
            if (request == null)
                throw new System.ArgumentNullException(nameof(request));

            var query = _context.UserProfiles.Include(x => x.User).AsQueryable();

            if (!string.IsNullOrEmpty(request.Username))
            {
                query = query.Where(x => x.User.Username.Contains(request.Username.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                query = query.Where(x => x.FirstName.ToLower().Contains(request.FirstName.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                query = query.Where(x => x.LastName.ToLower().Contains(request.LastName.ToLower()));
            }

            query = query.OrderByDescending(x => x.CreatedOn);

            var result = new PagedResult<UserProfile>(query, request.PageIndex, request.PageSize);

            var response = new DataSourceResult
            {
                Data = result.AsQueryable().ProjectTo<UserDto>(_mapper.ConfigurationProvider),
                HasPreviousPage = result.HasPreviousPage,
                HasNextPage = result.HasNextPage,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                RecordsFiltered = result.Count,
                RecordsTotal = result.TotalCount,
                TotalPages = result.TotalPages
            };

            return response;
        }
    }
}
