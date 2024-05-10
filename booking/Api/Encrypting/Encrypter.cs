using System.Security.Cryptography;
using System.Text;

namespace booking_api.Encrypting;

public class Encrypter
{
    
    //Using SHA-256 to hash the password
    public static string GetHash(string value)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA256.Create())            
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));
            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }
        return Sb.ToString();
    }
}