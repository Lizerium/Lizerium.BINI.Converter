/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 20 октября 2026 06:52:38
 * Version: 1.0.195
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniEntry(string Name, IReadOnlyList<BiniValue> Values);
}
