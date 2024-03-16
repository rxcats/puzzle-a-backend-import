using System;
using GameExtensions.Extensions;
using MessagePack;

namespace GameApi.Domain
{
    public class ServiceException : Exception
    {
        private readonly ResultCode _resultCode;

        public ServiceException(ResultCode resultCode, string message) : base(message)
        {
            _resultCode = resultCode;
        }

        public ResultCode GetResultCode()
        {
            return _resultCode;
        }
    }

    public enum ResultCode
    {
        Ok = 0,
        InternalServerError = 90001,
        UnknownUser = 91001,
        InvalidAccessToken = 91002,
        UnAuthorized = 91003,
    }

    [MessagePackObject(true)]
    public class ApiResponse<T>
    {
        public int Code { get; set; }

        public T Result { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }
}