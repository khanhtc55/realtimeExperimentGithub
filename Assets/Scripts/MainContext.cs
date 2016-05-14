using System.Collections.Generic;
using strange.extensions.context.impl;
using UnityEngine;
using strange.extensions.signal.impl;
using nFury.Utils;
using rot.main.logic;
using rot.main.datamanager;
using rot.utils;
using rot.command;


namespace rot.main
{
    public class MainContext : SignalContext
    {
#if UNITY_IOS
        public static List<BossSceneSpawnSystem> _unused;
        public static List<AttackManagerSystem> _unused0;
        public static List<CameraSystem> _unused1;
        public static List<CharacterStateMachineSystem> _unused2;
        public static List<DamageCalculationSystem> _unused21;
        public static List<EditorInputSystem> _unused3;
        public static List<ManualInputSystem> _unused5;
        public static List<MapCellTriggerSystem> _unused6;
        public static List<MapEditorSystem> _unused7;
        public static List<NormalSceneRuleSystem> _unused91;
        public static List<RotationSystem> _unused11;
        public static List<UniversalInputSystem> _unused12;
        public static List<BlockInputSystem> _unused13;
#endif

        public MainContext(MonoBehaviour contextView)
            : base(contextView)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            #region Inject

           //  for Services
            injectionBinder.Bind<RealtimeTestConnector>()
				.ToValue(GameObject.Find("Connector").GetComponent<RealtimeTestConnector>())
                .ToSingleton() 
                .CrossContext();

            injectionBinder.Bind<IRoutineRunner>().To<RoutineRunner>().ToSingleton().CrossContext();
            injectionBinder.Bind<GameEngine>().To<GameEngine>().ToSingleton().CrossContext();
            injectionBinder.Bind<FrameTimeSubscriber>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalManager>().ToSingleton().CrossContext();

            #endregion

            #region Command

            commandBinder.Bind<StartSignal>().InSequence()
                .To<InitRoutineRunnerCmd>()
                .To<InitGameEngineCmd>()
                .To<InitSignalManagerCmd>()
                .To<InitGameSystemCmd>();
            //    .To<NetworkConfigCommand>()
            //    .To<InitSettingCmd>()


            commandBinder.Bind<SendFirstSetupSignal>().To<FirstSetupCmd>();
            commandBinder.Bind<StartNewGameSignal>().To<StartNewGameCmd>();
            commandBinder.Bind<StartNewOfflineGameSignal>().To<StartNewOfflineGameCmd>();
            commandBinder.Bind<SendClientAckSignal>().To<SendClientAckCmd>();
            commandBinder.Bind<SendUpdateSnapshotSignal>().To<SendUpdateSnapshotCmd>();
            commandBinder.Bind<OnReceiveClientAckSignal>().To<OnReceiveClientAckCmd>();
            commandBinder.Bind<OnReceiveUpdateSnapshotSignal>().To<OnReceiveUpdateSnapshotCmd>();

            commandBinder.Bind<SendUserInputSignal>().To<SendUserInputCmd>();
            commandBinder.Bind<OnReceiveUserInputSignal>().To<OnReceiveUserInputCmd>();

            #endregion

           
        }

        protected override void postBindings()
        {
            base.postBindings();
        }

        public override void Launch()
        {
            base.Launch();
        }
    }
}