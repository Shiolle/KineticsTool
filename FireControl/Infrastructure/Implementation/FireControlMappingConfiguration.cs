using System;
using System.Collections.Generic;
using AutoMapper;
using FireControl.Models.Implementation.ShellStars;
using FireControl.Models.Implementation.UnitControl;
using FireControl.Models.Interfaces.UnitControl;
using Kinetics.Core.Data;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Storage.Position;
using Kinetics.Storage.Shellstars;
using Kinetics.Storage.Situation;

namespace FireControl.Infrastructure.Implementation
{
    internal class FireControlMappingConfiguration
    {
        public static void CreateMappings()
        {
            CreateSaveSitRepMappings();
            CreateLoadSitRepMappings();
            CeateShipClassLibraryMappings();
        }

        private static void CreateSaveSitRepMappings()
        {
            Mapper.CreateMap<IUnitModel, UnitSto>()
                  .ForMember(uns => uns.Position, ac => ac.MapFrom(um => um.Position.ToString()))
                  .ForMember(uns => uns.Velocity, ac => ac.MapFrom(um => um.Vectors))
                  .ForMember(uns => uns.IncomingProjectiles, ac => ac.MapFrom(um => um.AttachedShellstars));

            Mapper.CreateMap<HexVectorComponent, HexVectorComponentSto>()
                  .ForMember(vcs => vcs.Direction, ac => ac.MapFrom(hvc => hvc.Direction.ToString()));

            Mapper.CreateMap<RawHexVector, HexVectorSto>();

            Mapper.CreateMap<ImpulseTrackElement, ImpulseRecordSto>();

            Mapper.CreateMap<EvasionInfoModel, EvasionInfoSto>()
                  .ForMember(eis => eis.EvasionUp, ac => ac.MapFrom(shm => shm.EvasionUp.ToString()))
                  .ForMember(eis => eis.ImpactWindow, ac => ac.MapFrom(shm => shm.ImpactWindow.ToString()))
                  .ForMember(eis => eis.FuelSpentUd, ac => ac.UseValue(0))
                  .ForMember(eis => eis.FuelSpentLr, ac => ac.UseValue(0))
                  .ForMember(eis => eis.FuelSpentTa, ac => ac.UseValue(0));

            Mapper.CreateMap<ShellstarModel, ShellstarSto>()
                  .ForMember(shs => shs.Roc, ac => ac.MapFrom(shm => shm.Shellstar.RoC))
                  .ForMember(shs => shs.Dmg50, ac => ac.MapFrom(shm => shm.Shellstar.Dmg50))
                  .ForMember(shs => shs.Dmg100, ac => ac.MapFrom(shm => shm.Shellstar.Dmg100))
                  .ForMember(shs => shs.Dmg200, ac => ac.MapFrom(shm => shm.Shellstar.Dmg200))
                  .ForMember(shs => shs.SegmentOfLaunch, ac => ac.MapFrom(shm => shm.TimeOfLaunch))
                  .ForMember(shs => shs.ImpulseTrack, ac => ac.MapFrom(shm => shm.Shellstar.ImpulseTrack));

            Mapper.CreateMap<IUnitListModel, SitRepSto>()
                  .ForMember(srs => srs.Units, ac => ac.MapFrom(ulm => ulm.Models));
        }

        private static void CreateLoadSitRepMappings()
        {
            Mapper.CreateMap<HexVectorComponentSto, HexVectorComponent>()
                  .ForMember(hvc => hvc.Direction, ac => ac.MapFrom(vcs => Enum.Parse(typeof(HexAxis), vcs.Direction)))
                  .ForMember(hvc => hvc.Magnitude, ac => ac.MapFrom(vcs => vcs.Value));

            Mapper.CreateMap<HexVectorSto, RawHexVector>()
                  .ForMember(hvc => hvc.Components, ac => ac.Ignore())
                  .AfterMap((hvs, rhv) => rhv.AddComponents(Mapper.Map<IEnumerable<HexVectorComponentSto>, IEnumerable<HexVectorComponent>>(hvs.Components)));

            Mapper.CreateMap<ImpulseRecordSto, ImpulseTrackElement>()
                  .ForMember(ite => ite.Impulse, ac => ac.MapFrom(irs => TurnData.Parse(irs.Impulse)));

            Mapper.CreateMap<ShellstarSto, ShellstarInfo>();

            Mapper.CreateMap<UnitSto, UnitModel>()
                  .ForMember(um => um.Position, ac => ac.MapFrom(uns => HexGridCoordinate.Parse(uns.Position)))
                  .ForMember(um => um.Vectors, ac => ac.MapFrom(uns => uns.Velocity))
                  .AfterMap((uns, um) => uns.IncomingProjectiles.ForEach(st =>
                  {
                      var evasionInfo = new EvasionInfoModel(AvidWindow.Parse(st.EvasionInfo.ImpactWindow), AvidWindow.Parse(st.EvasionInfo.EvasionUp).Direction);
                      var shellstarModel = new ShellstarModel(Mapper.Map<ShellstarSto, ShellstarInfo>(st), TurnData.Parse(st.SegmentOfLaunch), evasionInfo);
                      shellstarModel.Tag = st.Tag;
                      um.AttachShellstar(shellstarModel);
                  }));
        }

        private static void CeateShipClassLibraryMappings()
        {
            
        }
    }
}
