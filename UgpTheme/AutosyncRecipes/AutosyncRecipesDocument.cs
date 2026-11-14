using System;
using System.Collections.Generic;
using OrchardCore.Data.Documents;

namespace OrchardCore.Themes.UgpTheme.AutosyncRecipes
{
    /// <summary>
    /// Per-tenant record of which *.autosync.recipe.json files have been applied.
    /// </summary>
    public class AutosyncRecipesDocument : Document
    {
        /// <summary>Keyed by recipe file name, e.g. "ugp-gala.autosync.recipe.json".</summary>
        public Dictionary<string, AppliedRecipe> Recipes { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }

    public class AppliedRecipe
    {
        /// <summary>SHA-256 of the recipe file contents when it was last applied.</summary>
        public string Hash { get; set; }

        public DateTime AppliedUtc { get; set; }

        /// <summary>
        /// State of every content item the recipe imports, captured right after it was applied.
        /// If an item no longer matches, it was edited outside the recipe (e.g. in the admin).
        /// </summary>
        public Dictionary<string, AppliedContentItem> ContentItems { get; set; } = new();
    }

    public class AppliedContentItem
    {
        public string ContentItemVersionId { get; set; }

        public DateTime? ModifiedUtc { get; set; }
    }
}
