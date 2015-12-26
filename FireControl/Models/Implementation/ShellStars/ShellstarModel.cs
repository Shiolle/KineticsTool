using System;
using System.Collections.Generic;
using FireControl.Models.Interfaces;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Properties;
using Kinetics.Core.Data;
using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Data.HexGrid;

namespace FireControl.Models.Implementation.ShellStars
{
    internal class ShellstarModel : IShellstarModel
    {
        private readonly ShellstarInfo _shellstar;
        private readonly List<ShellstarInfo> _counterfire;
        private readonly IEvasionInfoModel _evasionInfo;

        public ShellstarModel(ShellstarInfo shellstar, TurnData timeOfLaunch)
            :this(shellstar, timeOfLaunch, new EvasionInfoModel())
        { }

        public ShellstarModel(ShellstarInfo shellstar, TurnData timeOfLaunch, IEvasionInfoModel evasionInfo)
        {
            ValidateShellstar(shellstar);
            _shellstar = shellstar;
            _counterfire = new List<ShellstarInfo>();
            _evasionInfo = evasionInfo;

            TimeOfLaunch = timeOfLaunch;
        }

        public ShellstarInfo Shellstar
        {
            get { return _shellstar; }
        }

        public string Tag { get; set; }

        public TurnData TimeOfLaunch { get; private set; }

        public IEvasionInfoModel EvasionInfo
        {
            get { return _evasionInfo; }
        }

        public HexGridCoordinate GetMapPosition(HexGridCoordinate targetPosition, TurnData turnData)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<ShellstarInfo> Counterfire
        {
            get { return _counterfire.AsReadOnly(); }
        }

        public void AttachCounterfireShellstar(ShellstarInfo shellstar)
        {
            if (shellstar == null || _counterfire.Contains(shellstar))
            {
                throw new ArgumentException(Resources.ShellstarModel_AttachCounterfireShellstar_InvalidOrNull, "shellstar");
            }
            _counterfire.Add(shellstar);
            OnCounterfireUpdated(ListAction.Added, shellstar);
        }

        public void DetachCounterfireShellstar(ShellstarInfo shellstar)
        {
            if (shellstar == null || !_counterfire.Contains(shellstar))
            {
                throw new ArgumentException(Resources.ShellstarModel_DetachCounterfireShellstar_InvalidOrNull, "shellstar");
            }
        }

        public event CounterfireListChanged CounterfireUpdated;

        private static void ValidateShellstar(ShellstarInfo shellstar)
        {
            if (shellstar == null)
            {
                throw new ArgumentNullException("shellstar");
            }

            if (shellstar.ImpulseTrack == null || shellstar.ImpulseTrack.Count == 0)
            {
                throw new ArgumentException(Resources.ShellstarModel_ValidateShellstar_ImpusleTrackNullOrEmpty, "shellstar");
            }
        }

        protected virtual void OnCounterfireUpdated(ListAction action, ShellstarInfo affectedShellstar)
        {
            var handler = CounterfireUpdated;
            if (handler != null)
            {
                handler(action, affectedShellstar);
            }
        }
    }
}
