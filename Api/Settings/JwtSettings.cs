using System.Text;

namespace Api.Settings;

public class JwtSettings
{
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string Key { get; init; } = null!;

    public byte[] GetKey()
    {
        return Encoding.UTF8.GetBytes(Key);
    }
}
