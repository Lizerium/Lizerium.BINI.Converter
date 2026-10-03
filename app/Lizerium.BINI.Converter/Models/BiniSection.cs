/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 03 октября 2026 06:52:23
 * Version: 1.0.179
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniSection(string Name, IReadOnlyList<BiniEntry> Entries);
}
