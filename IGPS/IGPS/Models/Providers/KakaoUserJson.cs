using Newtonsoft.Json;

namespace IGPS.Models.Providers
{
    [JsonObject]
    public class KakaoUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("kakao_account.email")]
        public string Email { get; set; }

        [JsonProperty("kakao_account.email_verified")]
        public bool VerifiedEmail { get; set; }

        public Properties Properties { get; set; }
    }

    [JsonObject]
    public class Properties
    {
        [JsonProperty("nickname")]
        public string NickName { get; set; }

        [JsonProperty("thumbnail_image")]
        public string Thumbnail { get; set; }

        [JsonProperty("profile_image")]
        public string ProfileImage { get; set; }
    }
}
