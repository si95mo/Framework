using Diagnostic;
using Esprima.Ast;
using JintEngine = Jint.Engine;

namespace Engines
{
    /// <summary>
    /// Define an <see cref="Engine"/> for JavaScript
    /// </summary>
    public class JavascriptEngine : Engine
    {
        private JintEngine engine;

        /// <summary>
        /// Create a new instance of <see cref="JavascriptEngine"/>
        /// </summary>
        public JavascriptEngine()
        {
            engine = new JintEngine();
            Logger.Info($"{nameof(JavascriptEngine)} initialized");
        }

        public override bool ExecuteAsBool(string script)
            => Execute<bool>(script);

        public override double ExecuteAsDouble(string script)
            => Execute<double>(script);

        public override string ExecuteAsString(string script)
            => Execute<string>(script);

        protected override T Execute<T>(string script)
        {
            // Add return statement before the script
            script = $"return {script}";

            Script preparedScript = JintEngine.PrepareScript(script);
            object resultAsObject = engine.Evaluate(preparedScript).ToObject();

            T result = (T)resultAsObject;
            return result;
        }
    }
}