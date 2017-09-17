using System;

namespace P3Net.Data.Common
{
    /// <summary>Simple factory for creating an output parameter.</summary>
    public class OutputParameter
    {
        #region Construction

        private OutputParameter(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty.", "name");

            m_name = name;
        }
        #endregion

        /// <summary>Creates a new input parameter with the provided name.</summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The new parameter.</returns>
        public static OutputParameter Named(string name)
        {
            return new OutputParameter(name);
        }        

        /// <summary>Gets the typed parameter.</summary>
        public OutputParameter<T> OfType<T>()
        {
            return new OutputParameter<T>(m_name);
        }

        #region Private Members

        private readonly string m_name;
        #endregion
    }
}
