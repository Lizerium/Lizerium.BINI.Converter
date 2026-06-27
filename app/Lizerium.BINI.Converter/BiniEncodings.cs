/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 27 июня 2026 13:13:48
 * Version: 1.0.80
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
