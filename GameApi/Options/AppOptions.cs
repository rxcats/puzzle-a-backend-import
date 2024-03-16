using GameExtensions.Extensions;

namespace GameApi.Options
{
    public class AppOptions
    {
        public bool EnableValidateAccessToken { get; set; }
        public string FirebaseCredentialFile { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }
}