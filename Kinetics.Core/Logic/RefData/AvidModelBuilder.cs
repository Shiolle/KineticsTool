using Kinetics.Core.Data.Avid;
using Kinetics.Core.Interfaces.RefData;

namespace Kinetics.Core.Logic.RefData
{
    internal class AvidModelBuilder : IAvidModelBuilder
    {
        public AvidModel BuildModel()
        {
            var result = new AvidModel();

            // Add caps.
            var upperCap = new AvidModelWindow(AvidDirection.Undefined, AvidRing.Magenta, true);
            var lowerCap = new AvidModelWindow(AvidDirection.Undefined, AvidRing.Magenta, false);
            result.Windows.Add(upperCap);
            result.Windows.Add(lowerCap);

            var emberRing = BuildFullRing(result, AvidRing.Ember, true);
            var blueUpperRing = BuildFullRing(result, AvidRing.Blue, true);
            var blueLowerRing = BuildFullRing(result, AvidRing.Blue, false);

            ConnectFullRings(result, emberRing, blueUpperRing);
            ConnectFullRings(result, emberRing, blueLowerRing);

            BuildGreenRing(result, blueUpperRing, upperCap);
            BuildGreenRing(result, blueLowerRing, lowerCap);

            return result;
        }

        /// <summary>
        /// Creates a closed circle of windows connected by links.
        /// </summary>
        private static AvidModelWindow[] BuildFullRing(AvidModel model, AvidRing ring, bool abovePlane)
        {
            AvidModelWindow firstWindow = null;
            AvidModelWindow previousWindow = null;

            var result = new AvidModelWindow[12];

            for (int i = 1; i <= 12; i++)
            {
                var newWindow = new AvidModelWindow((AvidDirection)i, ring, abovePlane);
                result[i - 1] = newWindow;

                if (firstWindow == null)
                {
                    firstWindow = newWindow;
                }

                if (previousWindow != null)
                {
                    model.Links.Add(new AvidModelLink(newWindow, previousWindow, 2, false, false));
                }
                previousWindow = newWindow;
                model.Windows.Add(newWindow);
            }
            //Connect first and last windows closing the circle.
            model.Links.Add(new AvidModelLink(firstWindow, previousWindow, 2, false, false));

            return result;
        }

        /// <summary>
        /// Creates a green ring and attaches it to the right magenta window. Model must have magenta windows.
        /// </summary>
        private void BuildGreenRing(AvidModel model, AvidModelWindow[] adjacentFullRing, AvidModelWindow adjacentCap)
        {
            AvidModelWindow firstWindow = null;
            AvidModelWindow previousWindow = null;

            for (int i = 0; i < 6; i++)
            {
                var newWindow = new AvidModelWindow((AvidDirection)(i * 2 + 1), AvidRing.Green, adjacentCap.AbovePlane);
                if (firstWindow == null)
                {
                    firstWindow = newWindow;
                }
                model.Links.Add(new AvidModelLink(newWindow, adjacentCap, 2, false, false));
                model.Links.Add(new AvidModelLink(newWindow, adjacentFullRing[i * 2], 2, false, false));

                int previousIndex = i == 0 ? 11 : i * 2 - 1;
                int nextIndex = (i * 2 + 1) % 12;
                model.Links.Add(new AvidModelLink(newWindow, adjacentFullRing[previousIndex], 2, false, true));
                model.Links.Add(new AvidModelLink(newWindow, adjacentFullRing[nextIndex], 2, false, true));

                if (previousWindow != null)
                {
                    model.Links.Add(new AvidModelLink(newWindow, previousWindow, 2, false, false));
                    var cornerWindow = new AvidModelWindow((AvidDirection)(i * 2), AvidRing.Green, adjacentCap.AbovePlane);
                    model.Windows.Add(cornerWindow);
                    model.Links.Add(new AvidModelLink(cornerWindow, newWindow, 1, false, false));
                    model.Links.Add(new AvidModelLink(cornerWindow, previousWindow, 1, false, false));
                }
                previousWindow = newWindow;
                model.Windows.Add(newWindow);
            }
            //Connect first and last windows closing the circle.
            model.Links.Add(new AvidModelLink(firstWindow, previousWindow, 2, false, false));
            var closingCornerWindow = new AvidModelWindow(AvidDirection.FA, AvidRing.Green, adjacentCap.AbovePlane);
            model.Windows.Add(closingCornerWindow);
            model.Links.Add(new AvidModelLink(closingCornerWindow, firstWindow, 1, false, false));
            model.Links.Add(new AvidModelLink(closingCornerWindow, previousWindow, 1, false, false));
        }

        /// <summary>
        /// Builds straight and diagonal links between two full rings, from amber ring either up or down.
        /// </summary>
        private static void ConnectFullRings(AvidModel model, AvidModelWindow[] ringA, AvidModelWindow[] ringB)
        {
            //We take for granted that windows with the same index in both arrays have the same direction.
            for (int i = 0; i < 12; i++)
            {
                int previousIndex = i == 0 ? 11 : i - 1;
                int nextIndex = (i + 1) % 12;
                model.Links.Add(new AvidModelLink(ringA[i], ringB[i], 2, false, false));
                model.Links.Add(new AvidModelLink(ringA[i], ringB[previousIndex], 2, true, false));
                model.Links.Add(new AvidModelLink(ringA[i], ringB[nextIndex], 2, true, false));
            }
        }
    }
}
