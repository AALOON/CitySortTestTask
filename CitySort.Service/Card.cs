using System;
using System.Diagnostics.Contracts;

namespace CitySort.Service
{
    public class Card
    {
        public Card(string from, string to)
        {
            From = from;
            To = to;
        }

        public string From { get; }
        public string To { get; }

        public override bool Equals(object obj)
        {
            Contract.Assert(this != null);

            var c = obj as Card;

            if (c == null)
                return false;

            return Equals(c);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((From != null ? From.GetHashCode() : 0) * 397) ^ (To != null ? To.GetHashCode() : 0);
            }
        }

        public bool Equals(Card c)
        {
            if (ReferenceEquals(this, c))
                return true;
            return string.Equals(From, c.From, StringComparison.Ordinal) && string.Equals(To, c.To, StringComparison.Ordinal);
        }
    }
}
