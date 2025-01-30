namespace ApiEasier.Bll.Interfaces
{
    public interface IConverter<TSource, TDestination>
    {
        TDestination Convert(TSource source);
    }
}
