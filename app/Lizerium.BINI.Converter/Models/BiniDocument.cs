/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 01 ноября 2026 11:15:17
 * Version: 1.0.207
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniDocument(IReadOnlyList<BiniSection> Sections);
}
