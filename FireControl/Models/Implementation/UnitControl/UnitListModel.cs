using System;
using System.Collections.Generic;
using FireControl.Models.Interfaces;
using FireControl.Models.Interfaces.UnitControl;
using FireControl.Properties;
using Kinetics.Storage;
using Kinetics.Storage.Situation;

namespace FireControl.Models.Implementation.UnitControl
{
    internal class UnitListModel : IUnitListModel
    {
        private readonly IStorageController _storageController;

        public UnitListModel(IStorageController storageController)
        {
            _storageController = storageController;
            Models = new List<IUnitModel>();
        }

        public List<IUnitModel> Models { get; private set; }

        public event UnitListChanged UnitsChanged;

        public void AddUnit(IUnitModel unitModel)
        {
            if (unitModel == null || string.IsNullOrEmpty(unitModel.Name))
            {
                throw new ArgumentException(Resources.UnitListModel_AddUnit_MustHaveName, "unitModel");
            }

            Models.Add(unitModel);
            OnUnitsChanged(ListAction.Added, unitModel);
        }

        public void RemoveUnit(IUnitModel unitModel)
        {
            Models.Remove(unitModel);
            OnUnitsChanged(ListAction.Removed, unitModel);
        }

        public void Save(string filePath)
        {
            var sitRep = AutoMapper.Mapper.Map<IUnitListModel, SitRepSto>(this);

            _storageController.SaveUnits(sitRep, filePath);
        }

        public void Load(string filePath)
        {
            var sitRep = _storageController.ReadSitRep(filePath);

            Models.Clear();
            Models.AddRange(AutoMapper.Mapper.Map<IEnumerable<UnitSto>, IEnumerable<UnitModel>>(sitRep.Units));

            OnUnitsChanged(ListAction.Reset, null);
        }

        private void OnUnitsChanged(ListAction action, IUnitModel affectedUnit)
        {
            if (UnitsChanged != null)
            {
                UnitsChanged(action, affectedUnit);
            }
        }
    }
}
