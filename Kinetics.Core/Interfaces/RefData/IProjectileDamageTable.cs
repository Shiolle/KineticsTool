namespace Kinetics.Core.Interfaces.RefData
{
    public interface IProjectileDamageTable
    {
        void GetDamages(float roc, out int dmg50, out int dmg100, out int dmg200);
    }
}
