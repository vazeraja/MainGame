using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MainGame {
    public interface IStateMachine<in T> {
        void TransitionToState(T nextState);
    }
    public abstract class BaseState<T, U> : ScriptableObject where T : CustomPhysics { // U is BaseState_SO
        [SerializeField] public string stateName;
        [SerializeField] protected State<T>[] states;
        [SerializeField] protected Transition<T, U>[] transitions;

        protected event Action<T> enterStateEvent;
        protected event Action<T> exitStateEvent;
        protected event Action<T> updateStateEvent;

        protected BaseState(string stateName, State<T>[] states, Transition<T, U>[] transitions, Action<T> enterStateEvent, Action<T> exitStateEvent, Action<T> updateStateEvent) {
            this.stateName = stateName;
            this.states = states;
            this.transitions = transitions;
            this.enterStateEvent = enterStateEvent;
            this.exitStateEvent = exitStateEvent;
            this.updateStateEvent = updateStateEvent;
        }

        protected virtual void OnEnable() {
            foreach (State<T> state in states) {
                enterStateEvent += state.OnEnter;
                updateStateEvent += state.LogicUpdate;
                exitStateEvent += state.OnExit;
            }
            updateStateEvent += CheckTransitions;
            exitStateEvent += ResetAnimationFinished;
        }
        protected virtual void OnDisable() {
            foreach (State<T> state in states) {
                enterStateEvent -= state.OnEnter;
                updateStateEvent -= state.LogicUpdate;
                exitStateEvent -= state.OnExit;
            }
            updateStateEvent -= CheckTransitions;
            exitStateEvent -= ResetAnimationFinished;
        }
        protected abstract void CheckTransitions(T entity);
        protected abstract void ResetAnimationFinished(T entity);

        public void OnStateEnter(T entity) => enterStateEvent?.Invoke(entity);
        public void OnLogicUpdate(T entity) => updateStateEvent?.Invoke(entity);
        public void OnStateExit(T entity) => exitStateEvent?.Invoke(entity);
    }
}