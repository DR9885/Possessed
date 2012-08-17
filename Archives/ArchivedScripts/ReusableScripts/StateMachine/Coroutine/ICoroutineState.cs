using System.Collections;

namespace StateMachine
{
    public interface ICoroutineState<T>
    {
        IEnumerator Enter(T argObject);
        void Update(T argObject);
        IEnumerator Exit(T argObject);
    }

    public interface ICoroutineState<T, K>
    {
        IEnumerator Enter(T argObject1, K argObject2);
        void Update(T argObject1, K argObject2);
        IEnumerator Exit(T argObject1, K argObject2);
    }

}