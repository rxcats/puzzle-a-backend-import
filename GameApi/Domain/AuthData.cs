using GameExtensions.Extensions;
using MessagePack;

namespace GameApi.Domain
{
    [MessagePackObject(true)]
    public class LoginResponse
    {
        public long UserId { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }

    [MessagePackObject(true)]
    public class CreateUserRequest
    {
        public string UserPlatformId { get; set; }
        public string AccessToken { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }

    [MessagePackObject(true)]
    public class SignInUserRequest
    {
        public string UserPlatformId { get; set; }
        public string AccessToken { get; set; }
        public string Nickname { get; set; }
        public string PhotoUrl { get; set; }
        public string ProviderId { get; set; }
        public string ProviderUserId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderEmail { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }
}