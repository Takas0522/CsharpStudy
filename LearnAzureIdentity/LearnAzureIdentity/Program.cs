using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.IO;

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

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SettingsInit();

            var cred = GenTokenCredential(CertType.ClientCertificateCredential);
            var client = new SecretClient(new Uri(KeyVaultUrl), cred);
            var d = client.GetSecret("test-hoge");
            var val = d.Value;
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
        }

        private static TokenCredential GenTokenCredential(CertType certType)
        {
            switch (certType)
            {
                case CertType.ClientCertificateCredential:
                    // 証明書認証
                    // Azure KVで証明書生成してPFXファイル出力してCertPath通したらいけた
                    return new ClientCertificateCredential(TenantId, ClientId, CertPath);




                case CertType.AuthorizationCodeCredential:
                    // ユーザー情報込のやつ。/authで取得したAuthorizationCodeを使用する?
                    return new AuthorizationCodeCredential("","","","");
                case CertType.AzureCliCredential:
                    // Azure CLIを使用している場合これを介して取得できる?
                    return new AzureCliCredential();
                case CertType.AzurePowerShellCredential:
                    // PowerShellコマンドでAzureへの認証を通している場合これを介して取得できる?
                    return new AzurePowerShellCredential();
                case CertType.ChainedTokenCredential:
                    // 複数のTokenCredentialを組み合わせる場合に使用。多分DefaultCredと同じようなことができる？
                    return new ChainedTokenCredential();
                case CertType.ClientSecretCredential:
                    // シークレット認証?
                    return new ClientSecretCredential("", "", "");
                case CertType.DefaultAzureCredential:
                    // 通常の。Env->CLIのあれの順番
                    return new DefaultAzureCredential();
                case CertType.DeviceCodeCredential:
                    return new DeviceCodeCredential();
                case CertType.EnvironmentCredential:
                    return new EnvironmentCredential();
                case CertType.InteractiveBrowserCredential:
                    // ユーザーにID/Pass入力してもらうあれ？
                    return new InteractiveBrowserCredential();
                case CertType.ManagedIdentityCredential:
                    // ManagedIdentity？
                    return new ManagedIdentityCredential();
                case CertType.OnBehalfOfCredential:
                    // OnBeHalfOfFlow。アクセストークンとシークレット
                    return new OnBehalfOfCredential("","","","");
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
