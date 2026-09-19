/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 19 сентября 2026 08:45:44
 * Version: 1.0.165
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniSection(string Name, IReadOnlyList<BiniEntry> Entries);
}
