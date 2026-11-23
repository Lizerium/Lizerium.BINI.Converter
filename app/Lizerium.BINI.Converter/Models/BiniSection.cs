/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 23 ноября 2026 06:51:49
 * Version: 1.0.229
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniSection(string Name, IReadOnlyList<BiniEntry> Entries);
}
