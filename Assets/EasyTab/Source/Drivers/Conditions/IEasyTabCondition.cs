namespace EasyTab
{
    public interface IEasyTabCondition<in T>
    {
        bool IsMetFor(T obj);
    }
}