using MeiOrm;

namespace Melphi.WebApi
{
    public class WemoveDbService : BaseSqlService
    {
        public WemoveDbService() :
            base("wemove",
                new MeiOrm.SqlConnectionConfig()
                {
                    Password = "meicreated@2024",
                    Database = "wemove"
                }
            )
        {

        }
    }

    public class user_info
    {
        public int Id { get; set; }
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
