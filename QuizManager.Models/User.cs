using Microsoft.AspNetCore.Identity;

namespace QuizManager.Models
{
    public class User : IdentityUser
    {
        public PermissionLevel PermissionLevel { get; set; }
    }

    public enum PermissionLevel
    {
        Restricted = 0,
        View = 1,
        Edit = 2
    }
}