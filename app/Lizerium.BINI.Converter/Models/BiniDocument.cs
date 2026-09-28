/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 28 сентября 2026 12:08:41
 * Version: 1.0.174
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniDocument(IReadOnlyList<BiniSection> Sections);
}
