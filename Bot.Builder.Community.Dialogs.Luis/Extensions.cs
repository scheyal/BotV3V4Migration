using Bot.Builder.Community.Dialogs.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Bot.Builder.Community.Dialogs.Luis.BuiltIn.DateTime;

namespace Bot.Builder.Community.Dialogs.Luis
{
    /// <summary>
    /// LUIS extension methods.
    /// </summary>
    public static partial class Extensions
    {
        public static T MaxBy<T, R>(this IEnumerable<T> items, Func<T, R> selectRank, IComparer<R> comparer = null)
        {
            comparer = comparer ?? Comparer<R>.Default;

            var bestItem = default(T);
            var bestRank = default(R);
            using (var item = items.GetEnumerator())
            {
                if (item.MoveNext())
                {
                    bestItem = item.Current;
                    bestRank = selectRank(item.Current);
                }

                while (item.MoveNext())
                {
                    var rank = selectRank(item.Current);
                    var compare = comparer.Compare(rank, bestRank);
                    if (compare > 0)
                    {
                        bestItem = item.Current;
                        bestRank = rank;
                    }
                }
            }

            return bestItem;
        }

        /// <summary>
        /// Try to find an entity within the result.
        /// </summary>
        /// <param name="result">The LUIS result.</param>
        /// <param name="type">The entity type.</param>
        /// <param name="entity">The found entity.</param>
        /// <returns>True if the entity was found, false otherwise.</returns>
        public static bool TryFindEntity(this LuisResult result, string type, out EntityRecommendation entity)
        {
            Func<EntityRecommendation, IList<EntityRecommendation>, bool> doesNotOverlapRange = (current, recommendations) =>
            {
                return !recommendations.Where(r => current != r)
                            .Any(r => r.StartIndex.HasValue && r.EndIndex.HasValue && current.StartIndex.HasValue && 
                                 r.StartIndex.Value <= current.StartIndex.Value && r.EndIndex.Value >= current.EndIndex.Value);
            };


            // find the recommended entity that does not overlap start and end ranges with other result entities
            entity = result.Entities?.Where(e => e.Type == type && doesNotOverlapRange(e, result.Entities)).FirstOrDefault();
            return entity != null;
        }

        /// <summary>
        /// Parse all resolutions from a LUIS result.
        /// </summary>
        /// <param name="parser">The resolution parser.</param>
        /// <param name="entities">The LUIS entities.</param>
        /// <returns>The parsed resolutions.</returns>
        public static IEnumerable<Resolution> ParseResolutions(this IResolutionParser parser, IEnumerable<EntityRecommendation> entities)
        {
            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    Resolution resolution;
                    if (parser.TryParse(entity.Resolution, out resolution))
                    {
                        yield return resolution;
                    }
                }
            }
        }

        /// <summary>
        /// Return the next <see cref="DayPart"/>. 
        /// </summary>
        /// <param name="part">The <see cref="DayPart"/> query.</param>
        /// <returns>The next <see cref="DayPart"/> after the query.</returns>
        public static DayPart Next(this DayPart part)
        {
            switch (part)
            {
                case DayPart.MO: return DayPart.MI;
                case DayPart.MI: return DayPart.AF;
                case DayPart.AF: return DayPart.EV;
                case DayPart.EV: return DayPart.NI;
                case DayPart.NI: return DayPart.MO;
                default: throw new NotImplementedException();
            }
        }

    }
}
