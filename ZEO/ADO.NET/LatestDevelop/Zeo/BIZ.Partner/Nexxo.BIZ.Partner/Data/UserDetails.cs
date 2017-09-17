using MGI.Common.DataAccess.Data;
namespace MGI.Biz.Partner.Data
{
    public class UserDetails : NexxoModel
    {
        public  System.Guid Rowguid { get; set; }
        public  string UserName { get; set; }
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  string FullName { get; set; }
        public  bool IsEnabled { get; set; }
        public  System.Nullable<int> ManagerId { get; set; }
        public  long LocationId { get; set; }
        public  int UserRoleId { get; set; }
        public  int UserStatusId { get; set; }
        public  string PhoneNumber { get; set; }
        public  string Email { get; set; }
        public  string Notes { get; set; }
        public  string TempPassword { get; set; }
        public int ChannelPartnerId { get; set; }

        public string UserStatus { get; set; }
        public string LocationName { get; set; }

        public string ClientAgentIdentifier { get; set; }
    }

    public enum SaveMode : int
    {
        Add = 1,
        Update
    }
}
