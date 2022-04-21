using AI.Fuzzy.Library;
using Core;
using System;
using System.Collections.Generic;
using Variable = AI.Fuzzy.Library.FuzzyVariable;

namespace Mathematics.FuzzyLogic
{
    /// <summary>
    /// Implement a fuzzy system
    /// </summary>
    public class FuzzySystem : IProperty
    {
        private MamdaniFuzzySystem fuzzySystem;
        private string code;
        private Dictionary<Variable, double> inputs;
        private Dictionary<string, FuzzyVariable> outputVariables;

        /// <summary>
        /// The system <see cref="FuzzyVariable"/>
        /// </summary>
        public Dictionary<string, FuzzyVariable> InputVariables { get; private set; }

        public string Code => code;

        public object ValueAsObject { get => code; set => _ = value; }

        public Type Type => typeof(FuzzySystem);

        /// <summary>
        /// Create a new instance of <see cref="FuzzySystem"/>
        /// </summary>
        /// <param name="code">The code</param>
        public FuzzySystem(string code)
        {
            fuzzySystem = new MamdaniFuzzySystem();
            this.code = code;

            inputs = new Dictionary<Variable, double>();
            outputVariables = new Dictionary<string, FuzzyVariable>();
            InputVariables = new Dictionary<string, FuzzyVariable>();
        }

        /// <summary>
        /// Add a <see cref="FuzzyVariable"/> to the
        /// <see cref="FuzzySystem"/> inputs
        /// </summary>
        /// <param name="variable">The <see cref="FuzzyVariable"/></param>
        /// <param name="value">The value</param>
        public void AddInput(FuzzyVariable variable, double value)
        {
            fuzzySystem.Input.Add(variable.Variable);
            inputs.Add(variable.Variable, value);
        }

        /// <summary>
        /// Add a <see cref="FuzzyVariable"/> to the
        /// <see cref="FuzzySystem"/> outputs
        /// </summary>
        /// <param name="variable">The <see cref="FuzzyVariable"/></param>
        public void AddOutput(FuzzyVariable variable)
        {
            fuzzySystem.Output.Add(variable.Variable);
            outputVariables.Add(variable.Name, variable);
        }

        /// <summary>
        /// Add a rule to the <see cref="FuzzySystem"/>
        /// </summary>
        /// <remarks>
        /// The <paramref name="ruleAsString"/> should have the same form as the linguistic one. <br/>
        /// For example: <br/>
        /// if ('variableName' is 'linguisticTerm') or ('otherVariableName' is 'otherlinguisticTerm') then 'outputVariableName' is 'someOtherLinguisticTerm'
        /// </remarks>
        /// <param name="ruleAsString">The rule as string</param>
        public void AddRule(string ruleAsString)
        {
            MamdaniFuzzyRule rule = fuzzySystem.ParseRule(ruleAsString);
            fuzzySystem.Rules.Add(rule);
        }

        /// <summary>
        /// Calculate the <see cref="FuzzySystem"/>
        /// </summary>
        /// <returns>A <see cref="List{T}"/> with all the results for each output variable</returns>
        public List<string> Calculate()
        {
            List<string> result = new List<string>();

            Dictionary<Variable, double> outputs = fuzzySystem.Calculate(inputs);
            foreach (Variable variable in outputs.Keys)
                result.Add($"{variable.Name}: {outputs[variable].ToString(outputVariables[variable.Name].Format)}{outputVariables[variable.Name].MeasureUnit}");

            return result;
        }

        /// <summary>
        /// Clear all the inputs and outputs <see cref="FuzzyVariable"/>
        /// </summary>
        public void Clear()
        {
            fuzzySystem.Input.Clear();
            inputs.Clear();

            fuzzySystem.Output.Clear();
            outputVariables.Clear();
        }
    }
}