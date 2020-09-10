using API.Models;

namespace Helper
{
    public class StatusManagement
    {
        public Status ReturnStatus(int statusCode, string message, bool isSuccess)
        {
            Status status = new Status
            {
                StatusCode = statusCode,
                IsSuccess = isSuccess,
                Message = message
            };

            return status;
        }
    }
}