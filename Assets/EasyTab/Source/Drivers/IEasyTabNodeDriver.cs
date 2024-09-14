namespace EasyTab
{
    public interface IEasyTabNodeDriver
    {
        BorderMode GetBorderMode(TransformOrScene target);
        EasyTabNode GetParent(TransformOrScene target);
        EasyTabNode GetChild(TransformOrScene target, int childNumber);
        int GetChildrenCount(TransformOrScene target);
        bool IsSelectable(TransformOrScene target);
    }
}