using Newtonsoft.Json;

namespace FunctionChainingExample.Models
{
    public sealed class ActivityResult
    {
        public string ActivityName { get; }
        public bool CompletedSuccessfuly { get; }
        public object Result { get; }

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
    }


    //public sealed class ActivityResult<TOutput> where TOutput : class
    //{
    //    public bool CompletedSuccessfuly { get; }
    //    public TOutput Result { get; }

    //    [JsonConstructor]
    //    private ActivityResult(TOutput result, bool completedSuccessfuly)
    //    {
    //        this.Result = result;
    //        this.CompletedSuccessfuly = completedSuccessfuly;
    //    }

    //    public static ActivityResult<TOutput> Fulfilled<TInput>(TOutput fulfiledResult = null) where TInput : class
    //        => new ActivityResult<TOutput>(fulfiledResult, true);

    //    public static ActivityResult<TOutput> Rejected<TInput>(TOutput rejectedResult = null)  where TInput : class
    //        => new ActivityResult<TOutput>(rejectedResult, false);
    //}
}
