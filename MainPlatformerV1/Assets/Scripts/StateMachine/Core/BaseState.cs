using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MainGame {
    public interface IStateMachine<in T> {
        public void OnStateEnter(T entity);
        public void OnStateLogicUpdate(T entity);
        public void OnStateExit(T entity);
    }

    public abstract class BaseState<T, U> : ScriptableObject, IStateMachine<T> {
        // U is BaseState_SO
        [SerializeField] public string stateName;
        [SerializeField] public State<T>[] states;
        [SerializeField] public Transition<T, U>[] transitions;

        protected event Action<T> EnterStateEvent;
        protected event Action<T> ExitStateEvent;
        protected event Action<T> UpdateStateEvent;
        protected event Action<T> PhysicsUpdateStateEvent;

        protected BaseState(string stateName, State<T>[] states, Transition<T, U>[] transitions, Action<T> enterStateEvent, Action<T> exitStateEvent, 
            Action<T> updateStateEvent, Action<T> physicsUpdateStateEvent){
            this.stateName = stateName;
            this.states = states;
            this.transitions = transitions;
            EnterStateEvent = enterStateEvent;
            ExitStateEvent = exitStateEvent;
            UpdateStateEvent = updateStateEvent;
            PhysicsUpdateStateEvent = physicsUpdateStateEvent;
        }

        protected virtual void OnEnable(){
            foreach (var state in states) {
                EnterStateEvent += state.OnEnter;
                UpdateStateEvent += state.LogicUpdate;
                ExitStateEvent += state.OnExit;
                PhysicsUpdateStateEvent += state.PhysicsUpdate;
            }

            UpdateStateEvent += CheckTransitions;
            ExitStateEvent += ResetAnimationFinished;
        }
        protected virtual void OnDisable(){
            foreach (var state in states) {
                EnterStateEvent -= state.OnEnter;
                UpdateStateEvent -= state.LogicUpdate;
                ExitStateEvent -= state.OnExit;
                PhysicsUpdateStateEvent += state.PhysicsUpdate;
            }

            UpdateStateEvent -= CheckTransitions;
            ExitStateEvent -= ResetAnimationFinished;
        }

        protected abstract void CheckTransitions(T entity);
        protected abstract void ResetAnimationFinished(T entity);

        public void OnStateEnter(T entity){
            EnterStateEvent?.Invoke(entity);
        }
        public void OnStateLogicUpdate(T entity){
            UpdateStateEvent?.Invoke(entity);
        }
        public void OnStatePhysicsUpdate(T entity){
            PhysicsUpdateStateEvent?.Invoke(entity);
        }
        public void OnStateExit(T entity){
            ExitStateEvent?.Invoke(entity);
        }
    }
}
