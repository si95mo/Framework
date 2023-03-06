using Core.Parameters;
using System.Threading.Tasks;
using Tasks;

namespace Instructions.Common
{
    public class ExecuteTask : Instruction
    {
        private StringParameter taskCode;
        private EnumParameter<TaskStatus> taskResult;

        private IAwaitable task;

        public ExecuteTask(string code) : base(code)
        {
            taskCode = new StringParameter($"{Code}.TaskCode");
            taskResult = new EnumParameter<TaskStatus>($"{Code}.TaskResult");

            InputParameters.Add(taskCode);
            OutputParameters.Add(taskResult);
        }

        public override async Task ExecuteInstruction()
        {
            await task;
        }
    }
}
