using Ryujinx.HLE.HOS.Services.Account.Acc;
using System.Runtime.InteropServices;

namespace Ryujinx.HLE.HOS.Applets.PlayerSelect
{
    enum PlayerSelectResultCode : ulong
    {
        Success = 0,
        Aborted = 2
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct PlayerSelectResult
    {
        public PlayerSelectResultCode Code;
        public UserId UserId;
    }
}
