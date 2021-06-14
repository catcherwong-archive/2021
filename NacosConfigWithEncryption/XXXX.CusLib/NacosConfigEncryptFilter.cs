namespace XXXX.CusLib
{
    using Nacos.V2;
    using Nacos.V2.Config.Abst;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class NacosConfigEncryptFilter : IConfigFilter
    {
        private static readonly string DefaultKey = "catcherwong00000";

        public void DoFilter(IConfigRequest request, IConfigResponse response, IConfigFilterChain filterChain)
        {
            if (request != null)
            {
                var encryptedDataKey = DefaultKey;
                var raw_content = request.GetParameter(Nacos.V2.Config.ConfigConstants.CONTENT);

                // 部分配置加密后的 content
                var content = ReplaceJsonNode((string)raw_content, encryptedDataKey, true);

                // 加密配置后，不要忘记更新 request !!!!
                request.PutParameter(Nacos.V2.Config.ConfigConstants.ENCRYPTED_DATA_KEY, encryptedDataKey);
                request.PutParameter(Nacos.V2.Config.ConfigConstants.CONTENT, content);
            }

            if (response != null)
            {
                var resp_content = response.GetParameter(Nacos.V2.Config.ConfigConstants.CONTENT);
                var resp_encryptedDataKey = response.GetParameter(Nacos.V2.Config.ConfigConstants.ENCRYPTED_DATA_KEY);

                // nacos 2.0.2 服务端目前还没有把 encryptedDataKey 记录并返回，所以 resp_encryptedDataKey 目前只会是 null
                // 如果服务端有记录并且能返回，我们可以做到每一个配置都用不一样的 encryptedDataKey 来加解密。
                // 目前的话，只能固定一个 encryptedDataKey 
                var encryptedDataKey = (resp_encryptedDataKey == null || string.IsNullOrWhiteSpace((string)resp_encryptedDataKey)) ? DefaultKey : (string)resp_encryptedDataKey;

                var content = ReplaceJsonNode((string)resp_content, encryptedDataKey, false);
                response.PutParameter(Nacos.V2.Config.ConfigConstants.CONTENT, content);
            }
        }

        public string GetFilterName() => nameof(NacosConfigEncryptFilter);

        public int GetOrder() => 1;

        public void Init(NacosSdkOptions options)
        {
            // 从拓展信息里面获取需要加密的 json path
            // 这里只是示例，根据具体情况调整成自己合适的！！！！
            var extInfo = JObject.Parse(options.ConfigFilterExtInfo);

            if (extInfo.ContainsKey("JsonPaths"))
            {
                // JsonPaths 在这里的含义是，那个path下面的内容要加密
                _jsonPaths = extInfo.GetValue("JsonPaths").ToObject<List<string>>();
            }
        }

        public static string AESEncrypt(string data, string key)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(data);
                    byte[] bKey = new byte[32];
                    Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.KeySize = 256;
                    aes.Key = bKey;

                    using (CryptoStream cryptoStream = new CryptoStream(memory, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        try
                        {
                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            return Convert.ToBase64String(memory.ToArray());
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static string AESDecrypt(string data, string key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(data);
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

            using (MemoryStream memory = new MemoryStream(encryptedBytes))
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.KeySize = 256;
                    aes.Key = bKey;

                    using (CryptoStream cryptoStream = new CryptoStream(memory, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        try
                        {
                            byte[] tmp = new byte[encryptedBytes.Length];
                            int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                            byte[] ret = new byte[len];
                            Array.Copy(tmp, 0, ret, 0, len);

                            return Encoding.UTF8.GetString(ret, 0, len);
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            }
        }

        private List<string> _jsonPaths;

        private string ReplaceJsonNode(string src, string encryptedDataKey, bool isEnc = true)
        {
            // 示例配置用的是JSON，如果用的是 yaml，这里换成用 yaml 解析即可。
            var jObj = JObject.Parse(src);

            foreach (var item in _jsonPaths)
            {
                var t = jObj.SelectToken(item);

                if (t != null)
                {
                    var r = t.ToString();

                    // 加解密
                    var newToken = isEnc
                        ? AESEncrypt(r, encryptedDataKey)
                        : AESDecrypt(r, encryptedDataKey);

                    if (!string.IsNullOrWhiteSpace(newToken))
                    {
                        // 替换旧值
                        t.Replace(newToken);
                    }
                }
            }

            return jObj.ToString();
        }
    }
}
