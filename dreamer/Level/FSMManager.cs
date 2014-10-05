using System;
using System.Collections;
using System.Collections.Generic;


namespace dreamer.Level
{
    public enum Transition
    {
        NullTransition = 0, 
    }

    public enum StateID
    {
        NullStateID = 0, 
    }


    public abstract class FSMState
    {
        protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
        protected StateID stateID;
        public StateID ID { get { return stateID; } }

        public void AddTransition(Transition trans, StateID id)
        {
            if (trans == Transition.NullTransition)
            {
                Console.WriteLine("Transition is null");
                return;
            }

            if (id == StateID.NullStateID)
            {
                Console.WriteLine("StateId is Null");
                return;
            }

            if (map.ContainsKey(trans))
            {
                Console.WriteLine("state: " + stateID.ToString() + " has a transition " + trans.ToString());
                return;
            }

            map.Add(trans, id);
        }

        public void DeleteTransition(Transition trans)
        {
            
            if (trans == Transition.NullTransition)
            {
                Console.WriteLine("Cannot has null transition");
                return;
            }

            
            if (map.ContainsKey(trans))
            {
                map.Remove(trans);
                return;
            }
            Console.WriteLine("Transition: " + trans.ToString() + " gave to " + stateID.ToString());
        }


        public StateID GetOutputState(Transition trans)
        {
            
            if (map.ContainsKey(trans))
            {
                return map[trans];
            }
            return StateID.NullStateID;
        }


        public virtual void DoBeforeEntering() { }


        public virtual void DoBeforeLeaving() { }


        public abstract void Reason(Person player, Person npc);


        public abstract void Act(Person player, Person npc);

    }



    public class FSMManager
    {
        private List<FSMState> states;


        private StateID currentStateID;
        public StateID CurrentStateID { get { return currentStateID; } }
        private FSMState currentState;
        public FSMState CurrentState { get { return currentState; } }

        public FSMManager()
        {
            states = new List<FSMState>();
        }


        public void AddState(FSMState s)
        {
            if (s == null)
            {
                Console.WriteLine("cannot has null reference");
            }

            if (states.Count == 0)
            {
                states.Add(s);
                currentState = s;
                currentStateID = s.ID;
                return;
            }

            // Add the state to the List if it's not inside it
            foreach (FSMState state in states)
            {
                if (state.ID == s.ID)
                {
                    Console.WriteLine("cannot add state " + s.ID.ToString() +
                                   " state exist");
                    return;
                }
            }
            states.Add(s);
        }


        public void DeleteState(StateID id)
        {
            // Check for NullState before deleting
            if (id == StateID.NullStateID)
            {
                Console.WriteLine("state cannot has null stateID");
                return;
            }

            // Search the List and delete the state if it's inside it
            foreach (FSMState state in states)
            {
                if (state.ID == id)
                {
                    states.Remove(state);
                    return;
                }
            }
            Console.WriteLine("cannot delete state " + id.ToString() +
                           ". it is not in the list");
        }

        public void PerformTransition(Transition trans)
        {
            
            if (trans == Transition.NullTransition)
            {
                Console.WriteLine("transition cannot be null");
                return;
            }

           
            StateID id = currentState.GetOutputState(trans);
            if (id == StateID.NullStateID)
            {
                Console.WriteLine("state " + currentStateID.ToString());
                return;
            }

              
            currentStateID = id;
            foreach (FSMState state in states)
            {
                if (state.ID == currentStateID)
                {
                   
                    currentState.DoBeforeLeaving();

                    currentState = state;

                    
                    currentState.DoBeforeEntering();
                    break;
                }
            }

        }

    } 
}