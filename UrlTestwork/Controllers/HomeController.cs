using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using UrlTestwork.Models;

namespace UrlTestwork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CuttLinkContext db;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            db = new CuttLinkContext();
        }

        public IActionResult Index()
        {
            var links = db.CuttLinks.ToList(); // Получить список ссылок.
            return View(links);
        }
        /// <summary>
        /// Переход по сокращенной ссылке и увеличение счётчика перехода.
        /// </summary>
        /// <param name="token"> Уникальный токен сокращенной ссылки. </param>
        /// <returns></returns>
        [HttpGet, Route("/{token}")]
        public IActionResult CutUriRedirect([FromRoute] string token)
        {
            var link = db.CuttLinks.ToList().FirstOrDefault(u => u.Token == token); // Ищем токен в каждой записи.
            if (link == null)
            {
                return Redirect("/");
            }
            link.Clicked++; // Увеличение счетчика переходов по ссылке.
            db.SaveChanges();
            return Redirect(link.Url); // Переход по оригинальной ссылке.
        }

        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Создание сокращённой ссылки.
        /// </summary>
        /// <param name="url"> Длинная ссылка. </param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Create")]
        public IActionResult CreateLink([FromForm]string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                ModelState.AddModelError("", "Поле не может быть пустым.");
                return View();
            }
            if (!url.Contains("http"))
            {
                url = "http://" + url;
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host}/"; // Создание базовой ссылки нашего хоста
            var cut = new Cutter();

            var urls = db.CuttLinks.ToList();
            if (urls.Count == 0)
            {
                cut.GenerateToken();
            }
            else
            {
                // проверяем на совпадение токенов
                // каждый раз создается новый токен
                while (urls.Exists(u => u.Token == cut.GenerateToken().Token)) ;
            }

            var cutUrl = new CuttLink
            {
                Url = url,
                Token = cut.Token,
                ShortUrl = baseUrl + cut.Token
            };

            db.CuttLinks.Add(cutUrl);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Странница для редактирования ссылки.
        /// </summary>
        /// <param name="id"> Id ссылки пользователя. </param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                CuttLink link = await db.CuttLinks.FindAsync(id);
                if (link != null)
                    return View(link);
            }
            return NotFound();
        }
        /// <summary>
        /// Редактирование ссылки.
        /// </summary>
        /// <param name="link"> Новая ссылка пользователя. </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(CuttLink link)
        {
            if (link == null)
            {
                return BadRequest();
            }
            var cuttLink = db.CuttLinks.FindAsync(link.Id).Result;
            cuttLink.Url = link.Url;
            cuttLink.DateTime = link.DateTime;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Удаление ссылки.
        /// </summary>
        /// <param name="id"> Id ссылки пользователя. </param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var link = db.CuttLinks.FirstOrDefault(l => l.Id == id);
                if (link != null)
                {
                    db.CuttLinks.Remove(link);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
