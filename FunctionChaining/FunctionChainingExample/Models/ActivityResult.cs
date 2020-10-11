using Newtonsoft.Json;

namespace FunctionChainingExample.Models
{
    public sealed class ActivityResult
    {
        public string ActivityName { get; }
        public bool CompletedSuccessfuly { get; }
        public object Result { get; private set; }

        [JsonConstructor]
        private ActivityResult(string activityName, object result, bool completedSuccessfuly)
        {
            this.ActivityName = activityName;
            this.Result = result;
            this.CompletedSuccessfuly = completedSuccessfuly;
        }

        public static ActivityResult Fulfilled(string activityName, object fulfiledResult = null)
            => new ActivityResult(activityName, fulfiledResult, true);

        public static ActivityResult Rejected(string activityName, object rejectedResult = null) 
            => new ActivityResult(activityName, rejectedResult, false);

        public void InvalidateActionResult(string reason)
        {
            this.Result = null;
        }
    }
}
