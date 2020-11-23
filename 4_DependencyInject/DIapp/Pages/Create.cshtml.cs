using System.Threading.Tasks;
using DIapp.Model;
using DIapp.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DIapp.Pages
{
    public class CreateModel : PageModel
    {
        private IUserRepo _userRepo;

        public CreateModel(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User user { get; set; }
        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _userRepo.AddUser(user);
            return Page();
        }
    }
}
