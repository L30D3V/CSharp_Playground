using Newtonsoft.Json;

namespace IdentityServer_Mongo.Entities 
{
    public class ResultResponse<T>
    {
        public T Result { get; set; }
        public string QueryId { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public object Status { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ResultStatus
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class ResultStatusError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public ResultStatusErrorDetail[] Errors { get; set; }
    }


    public class ResultStatusErrorDetail
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}