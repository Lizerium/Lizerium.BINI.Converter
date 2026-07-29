/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 29 июля 2026 15:45:00
 * Version: 1.0.113
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
