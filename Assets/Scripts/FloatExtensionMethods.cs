namespace ExtensionMethods
{
    /// <summary>
    /// Extension Methods for float (This should definetly be part of Mathf in the future)
    /// <author> Joe Koelbel </author>
    /// </summary>
    public static class FloatExtensionMethods
    {

        #region Public Methods

        /// <summary>
        /// Remaps a float value from one range to another
        /// </summary>
        /// <param name="value"> the value to remap </param>
        /// <param name="fromLow"> lower border of the current range </param>
        /// <param name="fromUp"> upper border of the current range </param>
        /// <param name="toLow"> lower border of the desired range </param>
        /// <param name="toUp"> upper border of the desired range </param>
        /// <returns> the remapped value </returns>
        public static float Map(this float value, float fromLow, float fromUp, float toLow, float toUp)
        {
            return (value - fromLow) / (fromUp - fromLow) * (toUp - toLow) + toLow;
        }

        #endregion

    }
}