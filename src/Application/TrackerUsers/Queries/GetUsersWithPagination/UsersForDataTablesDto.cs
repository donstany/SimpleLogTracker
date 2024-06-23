using SimpleLogTracker.Domain.Entities;

namespace SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination
{
    public class UsersForDataTablesDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public decimal TotalHours { get; set; }
        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<User, UsersForDataTablesDto>();
                CreateMap<UserWithPagination, UsersForDataTablesDto>();
            }
        }
    }
}
