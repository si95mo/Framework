using AI.Fuzzy.Library;

namespace Mathematics.FuzzyLogic
{
    /// <summary>
    /// Define the type of the <see cref="LinguisticTerm"/> membership function
    /// </summary>
    public enum MembershipFunctionType
    {
        /// <summary>
        /// A trapezoidal membership function type
        /// </summary>
        Trapezoidal = 0,

        /// <summary>
        /// A triangular membership function type
        /// </summary>
        Triangular = 1,

        /// <summary>
        /// A normal membership function type
        /// </summary>
        Normal = 2,

        /// <summary>
        /// A constant membeership function type
        /// </summary>
        Constant = 3
    }

    /// <summary>
    /// Implement a linguistic term for the <see cref="FuzzySystem"/>
    /// </summary>
    public class LinguisticTerm
    {
        private FuzzyTerm fuzzyTerm;
        private string name;
        private MembershipFunctionType functionType;

        /// <summary>
        /// The associated <see cref="AI.Fuzzy.Library.FuzzyTerm"/>
        /// </summary>
        internal FuzzyTerm FuzzyTerm => fuzzyTerm;

        /// <summary>
        /// The <see cref="LinguisticTerm"/> name
        /// </summary>
        public string Name => name;

        /// <summary>
        /// The <see cref="MembershipFunctionType"/>
        /// </summary>
        public MembershipFunctionType FunctionType
        {
            get => functionType;
            private set => functionType = value;
        }

        /// <summary>
        /// Initialize the <see cref="LinguisticTerm"/>
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="membershipFunction">The <see cref="IMembershipFunction"/></param>
        protected LinguisticTerm(string name, IMembershipFunction membershipFunction)
        {
            fuzzyTerm = new FuzzyTerm(name, membershipFunction);

            this.name = name;
            functionType = MembershipFunctionType.Trapezoidal;
        }

        /// <summary>
        /// Create a new trapezoidal membership function
        /// </summary>
        /// <param name="name">The <see cref="LinguisticTerm"/> name</param>
        /// <param name="x1">The first point</param>
        /// <param name="x2">The second point</param>
        /// <param name="x3">The third point</param>
        /// <param name="x4">The fourth point</param>
        /// <returns>The <see cref="LinguisticTerm"/> created</returns>
        public static LinguisticTerm CreateTrapezoidal(string name, double x1, double x2, double x3, double x4)
        {
            TrapezoidMembershipFunction membershipFunction = new TrapezoidMembershipFunction(x1, x2, x3, x4);

            LinguisticTerm linguisticTerm = new LinguisticTerm(name, membershipFunction);
            linguisticTerm.FunctionType = MembershipFunctionType.Trapezoidal;

            return linguisticTerm;
        }

        /// <summary>
        /// Create a new triangular membership function
        /// </summary>
        /// <param name="name">The <see cref="LinguisticTerm"/> name</param>
        /// <param name="x1">The first point</param>
        /// <param name="x2">The second point</param>
        /// <param name="x3">The third point</param>
        /// <returns>The <see cref="LinguisticTerm"/> created</returns>
        public static LinguisticTerm CreateTriangular(string name, double x1, double x2, double x3)
        {
            TriangularMembershipFunction membershipFunction = new TriangularMembershipFunction(x1, x2, x3);

            LinguisticTerm linguisticTerm = new LinguisticTerm(name, membershipFunction);
            linguisticTerm.FunctionType = MembershipFunctionType.Triangular;

            return linguisticTerm;
        }

        /// <summary>
        /// Create a new normal membership function
        /// </summary>
        /// <param name="name">The <see cref="LinguisticTerm"/> name</param>
        /// <param name="center">The center of the membership function</param>
        /// <param name="sigma">The sigma value</param>
        /// <returns>The <see cref="LinguisticTerm"/> created</returns>
        public static LinguisticTerm CreateNormal(string name, double center, double sigma)
        {
            NormalMembershipFunction membershipFunction = new NormalMembershipFunction(center, sigma);

            LinguisticTerm linguisticTerm = new LinguisticTerm(name, membershipFunction);
            linguisticTerm.FunctionType = MembershipFunctionType.Normal;

            return linguisticTerm;
        }

        /// <summary>
        /// Create a new constant membership function
        /// </summary>
        /// <param name="name">The <see cref="LinguisticTerm"/> name</param>
        /// <param name="value">The (constant) value</param>
        /// <returns>The <see cref="LinguisticTerm"/> created /returns>
        public static LinguisticTerm CreateConstant(string name, double value)
        {
            ConstantMembershipFunction membershipFunction = new ConstantMembershipFunction(value);

            LinguisticTerm linguisticTerm = new LinguisticTerm(name, membershipFunction);
            linguisticTerm.FunctionType = MembershipFunctionType.Constant;

            return linguisticTerm;
        }
    }
}