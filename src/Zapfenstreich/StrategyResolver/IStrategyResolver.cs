namespace DiTryouts
{
    public interface IStrategyResolver<out T>
    {
        T Resolve();
    }
}