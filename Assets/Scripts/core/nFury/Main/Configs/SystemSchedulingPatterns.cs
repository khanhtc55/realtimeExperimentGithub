using System;
namespace nFury.Main.Configs
{
	public static class SystemSchedulingPatterns
	{
		private const ushort ALTERNATIVE_1_0 = 21845;
		private const ushort ALTERNATIVE_1_1 = 43690;
		private const ushort ALTERNATIVE_3_0 = 4369;
		private const ushort ALL = 65535;
        public const ushort BATTLE = ALL;
        public const ushort TARGETING = ALL;
        public const ushort ATTACK = ALTERNATIVE_1_1;
        public const ushort MOVEMENT = ALTERNATIVE_1_1;
        public const ushort HEALER_TARGETING = ALTERNATIVE_3_0;
        public const ushort TRACKING = ALTERNATIVE_1_0;
        public const ushort AREA_TRIGGER = ALTERNATIVE_1_0;
        public const ushort DROID_RENDER = ALL;
        public const ushort GENERATOR_RENDER = ALL;
        public const ushort SUPPORT_RENDER = ALL;
        public const ushort TRANSPORT_RENDER = ALL;
        public static readonly ushort ENTITY_RENDER = (!HardwareProfile.IsLowEndDevice()) ? ALL : ALTERNATIVE_1_0;
        public static readonly ushort TRACKING_RENDER = (!HardwareProfile.IsLowEndDevice()) ? ALL : ALTERNATIVE_3_0;
        public static readonly ushort HEALTH_RENDER = (!HardwareProfile.IsLowEndDevice()) ? ALL : ALTERNATIVE_3_0;
	}
}
