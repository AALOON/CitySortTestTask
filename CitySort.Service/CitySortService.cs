using System;
using System.Collections.Generic;
using System.Linq;

namespace CitySort.Service
{
    public class CitySortService : ICitySortService
    {
        private (Dictionary<string, Card>, Dictionary<string, Card>) PrepareCardsDictionaries(Card[] cards)
        {
            try
            {
                var cardFromDict = cards.ToDictionary(c => c.From, c => c); // O(n)
                var cardToDict = cards.ToDictionary(c => c.To, c => c);     // O(n)

                return (cardFromDict, cardToDict);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Somthing wrong with cards. Probably it is contains loops.");
            }
        }

        /// <summary>
        /// Функция позволяет сортировать крточки городов.
        /// 
        /// Сложность алгоритма сортировки O(n)*
        /// Требуется дополнительной памяти O(n)
        ///
        /// <remarks>
        /// * Предпологается что словарь обращается за O(1)
        /// </remarks>
        /// </summary>
        /// <param name="cards">массив карточек которые следует отсортировать</param>
        /// <param name="useOwnArray">
        /// Позволяет использовать тот же массив для сортировки.
        /// <remarks name="useOwnArray">
        /// Warning: При useOwnArray=true в случае ошибки массив может быть поврежден
        /// </remarks>
        /// </param>
        /// <returns>Возращает отсортированный массив, в случае useOwnArray=true вернется тот же массив</returns>
        public Card[] Sort(Card[] cards, bool useOwnArray = false)
        {
            if (cards == null)
                throw new ArgumentNullException();

            var length = cards.Length;

            var result = useOwnArray ? cards : new Card[length];
            if (length == 0)
                return result;

            var (cardFromDict, cardToDict) = PrepareCardsDictionaries(cards); // O(n)

            var currentCity = cards.FirstOrDefault(c => !cardFromDict.ContainsKey(c.To)); // O(n)

            var i = length - 1;

            result[i] = currentCity ?? throw new ArgumentException(nameof(cards) + " contains looped cards.");

            while (i != 0) // O(n)
            {
                if (!cardToDict.ContainsKey(currentCity.From))
                    throw new ArgumentException(nameof(cards) + " should have only one last card.");

                result[--i] = currentCity = cardToDict[currentCity.From];
            }

            return result;

        }
    }
}
