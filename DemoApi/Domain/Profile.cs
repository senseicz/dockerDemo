using System.Collections.Generic;

namespace DemoApi.Domain
{
    public class Profile
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
