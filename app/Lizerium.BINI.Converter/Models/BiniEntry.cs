/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 24 июня 2026 10:34:32
 * Version: 1.0.77
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniEntry(string Name, IReadOnlyList<BiniValue> Values);
}
