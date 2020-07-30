using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace IGPS.Models.Providers
{
    public class KakaoOAuth2 : OAuth2Base
    {
        private static readonly Lazy<KakaoOAuth2> lazy = new Lazy<KakaoOAuth2>(() => new KakaoOAuth2());

        public static KakaoOAuth2 Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private KakaoOAuth2()
        {
            Initialize();
        }

        void Initialize()
        {
            ProviderName = "Kakao";
            Description = "Kakao Login Provider";
            Provider = SNSProvider.Kakao;
            ClientId = "c5e894d048e321f5a541ac08b2b40f46";
            ClientSecret = null;
            Scope = null;
            AuthorizationUri = new Uri("https://kauth.kakao.com/oauth/authorize");
            RequestTokenUri = new Uri("https://kauth.kakao.com/oauth/token");
            RedirectUri = new Uri("https://github.com/URK96");
            UserInfoUri = new Uri("https://kapi.kakao.com/v2/user/me");
        }

        public override async Task<UserInfo> GetUserInfoAsync(Account account)
        {
            var user = AppEnvironment.authService.AuthenticatedUser;
            //string token = account.Properties["access_token"];
            //string refreshToke = account.Properties["refresh_token"];
            int.TryParse(account.Properties["expires_in"], out int expriesIn);

            var parameters = new Dictionary<string, string>();

            var request = new OAuth2Request("GET", UserInfoUri, parameters, account);

            using (var response = await request.GetResponseAsync())
            {
                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    string userJson = await response.GetResponseTextAsync();
                    var kakaoUser = JsonConvert.DeserializeObject<KakaoUser>(userJson);

                    if (user == null)
                    {
                        user = new UserInfo
                        {
                            Id = kakaoUser.Id,
                            Provider = SNSProvider.Kakao,
                            LoggedInWithSNSAccount = true
                        };
                    }
                    else
                    {
                        user.Id = kakaoUser.Id;
                        user.Provider = SNSProvider.Kakao;
                        user.LoggedInWithSNSAccount = true;
                    }
                }
            }

            return user;
        }

        public override async Task<(bool isRefresh, UserInfo user)> RefreshTokenAsync(UserInfo user)
        {
            bool refreshSuccess = false;
            //var user = AppSettings.User;

            if (user == null)
            {
                return (refreshSuccess, user);
            }

            var dictionary = new Dictionary<string, string> { { "grant_type", "refresh_token" }, { "refresh_token", user.RefreshToken }, { "client_id", ClientId } };
            var request = new Request("POST", RequestTokenUri, dictionary, null);

            using (var response = await request.GetResponseAsync())
            {
                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    string tokenString = await response.GetResponseTextAsync();
                    var jwtDynamic = JsonConvert.DeserializeObject<JObject>(tokenString);
                    var accessToken = jwtDynamic.Value<string>("access_token");
                    var refreshToken = jwtDynamic.Value<string>("refresh_token");
                    var expiresIn = jwtDynamic.Value<int>("expires_in");


                    user.Token = accessToken;
                    user.RefreshToken = refreshToken;
                    user.ExpiresIn = DateTime.UtcNow.Add(new TimeSpan(0, 0, expiresIn));

                    refreshSuccess = true;
                }
            }

            return (refreshSuccess, user);
        }
    }
}
