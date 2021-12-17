using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Variable = AI.Fuzzy.Library.FuzzyVariable;

namespace Mathematics.FuzzyLogic
{
    /// <summary>
    /// Define a fuzzy variable
    /// </summary>
    public class FuzzyVariable
    {
        private Variable fuzzyVariable;

        private string name;
        private double minimum;
        private double maximum;
        private string measureUnit;
        private string format;

        /// <summary>
        /// The <see cref="FuzzyVariable"/> name
        /// </summary>
        public string Name => name;

        /// <summary>
        /// The <see cref="FuzzyVariable"/> minimum value
        /// </summary>
        public double Minimum => minimum;

        /// <summary>
        /// The <see cref="FuzzyVariable"/> maximum value
        /// </summary>
        public double Maximum => maximum;

        /// <summary>
        /// The associated <see cref="AI.Fuzzy.Library.FuzzyVariable"/>
        /// </summary>
        internal Variable Variable => fuzzyVariable;

        /// <summary>
        /// The measure unit
        /// </summary>
        internal string MeasureUnit => measureUnit;


        /// <summary>
        /// The format
        /// </summary>
        internal string Format => format;

        /// <summary>
        /// Create a new instance of <see cref="FuzzyVariable"/>
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="minimum">The minimum value</param>
        /// <param name="maximum">The maximum value</param>
        public FuzzyVariable(string name, double minimum, double maximum, string measureUnit = "", string format = "0.000")
        {
            fuzzyVariable = new Variable(name, minimum, maximum);

            this.name = name;
            this.minimum = minimum;
            this.maximum = maximum;
            this.measureUnit = measureUnit;
            this.format = format;
        }

        /// <summary>
        /// Add a new <see cref="LinguisticTerm"/> to the 
        /// <see cref="FuzzyVariable"/>
        /// </summary>
        /// <param name="linguisticTerm">The <see cref="LinguisticTerm"/> to add</param>
        public void AddLinguisticTerm(LinguisticTerm linguisticTerm)
            => fuzzyVariable.Terms.Add(linguisticTerm.FuzzyTerm);
    }
}
