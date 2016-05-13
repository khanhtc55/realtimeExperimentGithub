
using System;
using Artemis;
using UnityEngine;
using System.Collections;
using nFury.Utils.Scheduling;
using nFury.Utils;


namespace rot.main.logic
{
	public class GameEngine
	{
		[Inject]
		public IRoutineRunner runner {get;set;}

		public static uint SIM_TIME_PER_FRAME = 25u;
		public static float VIEW_PHYSICS_TIME_PER_FRAME = 0.033f;

		private SimTimeEngine simTimeEngine;
		private ViewTimeEngine viewTimeEngine;
		
		public GameEngine ()
		{
			this.simTimeEngine = new SimTimeEngine(SIM_TIME_PER_FRAME);
			this.viewTimeEngine = new ViewTimeEngine(VIEW_PHYSICS_TIME_PER_FRAME);
		}

		[PostConstruct]
		public void Init()
		{
			runner.StartCoroutine(Run());
		}

		public void Destroy()
		{
			simTimeEngine.UnregisterAll();
			viewTimeEngine.UnregisterAll();
			Resources.UnloadUnusedAssets();
			GC.GetTotalMemory(true);
			GC.Collect();
		}
		public IEnumerator Run ()
		{
			while(true)
			{
                //try
                //{
					simTimeEngine.OnUpdate();
					viewTimeEngine.OnUpdate();
                //}
                //catch (Exception e)
                //{
                //    Debug.LogError(e.StackTrace);
                //}
				yield return new WaitForEndOfFrame();
			}
			
		}

		public void Scale(float scale)
		{
			simTimeEngine.ScaleTime(scale);
		}
	}
}

