using System.Linq;
using FireControl.Models.Interfaces.LaunchBoard;
using Kinetics.Storage;
using Kinetics.Storage.Configuration.CoilgunTypes;
using Kinetics.Storage.Configuration.MissileTypes;

namespace FireControl.ViewModels.LaunchBoard
{
    internal class WeaponSelectionViewModel : ViewModelBase
    {
        private IWeaponSelectionModel _weaponSelectionModel;

        private CoilgunType _selectedCoilgunType;
        private MissileType _selectedMissileType;

        public WeaponSelectionViewModel(IStaticConfigurationController staticConfiguration)
        {
            CoilgunTypes = staticConfiguration.CoilgunTypes;
            MissileTypes = staticConfiguration.MissileTypes;
        }

        public void Initialize(IWeaponSelectionModel weaponSelectionModel)
        {
            _weaponSelectionModel = weaponSelectionModel;
            OnPropertyChanged(Properties.IsMissile);
            OnPropertyChanged(Properties.IsWeaponSelected);

            SelectedCoilgunType = CoilgunTypes.FirstOrDefault();
            SelectedMissileType = MissileTypes.FirstOrDefault();
        }

        public CoilgunType[] CoilgunTypes { get; private set; }
        public MissileType[] MissileTypes { get; private set; }

        public object SelectedCoilgunType
        {
            get { return _selectedCoilgunType; }
            set
            {
                _selectedCoilgunType = value as CoilgunType;
                UpdateMuzzleVelocityMultiplyer();
                OnPropertyChanged(Properties.SelectedCoilgunType);
                OnPropertyChanged(Properties.MuzzleVelocity);
            }
        }

        public object SelectedMissileType
        {
            get { return _selectedMissileType; }
            set
            {
                _selectedMissileType = value as MissileType;
                UpdateMuzzleVelocityMultiplyer();
                OnPropertyChanged(Properties.SelectedMissileType);
            }
        }

        public int MuzzleVelocity
        {
            get { return _selectedCoilgunType != null ? _selectedCoilgunType.MvMultiplyer * 8 : 0; }
        }

        public bool IsMissile
        {
            get { return _weaponSelectionModel != null && _weaponSelectionModel.IsMissile; }
            set
            {
                if (_weaponSelectionModel != null)
                {
                    _weaponSelectionModel.IsMissile = value;
                    UpdateMuzzleVelocityMultiplyer();
                    OnPropertyChanged(Properties.IsMissile);
                }
                
            }
        }

        public bool IsWeaponSelected
        {
            get { return _weaponSelectionModel != null && _weaponSelectionModel.IsWeaponSelected; }
        }

        private void UpdateMuzzleVelocityMultiplyer()
        {
            if (_weaponSelectionModel == null)
            {
                return;
            }

            _weaponSelectionModel.MuzzleVelocityMultiplyer = IsMissile ? (_selectedMissileType != null ? _selectedMissileType.BurnDuration : 0) :
                                                                         (_selectedCoilgunType != null ? _selectedCoilgunType.MvMultiplyer : 0);
            _weaponSelectionModel.Acceleration = IsMissile ? (_selectedMissileType != null ? _selectedMissileType.Acceleration : 8) : 8;
            OnPropertyChanged(Properties.IsWeaponSelected);
        }

        private static class Properties
        {
            public const string IsWeaponSelected = "IsWeaponSelected";
            public const string SelectedCoilgunType = "SelectedCoilgunType";
            public const string SelectedMissileType = "SelectedMissileType";
            public const string MuzzleVelocity = "MuzzleVelocity";
            public const string IsMissile = "IsMissile";
        }
    }
}
