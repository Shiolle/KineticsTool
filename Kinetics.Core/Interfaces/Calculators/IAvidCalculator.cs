using Kinetics.Core.Data.Avid;

namespace Kinetics.Core.Interfaces.Calculators
{
    /// <summary>
    /// Defines operations on AVID windows.
    /// </summary>
    public interface IAvidCalculator
    {
        /// <summary>
        /// Converts avid direction into angle in radians, where direction B/C is 0 and the value increases counter clockwise.
        /// </summary>
        /// <param name="direction">AVID direction</param>
        /// <returns>Angle in radians.</returns>
        double DirectionToAngle(AvidDirection direction);

        /// <summary>
        /// Gets the list of windows on AVID forming a great circle in a plane perpendicular to the provided axis. Usefull for persistent spins and rolls.
        /// </summary>
        /// <param name="rotationAxis">AVID window where the rotation axis intersects the AVID sphere.
        /// Since there are two such points, one is picked so that rotation is clockwise.
        /// Thus no separate parameter needed to specify rotation direction.</param>
        /// <returns>Returns a string of avid windows that a marker would follow in the specified rotation if its rotation speed was 1 window per segment.</returns>
        AvidWindow[] GetRotationCircleFromNormal(AvidWindow rotationAxis);

        /// <summary>
        /// Finds an AVID window opposite to the one provided as argument. Used to shift the frame of reference from attacker to target (among other things).
        /// </summary>
        /// <param name="window">AVID window opposite of the one we would like to find.</param>
        /// <returns>AVID window opposite to the one provided as argument.</returns>
        AvidWindow GetOppositeWindow(AvidWindow window);

        /// <summary>
        /// Gets the location of all six AVID markers from front and top markers, which completely define object orientation on the AVID.
        /// Also checks that the markers provided are 90 degrees apart.
        /// </summary>
        /// <param name="nose">Nose marker window.</param>
        /// <param name="top">Top marker window.</param>
        /// <returns>A set of all six markers. All markers should be 90 degrees apart.</returns>
        AvidOrientation GetOrientation(AvidWindow nose, AvidWindow top);

        /// <summary>
        /// Gets the location of all six orientation markers on the AVID from front marker location, presuming that the object has no roll.
        /// Can be used to determine evasion directions.
        /// </summary>
        /// <param name="nose">Nose marker window.</param>
        /// <param name="referenceDirection">If nose is in magenta window we need some cue to determine the up direction.</param>
        /// <returns>A set of all six markers. All markers should be 90 degrees apart.</returns>
        AvidOrientation GetOrientationWithoutRoll(AvidWindow nose, AvidDirection referenceDirection);

        /// <summary>
        /// Calculates the list of possible launch windows by course offset, target position on AVID and Crossing Vector.
        /// </summary>
        /// <param name="courseOffset">Course offset in number of windows.</param>
        /// <param name="targetWindow">The window through which the target is seen.</param>
        /// <param name="crossingVector">The window of the crossing vector.</param>
        /// <returns>The list of valid launch windows.</returns>
        AvidWindow[] GetPossibleLaunchWindows(int courseOffset, AvidWindow targetWindow, AvidWindow crossingVector);

        /// <summary>
        /// Gets the distance between the two windows by traversing AVID.
        /// </summary>
        /// <param name="start">Starting window.</param>
        /// <param name="destination">Destination window.</param>
        /// <returns>The distance in windows.</returns>
        int CountWindows(AvidWindow start, AvidWindow destination);

        /// <summary>
        /// Simialr to counting windows, but implements specific rules for course offset.
        /// </summary>
        /// <param name="targetVector">Vector to the target.</param>
        /// <param name="crossingVector">Crossing vecotr.</param>
        /// <returns>Course offset in windows</returns>
        int GetCourseOffset(AvidWindow targetVector, AvidWindow crossingVector);
    }
}
