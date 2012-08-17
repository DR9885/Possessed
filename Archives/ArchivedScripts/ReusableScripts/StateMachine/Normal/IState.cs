using System.Collections;

namespace StateMachine
{
    public interface IState<T>
    {
        void Enter(T argObject);
        void Update(T argObject);
        void Exit(T argObject);
    }

    public interface IState<T, K>
    {
        void Enter(T argObject1, K argObject2);
        void Update(T argObject1, K argObject2);
        void Exit(T argObject1, K argObject2);
    }
}