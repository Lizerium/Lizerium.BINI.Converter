/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 21 сентября 2026 09:34:38
 * Version: 1.0.167
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniSection(string Name, IReadOnlyList<BiniEntry> Entries);
}
