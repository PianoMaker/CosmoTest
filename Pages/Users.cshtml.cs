using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CosmoTest.Data;
using CosmoTest.Models;

namespace CosmoTest.Models
{
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _context;

        public UsersModel(AppDbContext context) => _context = context;

        public List<User> Users = new();

        [BindProperty]
        public User NewUser { get; set; } = new User();

        [BindProperty]
        public IFormFile? UserImage { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(NewUser.Name) || string.IsNullOrWhiteSpace(NewUser.Email))
            {
                await OnGetAsync();
                return Page();
            }

            if (UserImage != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(UserImage.FileName)}";
                var dir = Path.Combine("wwwroot/images");
                var filePath = Path.Combine(dir, fileName);


                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await UserImage.CopyToAsync(stream);
                }

                NewUser.ImageUrl = $"/images/{fileName}";
            }

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
