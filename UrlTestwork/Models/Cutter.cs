using System;
using System.Linq;

namespace UrlTestwork.Models
{
    /// <summary>
    /// Создание сокращенных ссылок с токеном.
    /// </summary>
    public class Cutter
    {
        public string Token { get; private set; }

        /// <summary>
        /// Создание токена для сокращенной ссылки.
        /// </summary>
        /// <returns> Вернет текущий экземпляр класса. </returns>
        public Cutter GenerateToken()
        {
            string urlsafe = string.Empty;
            // Выбираем a-z A-Z 0-9 и из них создаем рандомный токен
            Enumerable.Range(48, 75)
              .Where(i => i < 58 || i > 64 && i < 91 || i > 96)
              .OrderBy(o => new Random().Next())
              .ToList()
              .ForEach(i => urlsafe += Convert.ToChar(i)); // Вставляем каждый символ в urlsafe
            // Выбираем случайную длину от 2 до 6 символов из urlsafe
            Token = urlsafe.Substring(new Random().Next(0, urlsafe.Length-1), new Random().Next(2, 6));

            return this;
        }
    }
}
