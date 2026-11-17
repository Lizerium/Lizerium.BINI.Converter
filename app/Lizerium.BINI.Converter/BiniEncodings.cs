/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 17 ноября 2026 10:41:35
 * Version: 1.0.223
 */

using System.Text;

namespace Lizerium.BINI.Converter;

public static class BiniEncodings
{
    public static Encoding Windows1251 { get; } = GetCodePageEncoding(1251);

    public static Encoding Latin1 { get; } = Encoding.Latin1;

    private static Encoding GetCodePageEncoding(int codePage)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        return Encoding.GetEncoding(codePage);
    }
}
