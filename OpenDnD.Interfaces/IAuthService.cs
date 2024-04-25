using Microsoft.Extensions.ObjectPool;
using System.Buffers;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace OpenDnD.Interfaces
{
    public class AuthToken
    {
        public Guid PlayerId { get; set; }
        public string TokenValue { get; set; }
    }

    public class Secret
    {
        public string SecretKey { get; set; }
    }
    public interface IAuthService
    {
        public AuthToken Register(Uri address, string login, string password);
        public AuthToken Authenticate(Uri address, string login, string password);
        public bool ValidateAuthToken(AuthToken authToken);
    }

    public class CryptoPoolPolicy : PooledObjectPolicy<SHA512>
    {
        public override SHA512 Create() => SHA512.Create();
        public override bool Return(SHA512 obj) => true;
    }
    public static class CryptoService
    {
        //HASH(id + secret) = token
        //HASH(Password + salt) = hash
        private static ObjectPool<SHA512> CryptoPool = new DefaultObjectPool<SHA512>(new CryptoPoolPolicy(), 100);
        public static string GetAuthToken(Guid playerId, string secretValue)
            => GetSHA512(Encoding.UTF8.GetString(playerId.ToByteArray()) + secretValue);
        public static bool ValidateAuthToken(AuthToken authToken, string secretValue)
            => GetSHA512(Encoding.UTF8.GetString(authToken.PlayerId.ToByteArray()) + secretValue) == authToken.TokenValue;
        public static (string hash, string salt) GetHashSaltPair(string password)
        {
            var salt = GetRandonString();
            var hash = GetSHA512(password + salt);
            return (hash, salt);
        }
        public static bool ValidatePasswordSaltHash(string password, string salt, string hash)
            => GetSHA512(password + salt) == hash;
        

        public static byte[] GetSHA512(byte[] bytes)
        {
            var instane = CryptoPool.Get();
            var hash = instane.ComputeHash(bytes);
            CryptoPool.Return(instane);
            return hash;
        }
        public static string GetSHA512(string x)
        {
            var bytes = Encoding.UTF8.GetBytes(x);
            var hash = GetSHA512(bytes);
            var result = Encoding.UTF8.GetString(hash);
            return result;
        }
        public static string GetRandonString()
        {
            var bytes = new byte[2048];
            Random.Shared.NextBytes(bytes);
            bytes = GetSHA512(bytes);
            return Encoding.UTF8.GetString(bytes);
        }
    }

}
