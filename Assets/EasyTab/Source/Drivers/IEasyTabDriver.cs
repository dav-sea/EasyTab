namespace EasyTab
{
    public interface IEasyTabDriver
    {
        BorderMode GetBorderMode(Target target);
        EasyTabNode GetParent(Target target);
        EasyTabNode GetChild(Target target, int childNumber);
        int GetChildrenCount(Target target);
        bool IsSelectable(Target target);
    }
}