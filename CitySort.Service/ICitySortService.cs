namespace CitySort.Service
{
    public interface ICitySortService
    {
        Card[] Sort(Card[] cards, bool useOwnArray = false);
    }
}