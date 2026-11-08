using System;
using System.Collections.Concurrent;
using System.Reflection;

using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Mindmapper
{
	/// <summary>
	/// #1471 (badge reported from #1444): language badge for mind-map nodes whose served link is a
	/// CROSS-LANGUAGE fallback. The link cascade (<c>Link&lt;Lang&gt;Fallback = link_&lt;lang&gt; ?? link_en ??
	/// link_fr</c>, <see cref="Fallacy"/> / <see cref="Virtue"/>) guarantees a link in every map, but a reader
	/// of the <c>ru</c> map then lands on an ENGLISH or FRENCH page without being told. The badge appends
	/// « (en) » / « (fr) » to the node TEXT at .mm generation time, so it flows into the SVGs at the next
	/// FreeMind pass — the committed SVGs/wrappers are untouched until then (same one-pass-ahead contract
	/// as the #2190 tints).
	/// <para>
	/// The origin is derived from the SAME fields the cascade reads (native <c>Link&lt;Lang&gt;</c>, then
	/// <c>LinkEn</c>, then <c>LinkFr</c>), NOT by probing the URL — the cascade is deterministic and
	/// #1468/#1470 keep <c>link_&lt;lang&gt;</c> free of foreign URLs, so the column that served the link IS
	/// the page language. The dest-language <paramref name="language"/> is the one the pipeline passes to
	/// <c>CreateNode</c> (the token-swap of <c>LinkExpression</c> in AssetConverterConfig.MindMapLocalization
	/// rewrites <c>LinkFrFallback</c> → <c>Link&lt;Lang&gt;Fallback</c> with the same codes); an overridden
	/// <c>LinkExpression</c> bypassing the cascade would desynchronize badge and link — the call sites only
	/// badge when the resolved link is non-empty, which keeps the standard config coherent.
	/// </para>
	/// </summary>
	internal static class MindMapLinkBadge
	{
		/// <summary>
		/// The language the served link will fall back to for <paramref name="language"/>'s map, or
		/// <c>null</c> when the native link is present or no link exists at all (no badge).
		/// </summary>
		public static string GetBadgeSourceLanguage(IMindMapItem item, string language)
		{
			if (item == null || string.IsNullOrEmpty(language))
			{
				return null;
			}

			// A MISSING property (language unknown to the entity) aborts the derivation — the
			// native state cannot be known, so no badge can be derived. It must NOT fall through
			// to the en/fr hops: that would badge an en link on a map whose own cascade is unknown.
			if (!TryGetLinkValue(item, language, out var native))
			{
				return null;
			}

			if (!string.IsNullOrEmpty(native))
			{
				return null; // native link served — no badge
			}

			if (!TryGetLinkValue(item, "en", out var en))
			{
				return null; // defensive: LinkEn missing = entity broken (logged once)
			}

			if (!string.IsNullOrEmpty(en))
			{
				return "en"; // cascade hop 1: link_xx ?? link_en
			}

			if (!TryGetLinkValue(item, "fr", out var fr))
			{
				return null; // defensive: LinkFr missing = entity broken (logged once)
			}

			return string.IsNullOrEmpty(fr) ? null : "fr"; // cascade hop 2: ?? link_fr — empty means no link anywhere
		}

		/// <summary>The ready-to-append TEXT suffix: «&nbsp; (en) » / «&nbsp; (fr) », or the empty string.</summary>
		public static string GetBadge(IMindMapItem item, string language)
		{
			var source = GetBadgeSourceLanguage(item, language);
			return source == null ? "" : $" ({source})";
		}

		/// <summary>
		/// Reflection cache: property name « Link{Lang} » (LinkFr, LinkEn, LinkRu…) per concrete entity
		/// type. Mind-map generation clones the config per language and can run languages in parallel —
		/// the cache is a ConcurrentDictionary so concurrent lookups stay safe. A missing property is
		/// cached as null and logged ONCE (fail-loud declared gap: the language list diverged from the
		/// entity), never per node.
		/// </summary>
		private static readonly ConcurrentDictionary<(Type EntityType, string Language), PropertyInfo> LinkPropertyCache =
			new ConcurrentDictionary<(Type, string), PropertyInfo>();

		private static readonly ConcurrentDictionary<(Type EntityType, string Language), bool> LoggedPropertyMisses =
			new ConcurrentDictionary<(Type, string), bool>();

		private static bool TryGetLinkValue(IMindMapItem item, string language, out string value)
		{
			var key = (item.GetType(), language);
			var property = LinkPropertyCache.GetOrAdd(key, static (k, lang) =>
			{
				var propertyName = "Link" + char.ToUpperInvariant(lang[0]) + lang.Substring(1);
				return k.EntityType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
			}, language);

			if (property == null)
			{
				// The miss is cached, but the null check above runs per call — dedupe the LOG the
				// same way, or a 1408-node generation in an unknown language emits 1408 problems.
				if (LoggedPropertyMisses.TryAdd(key, true))
				{
					Logger.LogProblem(
						$"#1471 badge: no public property 'Link{char.ToUpperInvariant(language[0])}{language.Substring(1)}' on {item.GetType().Name} " +
						$"- cannot derive the link origin for language '{language}', nodes of this map get no badge.");
				}
				value = null;
				return false;
			}

			value = property.GetValue(item) as string;
			return true;
		}
	}
}
