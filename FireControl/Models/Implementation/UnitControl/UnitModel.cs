using System;
using System.Collections.Generic;
using FireControl.Models.Interfaces;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.UnitControl;
using FireControl.Properties;
using Kinetics.Core;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.Calculators;

namespace FireControl.Models.Implementation.UnitControl
{
    internal class UnitModel : IUnitModel
    {
        private readonly IHexVectorUtility _hexVectorUtility;
        private readonly IHexGridCalculator _hexGridCalculator;

        private readonly List<IShellstarModel> _attachedShellstars;

        public UnitModel()
        {
            Position = new HexGridCoordinate();
            Vectors = RawHexVector.Zero;
            _attachedShellstars = new List<IShellstarModel>();

            _hexVectorUtility = ServiceFactory.Library.HexVectorUtility;
            _hexGridCalculator = ServiceFactory.Library.HexGridCalculator;
        }

        private HexGridCoordinate _position;
        private RawHexVector _vector;

        public string Name { get; set; }

        public HexGridCoordinate Position
        { 
            get { return _position; }
            set
            {
                _position = value;
                OnMoved();
            }
        }

        public RawHexVector Vectors
        {
            get { return _vector; }
            set
            {
                _vector = value;
                OnVelocityChanged();
            }
        }

        public void Move(HexAxis direction)
        {
            _hexGridCalculator.Move(Position, direction, 1);
            OnMoved();
        }

        public int GetDirectionValue(HexAxis direction)
        {
            return _hexVectorUtility.GetMagnitudeAlongDirection(Vectors, direction);
        }

        public void AssginDirectionValue(HexAxis direction, uint newValue)
        {
            _hexVectorUtility.AddOrUpdateVectorDirection(Vectors, (int)newValue, direction);
            OnVelocityChanged();
        }

        public IReadOnlyCollection<IShellstarModel> AttachedShellstars
        {
            get { return _attachedShellstars.AsReadOnly(); }
        }

        public void AttachShellstar(IShellstarModel shellstar)
        {
            if (_attachedShellstars.Contains(shellstar))
            {
                throw new ArgumentException(Resources.UnitModel_AttachShellstar_AlreadyExists, "shellstar");
            }

            _attachedShellstars.Add(shellstar);
            OnShellstarListChanged(ListAction.Added, shellstar);
        }

        public void DetachShellstar(IShellstarModel shellstar)
        {
            if (!_attachedShellstars.Contains(shellstar))
            {
                throw new ArgumentException(Resources.UnitModel_DetachShellstar_DoesNotExsit, "shellstar");
            }

            _attachedShellstars.Remove(shellstar);
            OnShellstarListChanged(ListAction.Removed, shellstar);
        }

        public event Action Moved;
        public event Action VelocityChanged;
        public event ShellstarListChangedDelegate ShellstarListChanged;

        protected virtual void OnMoved()
        {
            Action handler = Moved;
            if (handler != null)
            {
                handler();
            }
        }

        protected virtual void OnVelocityChanged()
        {
            Action handler = VelocityChanged;
            if (handler != null)
            {
                handler();
            }
        }

        protected virtual void OnShellstarListChanged(ListAction action, IShellstarModel shellstar)
        {
            ShellstarListChangedDelegate handler = ShellstarListChanged;
            if (handler != null)
            {
                handler(action, shellstar);
            }
        }
    }
}
