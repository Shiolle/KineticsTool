using Kinetics.Core.Data.Avid;
using System;
using System.Text.RegularExpressions;

namespace Kinetics.Core.Presentation
{
    internal static class FormattingExtensions
    {
        private const string RingPattern = @"(\++|\-+)"; // Mathches _eith_ a group of pluses or a group of minuses.
        private const string DirectionPattern = @"[ABCDEF](\/[ABCDEF])?";

        public static string AvidDirectionToString(AvidDirection direction)
        {
            switch (direction)
            {
                case AvidDirection.AB:
                    return "A/B";
                case AvidDirection.BC:
                    return "B/C";
                case AvidDirection.CD:
                    return "C/D";
                case AvidDirection.DE:
                    return "D/E";
                case AvidDirection.EF:
                    return "E/F";
                case AvidDirection.FA:
                    return "F/A";
                case AvidDirection.Undefined:
                    return string.Empty;
                default:
                    return direction.ToString();
            }
        }

        public static AvidWindow ParseWindow(string input)
        {
            var matchDir = Regex.Match(input, DirectionPattern);
            var directionStr = matchDir.Value;

            var direction = ParseDirection(directionStr);

            var matchRing = Regex.Match(input, RingPattern);
            var ringStr = matchRing.Success ? matchRing.Value : string.Empty;

            bool abovePlane;
            var ring = ParseRing(ringStr, out abovePlane);

            return new AvidWindow(direction, ring, abovePlane);
        }

        public static string AvidRingToNumericFormat(AvidRing ring, bool isAbovePlain)
        {
            char notationSymbol = isAbovePlain ? '+' : '-';

            switch (ring)
            {
                case AvidRing.Blue:
                    return new string(notationSymbol, 1);
                case AvidRing.Green:
                    return new string(notationSymbol, 2);
                case AvidRing.Magenta:
                    return new string(notationSymbol, 3);
                default:
                    return string.Empty;
            }
        }

        public static string AvidLinkToString(AvidModelLink link)
        {
            return string.Format("{0} -> {1} ({2}{3}:{4})",
                                 link.EndpointA,
                                 link.EndpointB,
                                 link.IsDiagonal ? "Dg" : "St",
                                 link.IsAbovePlane ? "+" : "-",
                                 link.Weight);
        }

        public static string AvidPathToString(AvidPathInfo path)
        {
            string pathNodes = string.Join(" -> ", path.PathNodes);
            return string.Format("D:{0} ({1})", path.Distance, pathNodes);
        }

        private static AvidDirection ParseDirection(string directionStr)
        {
            var direction = directionStr.ToUpper();
            switch (direction)
            {
                case "A":
                    return AvidDirection.A;
                case "A/B":
                    return AvidDirection.AB;
                case "B":
                    return AvidDirection.B;
                case "B/C":
                    return AvidDirection.BC;
                case "C":
                    return AvidDirection.C;
                case "C/D":
                    return AvidDirection.CD;
                case "D":
                    return AvidDirection.D;
                case "D/E":
                    return AvidDirection.DE;
                case "E":
                    return AvidDirection.E;
                case "E/F":
                    return AvidDirection.EF;
                case "F":
                    return AvidDirection.F;
                case "F/A":
                    return AvidDirection.FA;
                default:
                    return AvidDirection.Undefined;
            }
        }

        private static AvidRing ParseRing(string ringStr, out bool isAbovePlane)
        {
            if (string.IsNullOrEmpty(ringStr))
            {
                isAbovePlane = true;
                return AvidRing.Ember;
            }

            isAbovePlane = ringStr[0].Equals('+');

            return (AvidRing)(ringStr.Length + 1);
        }
    }
}
