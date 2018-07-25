using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalrChatSample.Models;

namespace SignalrChatSample.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; } = "Enter your username:";

        public void OnGet()
        {
        }

        [BindProperty]
        public User ChatUser { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage($"Chat", new { username = ChatUser.Username });
        }
    }
}
