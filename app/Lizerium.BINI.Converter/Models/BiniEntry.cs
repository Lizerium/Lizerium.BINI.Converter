/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 04 июля 2026 08:47:17
 * Version: 1.0.88
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniEntry(string Name, IReadOnlyList<BiniValue> Values);
}
