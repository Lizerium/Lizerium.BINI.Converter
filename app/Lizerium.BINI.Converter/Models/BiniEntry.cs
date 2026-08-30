/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 30 августа 2026 06:52:29
 * Version: 1.0.145
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniEntry(string Name, IReadOnlyList<BiniValue> Values);
}
