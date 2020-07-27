using System;

namespace UrlTestwork.Models
{
    /// <summary>
    /// Ссылка пользователя.
    /// </summary>
    public class CuttLink
    {
        public int Id { get; set; }
        /// <summary>
        /// Длинная ссылка.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Сокращённая ссылка.
        /// </summary>
        public string ShortUrl { get; set; }
        /// <summary>
        /// Токен для перехода по сокращенной ссылке.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Время создания ссылки.
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// Количество переходов по сокращенной ссылке.
        /// </summary>
        public int Clicked { get; set; }
    }
}
