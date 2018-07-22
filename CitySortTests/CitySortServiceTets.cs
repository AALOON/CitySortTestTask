using System;
using System.Collections.Generic;
using CitySort.Service;
using FluentAssertions;
using Xunit;

namespace CitySortTests
{
    public class CitySortServiceTets
    {
        private readonly ICitySortService _sortService;
        public CitySortServiceTets()
        {
            _sortService = new CitySortService();
        }

        [Fact]
        public void Sort_ExampleCardsOwnArray_Ok()
        {
            // arrange
            var cards = new[] { new Card("Мельбурн", "Кельн"), new Card("Москва", "Париж"), new Card("Кельн", "Москва") };
            var expected = new[] { new Card("Мельбурн", "Кельн"), new Card("Кельн", "Москва"), new Card("Москва", "Париж") };

            // act
            _sortService.Sort(cards, true);

            // assert
            cards.Should().BeEquivalentTo(expected, o=> o.WithStrictOrdering());
        }

        [Fact]
        public void Sort_ExampleCards_Ok()
        {
            // arrange
            var cards = new[] { new Card("Мельбурн", "Кельн"), new Card("Москва", "Париж"), new Card("Кельн", "Москва") };
            var expected = new[] { new Card("Мельбурн", "Кельн"), new Card("Кельн", "Москва"), new Card("Москва", "Париж") };
            var expectedOwn = new[] { new Card("Мельбурн", "Кельн"), new Card("Москва", "Париж"), new Card("Кельн", "Москва") };

            // act
            var result = _sortService.Sort(cards);

            // assert
            cards.Should().BeEquivalentTo(expectedOwn, o => o.WithStrictOrdering());
            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Fact]
        public void Sort_EmptyArray_Ok()
        {
            // arrange
            var cards = new Card[] {  };
            var expected = new Card[] {  };

            // act
            _sortService.Sort(cards);

            // assert
            cards.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Fact]
        public void Sort_Null_NullReferenceException()
        {
            // arrange

            // act
            var result = _sortService.Invoking(s => s.Sort(null));

            // assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Sort_LoopedCards_ArgumentException()
        {
            // arrange
            var cards = new[] { new Card("Мельбурн", "Кельн"), new Card("Москва", "Мельбурн"), new Card("Кельн", "Москва") };

            // act
            var result = _sortService.Invoking(s => s.Sort(cards));

            // assert
            result.Should().Throw<ArgumentException>().WithMessage("cards contains looped cards.");
        }

        [Fact]
        public void Sort_LoopedSomeCards_ArgumentException()
        {
            // arrange
            var cards = new[] { new Card("Мельбурн", "Кельн"), new Card("Москва", "Париж"), new Card("Кельн", "Мельбурн") };

            // act
            var result = _sortService.Invoking(s => s.Sort(cards));

            // assert
            result.Should().Throw<ArgumentException>().WithMessage("cards should have only one last card.");
        }

        [Fact]
        public void Sort_SameCityTwice_ArgumentException()
        {
            // arrange
            var cards = new[] { new Card("Мельбурн", "Кельн"), new Card("Москва", "Париж"), new Card("Париж", "Берлин"), new Card("Берлин", "Москва"), new Card("Москва", "Казань") };

            // act
            var result = _sortService.Invoking(s => s.Sort(cards));

            // assert
            result.Should().Throw<ArgumentException>().WithMessage("Somthing wrong with cards. Probably it is contains loops.");
        }


        [Theory]
        [MemberData(nameof(GoodCards))]
        public void Sort_GoodCards_Ok(/* arrange */ Card[] cards, Card[] expected)
        {
            // act
            var result = _sortService.Sort(cards);

            // assert
            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        public static IEnumerable<object[]> GoodCards =>
            new List<object[]>
            {
                new object[]
                {
                    new []
                    {
                        new Card("Лондон", "Берн"), new Card("Москва", "Париж"), new Card("Берлин", "Лондон"),
                        new Card("Мельбурн", "Кельн"), new Card("Париж", "Берлин"), new Card("Кельн", "Москва")
                    },
                    new []
                    {
                        new Card("Мельбурн", "Кельн"), new Card("Кельн", "Москва"), new Card("Москва", "Париж"),
                        new Card("Париж", "Берлин"), new Card("Берлин", "Лондон"), new Card("Лондон", "Берн"),
                    }
                },
                new object[]
                {
                    new []
                    {
                        new Card("Лондон", "Берн")
                    },
                    new []
                    {
                        new Card("Лондон", "Берн")
                    }
                },
                new object[]
                {
                    new []
                    {
                        new Card("Берлин", "Сиетл"), new Card("Лондон", "Берлин")
                    },
                    new []
                    {
                        new Card("Лондон", "Берлин"), new Card("Берлин", "Сиетл")
                    }
                },
                new object[]
                {
                    new []
                    {
                        new Card("Лондон", "Берлин"), new Card("Берлин", "Сиетл"), new Card("Сиетл", "Сидней"), new Card("Сидней", "Веллингтон")
                    },
                    new []
                    {
                        new Card("Лондон", "Берлин"), new Card("Берлин", "Сиетл"), new Card("Сиетл", "Сидней"), new Card("Сидней", "Веллингтон")
                    }
                },
                new object[]
                {
                    new []
                    {
                        new Card("Берлин", "Сиетл"), new Card("", "Берлин")
                    },
                    new []
                    {
                        new Card("", "Берлин"), new Card("Берлин", "Сиетл")
                    }
                }
            };
    }
}
