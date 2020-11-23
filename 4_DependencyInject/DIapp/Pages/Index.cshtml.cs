using System.Collections.Generic;
using DIapp.Model;
using DIapp.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
//Dependency is another object that the our class needs to execute
//Dependency Injection is when we bring the dependent object to our class to perform functions
namespace DIapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IUserRepo _userRepo;

        public IndexModel(ILogger<IndexModel> logger, IUserRepo userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }
        public List<User> users { get; set; }

        [BindProperty]
        public User user { get; set; }
        public void OnGet()
        {
            users = _userRepo.GetUser();
        }

        public IActionResult OnPost()
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
