using System.Collections.Generic;

namespace Messages
{
    public class UserProfile
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }

    public interface ISendUserProfile
    {
        UserProfile UserProfile { get; set; }
    }
}
