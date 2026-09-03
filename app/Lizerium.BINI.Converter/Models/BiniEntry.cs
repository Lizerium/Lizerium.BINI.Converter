/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 03 сентября 2026 07:17:13
 * Version: 1.0.149
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniEntry(string Name, IReadOnlyList<BiniValue> Values);
}
