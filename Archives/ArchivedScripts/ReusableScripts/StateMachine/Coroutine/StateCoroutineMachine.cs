using System.Collections;
using UnityEngine;
using System;

namespace StateMachine
{

    public class StateCoroutineMachine<T> where T : MonoBehaviour
    {
        private T _Owner;
        private ICoroutineState<T> _PreviousState = null;
        private ICoroutineState<T> _CurrentState = null;
        private bool _SafeState = false;

        public ICoroutineState<T> State
        {
            set
            {
                _PreviousState = _CurrentState;
                _CurrentState = value;
                _Owner.StartCoroutine(OnStateChange(_PreviousState, _CurrentState));
            }
        }

        public StateCoroutineMachine(T argOwner)
        {
            _Owner = argOwner;
        }

        private IEnumerator OnStateChange(ICoroutineState<T> argOldState, ICoroutineState<T> argNewState)
        {
            if (_Owner)
            {
                /// Stop Update
                _SafeState = false;

                /// Exit Old State
                if (argOldState != null)
                    yield return _Owner.StartCoroutine(argOldState.Exit(_Owner));

                /// Enter New State
                if (argNewState != null)
                    yield return _Owner.StartCoroutine(argNewState.Enter(_Owner));

                /// Start Update
                _Owner.StartCoroutine(Update());
            }
            else
                throw new NullReferenceException("StateCoroutineMachine has No Owner!");
        }

        private IEnumerator Update()
        {
            _SafeState = true;
            while (_SafeState && _CurrentState != null)
            {
                if (_CurrentState != null)
                    _CurrentState.Update(_Owner);
                yield return new WaitForFixedUpdate();
            }
        }
    }


    public class StateCoroutineMachine<T, K>
        where T : MonoBehaviour
        where K : MonoBehaviour
    {
        private T _Owner;
        private K _Target;
        private ICoroutineState<T, K> _PreviousState = null;
        private ICoroutineState<T, K> _CurrentState = null;
        private bool _SafeState = false;

        public ICoroutineState<T, K> State
        {
            set
            {
                _PreviousState = _CurrentState;
                _CurrentState = value;
                _Owner.StartCoroutine(OnStateChange(_PreviousState, _CurrentState));
            }
        }

        public StateCoroutineMachine(T argOwner, K argTarget)
        {
            _Owner = argOwner;
            _Target = argTarget;
        }

        private IEnumerator OnStateChange(ICoroutineState<T, K> argOldState, ICoroutineState<T, K> argNewState)
        {
            if (_Owner)
            {
                /// Stop Update
                _SafeState = false;

                /// Exit Old State
                if (argOldState != null)
                    yield return _Owner.StartCoroutine(argOldState.Exit(_Owner, _Target));

                /// Enter New State
                if (argNewState != null)
                    yield return _Owner.StartCoroutine(argNewState.Enter(_Owner, _Target));

                /// Start Update
                yield return _Owner.StartCoroutine(Update());
            }
            else
                throw new NullReferenceException("StateCoroutineMachine has No Owner!");
        }

        private IEnumerator Update()
        {
            _SafeState = true;
            while (_SafeState && _CurrentState != null)
            {
                yield return new WaitForFixedUpdate();
                if (_CurrentState != null)
                    _CurrentState.Update(_Owner, _Target);
            }
        }
    }
}