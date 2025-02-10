namespace ApiEasier.Bll.Interfaces.Converter
{
    public interface IConverter<TSource, TDestination>
    {
        TDestination Convert(TSource source);
    }
}
