/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 18 июля 2026 07:27:13
 * Version: 1.0.102
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
