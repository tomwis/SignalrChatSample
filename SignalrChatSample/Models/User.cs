using System.ComponentModel.DataAnnotations;

namespace SignalrChatSample.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }
    }
}
