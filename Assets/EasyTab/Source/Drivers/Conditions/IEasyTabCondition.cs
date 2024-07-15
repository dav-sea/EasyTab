using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTab
{
    /// <summary>
    ///  Spec TODO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEasyTabCondition<in T>
    {
        bool IsMetFor(T obj);
    }
}