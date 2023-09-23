using Ryujinx.Common.Logging;
using Ryujinx.HLE.HOS.Services.Account.Acc;
using Ryujinx.HLE.HOS.Services.Am.AppletAE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ryujinx.HLE.HOS.Applets.PlayerSelect
{
    internal class PlayerSelectApplet : IApplet
    {
        private readonly Horizon _system;

        private AppletSession _normalSession;

        public event EventHandler AppletStateChanged;

        public PlayerSelectApplet(Horizon system)
        {
            _system = system;
        }

        public ResultCode Start(AppletSession normalSession, AppletSession interactiveSession)
        {
            _normalSession = normalSession;

            CommonArguments commonArguments = IApplet.ReadStruct<CommonArguments>(normalSession.Pop());
            Logger.Info?.PrintMsg(LogClass.ServiceAm, $"PlayerSelectApplet version: 0x{commonArguments.AppletVersion:x8}");

            PlayerSelectSettings settings = IApplet.ReadStruct<PlayerSelectSettings>(normalSession.Pop());
            if (settings.Mode != PlayerSelectMode.UserSelector)
            {
                throw new NotImplementedException($"PlayerSelectMode {settings.Mode} is not implemented.");
            }

            List<UserProfile> users = _system.AccountManager.GetAllUsers().ToList();
            // TODO: check which users are already being used, opening the same user twice will cause problems

            UserId selected;
            bool result = _system.Device.UiHandler.DisplayProfileSelector(users.ToArray(), out selected);
            
            PlayerSelectResult selectResult = new PlayerSelectResult
            {
                Code = result ? PlayerSelectResultCode.Success : PlayerSelectResultCode.Aborted,
                UserId = selected
            };
            
            _normalSession.Push(BuildResponse(selectResult));
            AppletStateChanged?.Invoke(this, null);
            _system.ReturnFocus();
            return ResultCode.Success;
        }

        public ResultCode GetResult()
        {
            return ResultCode.Success;
        }

        private byte[] BuildResponse(PlayerSelectResult result)
        {
            byte[] data = new byte[Unsafe.SizeOf<PlayerSelectResult>()];

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            Marshal.StructureToPtr(result, handle.AddrOfPinnedObject(), true);
            handle.Free();

            return data;
        }
    }
}
