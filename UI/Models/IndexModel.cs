using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Models
{
    public class IndexModel : PageModel
    {
        // Kartların içindeki stringleri saklamak için bir liste
        public List<string> Cards { get; set; } = new List<string>();

        public IActionResult OnGet()
        {
            return Page();
        }

        // Kart eklemek için Post metodu
        public IActionResult OnPostAddCard(string cardContent)
        {
            // Gelen kart içeriğini kart listesine ekleyin
            Cards.Add(cardContent);
            // Sayfayı tekrar yükle
            return RedirectToPage();
        }
    }
}
