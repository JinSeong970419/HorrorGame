namespace Horror.Factory
{
    public interface IFactory<T>
    {
        T Create();
    }
}
