namespace EasyTab
{
    /// <summary>
    /// This interface is needed in order to get the correct EasyTabDriver with all the decorators
    /// </summary>
    public interface IEasyTabDriverProvider
    {
        IEasyTabDriver GetDriver();
    }
}