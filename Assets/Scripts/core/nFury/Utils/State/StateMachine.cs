
using System;
using System.Collections.Generic;
using nFury.Utils.Core;
using nFury.Utils.Diagnostics;

namespace nFury.Utils.State
{
	public class StateMachine
	{
		private IState curState;
		private Type prevStateType;
		private Dictionary<Type, List<Type>> legalTransitions;
		private List<Type> wildcardStates;
		public IState CurrentState
		{
			get
			{
				return this.curState;
			}
		}
		public Type PreviousStateType
		{
			get
			{
				return this.prevStateType;
			}
		}
		public StateMachine()
		{
			this.curState = null;
			this.prevStateType = null;
			this.legalTransitions = new Dictionary<Type, List<Type>>();
			this.wildcardStates = new List<Type>();
		}
		public void SetLegalTransition(Type fromType, Type toType)
		{
			List<Type> list;
			if (this.legalTransitions.ContainsKey(fromType))
			{
				list = this.legalTransitions[fromType];
			}
			else
			{
				list = new List<Type>();
				this.legalTransitions.Add(fromType, list);
			}
			if (list.IndexOf(toType) < 0)
			{
				list.Add(toType);
			}
		}
		public void SetLegalTransition<TFrom, TTo>()
		{
			this.SetLegalTransition(typeof(TFrom), typeof(TTo));
		}
		public void SetLegalTransition(Type wildcardType)
		{
			this.wildcardStates.Add(wildcardType);
		}
		public void SetLegalTransition<TWildcard>()
		{
			this.SetLegalTransition(typeof(TWildcard));
		}
		public virtual bool SetState(IState state)
		{
			if (!this.IsLegalTransition(state))
			{
				Service.Get<Logger>().DebugFormat("StateMachine failed to transition from state {0} to {1}", new object[]
				{
					this.curState.GetType().ToString(),
					state.GetType().ToString()
				});
				return false;
			}
			if (this.curState != null)
			{
				this.curState.OnExit(state);
			}
			this.prevStateType = ((this.curState != null) ? this.curState.GetType() : null);
			this.curState = state;
			this.curState.OnEnter();
			return true;
		}
		public bool IsLegalTransition(IState state)
		{
			if (state == null)
			{
				return false;
			}
			if (this.curState == null)
			{
				return true;
			}
			Type type = this.curState.GetType();
			Type type2 = state.GetType();
			return this.wildcardStates.Contains(type) || this.wildcardStates.Contains(type2) || (this.legalTransitions.ContainsKey(type) && this.legalTransitions[type].IndexOf(type2) >= 0);
		}
	}
}
