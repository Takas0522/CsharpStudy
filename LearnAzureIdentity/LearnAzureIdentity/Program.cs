using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LearnAzureIdentity
{

    class Program
    {
        private enum CertType
        {
            AuthorizationCodeCredential,
            AzureCliCredential,
            AzurePowerShellCredential,
            ChainedTokenCredential,
            ClientCertificateCredential,
            ClientSecretCredential,
            DefaultAzureCredential,
            DeviceCodeCredential,
            EnvironmentCredential,
            InteractiveBrowserCredential,
            ManagedIdentityCredential,
            OnBehalfOfCredential,
            SharedTokenCacheCredential,
            UsernamePasswordCredential,
            VisualStudioCodeCredential,
            VisualStudioCredential,
            TokenAcquisitionTokenCredential
        }

        private static string KeyVaultUrl = "";
        private static string ClientId = "";
        private static string TenantId = "";
        private static string CertPath = "";
        private static string AuthCode = "";
        private static string ClientSecret = "";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            SettingsInit();

            var cred = GenTokenCredential(CertType.OnBehalfOfCredential);
            var client = new SecretClient(new Uri(KeyVaultUrl), cred);
            var d = client.GetSecret("test-hoge");
            var val = d.Value;
        }

        private static string GetAccessToken()
        {
            IPublicClientApplication app = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, TenantId).WithRedirectUri("http://localhost").Build();
            var res = app.AcquireTokenInteractive(new List<string> { "api://63569bf4-d27a-4c67-9401-98646550b3a1/access" }).ExecuteAsync().Result;
            return res.AccessToken;
        }

        private static void SettingsInit()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json")
                .Build();
            KeyVaultUrl = configuration["KeyVaultUrl"];
            ClientId = configuration["ClientId"];
            TenantId = configuration["TenantId"];
            CertPath = configuration["CertPath"];
            AuthCode = configuration["AuthCode"];
            ClientSecret = configuration["ClientSecret"];
        }

        private static TokenCredential GenTokenCredential(CertType certType)
        {
            var accessToken = "";
            if (certType == CertType.OnBehalfOfCredential)
            {
                accessToken = GetAccessToken();
            }
            switch (certType)
            {
                case CertType.ClientCertificateCredential:
                    // 証明書認証
                    // Azure KVで証明書生成してPFXファイル出力してCertPath通したらいけた
                    return new ClientCertificateCredential(TenantId, ClientId, CertPath);

                case CertType.AuthorizationCodeCredential:
                    // ユーザー情報込のやつ。/authで取得したAuthorizationCodeを使用する
                    return new AuthorizationCodeCredential(TenantId, ClientId, ClientSecret, AuthCode, new AuthorizationCodeCredentialOptions { RedirectUri = new Uri(@"http://localhost/") });

                case CertType.AzureCliCredential:
                    // Azure CLIを使用している場合これを介して取得できる
                    return new AzureCliCredential(new AzureCliCredentialOptions { TenantId = TenantId });

                case CertType.AzurePowerShellCredential:
                    // PowerShellコマンドでAzureへの認証を通している場合これを介して取得できる
                    return new AzurePowerShellCredential(new AzurePowerShellCredentialOptions { TenantId = TenantId });

                case CertType.ChainedTokenCredential:
                    // 複数のTokenCredentialを組み合わせる場合に使用。DefaultCredと同じようなことができる
                    // PowerShell -> AzureCLI -> ClientCertificate
                    var first = new AzurePowerShellCredential(new AzurePowerShellCredentialOptions { TenantId = TenantId });
                    var secound = new AzureCliCredential(new AzureCliCredentialOptions { TenantId = TenantId });
                    var last = new ClientCertificateCredential(TenantId, ClientId, CertPath);
                    var list = new List<TokenCredential> { first, secound, last };
                   
                    return new ChainedTokenCredential(list.ToArray());

                case CertType.ClientSecretCredential:
                    // シークレット認証?
                    return new ClientSecretCredential(TenantId, ClientId, ClientSecret);

                case CertType.DefaultAzureCredential:
                    // 通常の。
                    return new DefaultAzureCredential();

                case CertType.DeviceCodeCredential:
                    // デバイスコード認証
                    return new DeviceCodeCredential();

                case CertType.EnvironmentCredential:
                    // 環境変数
                    return new EnvironmentCredential();

                case CertType.InteractiveBrowserCredential:
                    // ユーザーにID/Pass入力してもらうあれ
                    return new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions { ClientId = ClientId, TenantId = TenantId, RedirectUri= new Uri(@"http://localhost/") });

                case CertType.ManagedIdentityCredential:
                    // ManagedIdentity
                    return new ManagedIdentityCredential();

                case CertType.OnBehalfOfCredential:
                    // OnBeHalfOfFlow。アクセストークンとシークレット
                    return new OnBehalfOfCredential(TenantId, ClientId, ClientSecret, accessToken);

                case CertType.SharedTokenCacheCredential:
                    return new SharedTokenCacheCredential();
                case CertType.UsernamePasswordCredential:
                    // Basicなやつ?
                    return new UsernamePasswordCredential("","","","");
                case CertType.VisualStudioCodeCredential:
                    // VisualStudioCodeの資格情報?
                    return new VisualStudioCodeCredential();
                case CertType.VisualStudioCredential:
                    // VisualStudioの資格情報?
                    return new VisualStudioCredential();
                case CertType.TokenAcquisitionTokenCredential:
                    // Microsoft.Identity.WebのOnBeHalfOfFlowのアレのはず?
                    return new TokenAcquisitionTokenCredential(null);
                default:
                    return null;
            }
        }

    }
}
