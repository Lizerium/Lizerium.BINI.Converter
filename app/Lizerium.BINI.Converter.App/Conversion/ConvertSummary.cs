/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 10 июля 2026 11:54:45
 * Version: 1.0.94
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
