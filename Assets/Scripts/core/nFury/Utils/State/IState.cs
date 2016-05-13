using System;
namespace nFury.Utils.State
{
	public interface IState
	{
		void OnEnter();
		void OnExit(IState nextState);
	}
}
