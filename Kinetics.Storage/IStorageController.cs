using Kinetics.Storage.HitLocationTable;
using Kinetics.Storage.Situation;

namespace Kinetics.Storage
{
    public interface IStorageController
    {
        void SaveUnits(SitRepSto sitRep, string path);

        SitRepSto ReadSitRep(string path);

        HitTablesCatalogSto LoadHitTableCatalog(string path);

        void SaveHitTableCatalog(HitTablesCatalogSto catalog, string path);
    }
}
