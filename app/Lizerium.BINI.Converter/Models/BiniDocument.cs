/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 09 мая 2026 07:52:28
 * Version: 1.0.31
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniDocument(IReadOnlyList<BiniSection> Sections);
}
