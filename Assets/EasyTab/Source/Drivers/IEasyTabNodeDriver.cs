namespace EasyTab
{
    public interface IEasyTabNodeDriver<T> : IEasyTabNodeDriver
    {
        BorderMode GetBorderMode(T target);
        EasyTabNode GetChild(T target, int childNumber);
        int GetChildrenCount(T target);
        bool IsSelectable(T target);
        EasyTabNode GetParent(T target);
    }

    public interface IEasyTabNodeDriver
    {
        BorderMode GetBorderMode(object target);
        EasyTabNode GetParent(object target);
        EasyTabNode GetChild(object target, int childNumber);
        int GetChildrenCount(object target);
        bool IsSelectable(object target);
    }
}