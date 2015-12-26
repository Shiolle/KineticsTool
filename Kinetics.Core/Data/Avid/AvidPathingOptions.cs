namespace Kinetics.Core.Data.Avid
{
    /// <summary>
    /// Specifies restrictions upon using diagonal transitions when performing AVID pathing, like finding shortest path bedtween two windows.
    /// </summary>
    internal enum AvidPathingOptions : byte
    {
        /// <summary>
        /// No restrictions upon the use of diagonal transitions. Path can go diagonally as much as needed.
        /// </summary>
        DiagonalTransitionsUnlimited = 0,

        /// <summary>
        /// Diagonal transitions are limited to 1 transition per 6 windows per hemisphere.
        /// </summary>
        DiagonalTransitionsLimitPerHemisphere = 1,

        /// <summary>
        /// Same as diagonal transitions limit per hemisphere, but also includes limits by and to polar diagonal transitions.
        /// </summary>
        DiagonalTransitionsLimitWithPolar = 2,

        /// <summary>
        /// Diagonal transitions are limited to 1 transition per 6 windows regardless of hemisphere.
        /// </summary>
        DiagonalTransitionsTotalLimit = 3,

        /// <summary>
        /// Diagonal links between windows are ignored.
        /// </summary>
        DiagonalTransitionsIgnored = 4
    }
}
