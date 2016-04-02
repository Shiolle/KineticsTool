using Kinetics.Core.Data.HexVectors;
using System;
using System.Linq;
using System.Text;

namespace Kinetics.Core.Data.HexGrid
{
    /// <summary>
    /// Hex grid position is a hex vector that always have its primary and secondary components along CF and DA axis.
    /// </summary>
    public class HexGridCoordinate : RawHexVector
    {
        private const char PlaneCoordinateSeparator = ' ';
        private const char AltitudeSeparator = ':';

        private HexVectorComponent _daCoordinate;
        private HexVectorComponent _cfCoordinate;
        private HexVectorComponent _altitude;

        public int DaCoordinate
        {
            get { return DaVectorToGridCoordinate(); }

            set
            {
                int adjustedDaCoordinate = value - GetVerticalCoordinateAdjustment();
                _daCoordinate = ReplaceVectorComponent(adjustedDaCoordinate, _daCoordinate, HexAxis.D, HexAxis.A);
            }
        }

        public int CfCoordinate
        {
            get { return VectorComponentToMagnitude(_cfCoordinate, HexAxis.C); }

            set { _cfCoordinate = ReplaceVectorComponent(value, _cfCoordinate, HexAxis.C, HexAxis.F); }
        }

        public int Altitude
        {
            get { return VectorComponentToMagnitude(_altitude, HexAxis.Up); }

            set { _altitude = ReplaceVectorComponent(value, _altitude, HexAxis.Up, HexAxis.Down); }
        }

        public override string ToString()
        {
            var hexPosBuilder = new StringBuilder();

            hexPosBuilder.AppendFormat("{0}{1}{2}", CfCoordinate, PlaneCoordinateSeparator, DaCoordinate);

            if (Altitude != 0)
            {
                hexPosBuilder.AppendFormat("{0}{1}", AltitudeSeparator, Altitude);
            }

            return hexPosBuilder.ToString();
        }

        public static HexGridCoordinate Parse(string source)
        {
            // So far only one format is supported <CFcoord> <DAcoord>:<Altitude>
            string[] hexCoords = source.Split(AltitudeSeparator);

            // Altitude is always the last coordinate.
            int altitude = hexCoords.Length > 1 ? Convert.ToInt32(hexCoords[1]) : 0;

            string[] planeCoords = hexCoords[0].Split(PlaneCoordinateSeparator);
            //C-F is always the first coordinate, D-A is the second.
            int coordD = planeCoords.Length > 1 ? Convert.ToInt32(planeCoords[1]) : 0;
            int coordC = Convert.ToInt32(planeCoords[0]);

            return new HexGridCoordinate
            {
                CfCoordinate = coordC,
                DaCoordinate = coordD,
                Altitude = altitude
            };
        }

        public HexGridCoordinate Clone()
        {
            return new HexGridCoordinate
            {
                Altitude = Altitude,
                CfCoordinate = CfCoordinate,
                DaCoordinate = DaCoordinate
            };
        }

        protected override void OnComponentsChanged()
        {
            base.OnComponentsChanged();

            _cfCoordinate = Components.FirstOrDefault(hvc => hvc.Direction == HexAxis.C || hvc.Direction == HexAxis.F);
            _daCoordinate = Components.FirstOrDefault(hvc => hvc.Direction == HexAxis.D || hvc.Direction == HexAxis.A);
            _altitude = Components.FirstOrDefault(hvc => hvc.Direction == HexAxis.Up || hvc.Direction == HexAxis.Down);
        }

        private int VectorComponentToMagnitude(HexVectorComponent vectorComponent, HexAxis positiveDirection)
        {
            if (vectorComponent == null)
            {
                return 0;
            }

            int coordinateSign = vectorComponent.Direction == positiveDirection ? +1 : -1;

            return coordinateSign * vectorComponent.Magnitude;
        }

        private HexVectorComponent ReplaceVectorComponent(int coordinateValue, HexVectorComponent existingComponent, HexAxis positiveAxis, HexAxis negativeAxis)
        {
            if (existingComponent != null)
            {
                _components.Remove(existingComponent);
            }

            var newComponent = new HexVectorComponent(coordinateValue >= 0 ? positiveAxis : negativeAxis, Math.Abs(coordinateValue));

            _components.Add(newComponent);

            return newComponent;
        }

        private int DaVectorToGridCoordinate()
        {
            int result = VectorComponentToMagnitude(_daCoordinate, HexAxis.D);

            return result + GetVerticalCoordinateAdjustment();
        }

        private int GetVerticalCoordinateAdjustment()
        {
            // If you trace a line directly in C direction on the map you wil notice that second coordinate will increase every two hexes.
            return (int)Math.Ceiling(CfCoordinate / 2d);
        }
    }
}
