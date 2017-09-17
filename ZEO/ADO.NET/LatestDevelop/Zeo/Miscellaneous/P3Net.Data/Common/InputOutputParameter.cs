using System;

namespace P3Net.Data.Common
{
    /// <summary>Simple factory for creating input/output parameters.</summary>
    public class InputOutputParameter
    {
        #region Construction

        private InputOutputParameter (string name)
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
        public static InputOutputParameter Named (string name)
        {
            return new InputOutputParameter(name);
        }

        /// <summary>Sets the value of the parameter.</summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>The updated parameter.</returns>
        public InputOutputParameter<T> WithValue<T>(T value)
        {
            return new InputOutputParameter<T>(m_name).WithValue(value);
        }

        #region Private members

        private readonly string m_name;
        #endregion
    }
}
