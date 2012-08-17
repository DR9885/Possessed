using UnityEngine;
using System;
namespace StateMachine
{
    public class StateMachine<T> where T : MonoBehaviour
    {
        private T _Owner;
        private IState<T> _PreviousState = null;
        private IState<T> _CurrentState = null;

        public IState<T> State
        {
            set
            {
                _PreviousState = _CurrentState;
                _CurrentState = value;

                _PreviousState.Exit(_Owner);
                _CurrentState.Enter(_Owner);
            }
        }

        public StateMachine(T argOwner)
        {
            _Owner = argOwner;
        }

        private void Update()
        {
            if (_CurrentState != null)
                _CurrentState.Update(_Owner);
        }
    }

    public class StateMachine<T, K>
        where T : MonoBehaviour
        where K : MonoBehaviour
    {
        private T _Owner;
        private K _Target;
        private IState<T, K> _PreviousState = null;
        private IState<T, K> _CurrentState = null;

        public IState<T, K> State
        {
            set
            {
                _PreviousState = _CurrentState;
                _CurrentState = value;

                if(_PreviousState != null)
                    _PreviousState.Exit(_Owner, _Target);
                if (_CurrentState != null)
                    _CurrentState.Enter(_Owner, _Target);
            }
        }

        public StateMachine(T argOwner, K argTarget)
        {
            _Owner = argOwner;
            _Target = argTarget;
        }

        public void Update()
        {
            if (_CurrentState != null)
                _CurrentState.Update(_Owner, _Target);
        }
    }
}