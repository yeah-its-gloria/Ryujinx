using Ryujinx.Common.Memory;
using Ryujinx.HLE.HOS.Services.Account.Acc;
using System.Runtime.InteropServices;

namespace Ryujinx.HLE.HOS.Applets.PlayerSelect
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct PlayerSelectSettings
    {
        public PlayerSelectMode Mode;
        public uint Padding;
        public Array8<UserId> InvalidUserIdList;
        public ulong AppId;
        public byte IsOnlineRequired;
        public byte IsSkippedEnabled;
        public byte DispatchedBySystem;
        public byte AllowUserCreation;
        public byte CanSkipSelection;
        public byte AdditionalSelect;
        public Array10<byte> Unused2;
    }
}
