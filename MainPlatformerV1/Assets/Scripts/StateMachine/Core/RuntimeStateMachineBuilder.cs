﻿using System.Collections.Generic;
using TN.Extensions;
using UnityEngine;

namespace ThunderNut.StateMachine {
    public static class Builder {
        public static RuntimeStateMachineBuilder RuntimeStateMachine => new RuntimeStateMachineBuilder();
    }

    public class RuntimeStateMachineBuilder {
        private List<State> states = new List<State>();
        private List<Decision> decisions = new List<Decision>();
        private State currentState;
        private State remainState;


        public RuntimeStateMachineBuilder WithState(IEnumerable<State> statesToAdd) {
            states = new List<State>(statesToAdd);
            return this;
        }

        public RuntimeStateMachineBuilder WithState(State state, out State addedState) {
            states ??= new List<State>();
            states.Add(state);
            addedState = state;
            return this;
        }

        public RuntimeStateMachineBuilder WithTransition(Transition transition, Decision decision, State trueState,
            State falseState, State statesToAdd) {

            transition.SetDecision(decision);
            transition.SetStates(trueState, falseState);

            statesToAdd.AddTransition(transition);
            decisions.Add(decision);

            return this;
        }

        public RuntimeStateMachineBuilder WithTransition(Transition transition, Decision decision, State trueState,
            State falseState, IEnumerable<State> statesToAdd) {

            transition.SetDecision(decision);
            transition.SetStates(trueState, falseState);

            statesToAdd.ForEach(x => x.AddTransition(transition));
            decisions.Add(decision);

            return this;
        }

        private RuntimeStateMachineBuilder WithDecision(IEnumerable<Decision> decisionsToAdd) {
            decisions = new List<Decision>(decisionsToAdd);
            return this;
        }

        private RuntimeStateMachineBuilder WithDecision(Decision decision) {
            decisions ??= new List<Decision>();
            decisions.Add(decision);
            return this;
        }

        public RuntimeStateMachineBuilder SetCurrentState(State state) {
            currentState = state;
            return this;
        }

        public RuntimeStateMachineBuilder SetRemainState(State state) {
            remainState = state;
            return this;
        }

        public RuntimeStateMachine Build() {
            var newStateMachine = ScriptableObject.CreateInstance<RuntimeStateMachine>();

            if (states != null) {
                newStateMachine.states = new List<State>(states);
            }

            if (decisions != null) {
                newStateMachine.decisions = new List<Decision>(decisions);
            }

            newStateMachine.currentState = currentState;
            newStateMachine.remainState = remainState;

            newStateMachine.EnableTransitions();

            return newStateMachine;
        }
    }
}