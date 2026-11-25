/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 25 ноября 2026 11:15:01
 * Version: 1.0.231
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal sealed class ConvertSummary
{
    public int Converted { get; set; }

    public int Skipped { get; set; }

    public int Failed { get; set; }

    public void Add(ConvertSummary other)
    {
        Converted += other.Converted;
        Skipped += other.Skipped;
        Failed += other.Failed;
    }
}
