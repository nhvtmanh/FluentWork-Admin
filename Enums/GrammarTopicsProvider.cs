using Microsoft.AspNetCore.Mvc.Rendering;

namespace FluentWork_Admin.Enums
{
    public static class GrammarTopicsProvider
    {
        public static List<SelectListItem> GetTopics() => new()
        {
            new SelectListItem { Value = "Tenses", Text = "Tenses" },
            new SelectListItem { Value = "Modal verbs", Text = "Modal verbs" },
            new SelectListItem { Value = "Subjunctive mood", Text = "Subjunctive mood" },
            new SelectListItem { Value = "Tag questions", Text = "Tag questions" },
            new SelectListItem { Value = "Comparison", Text = "Comparison" },
            new SelectListItem { Value = "Articles", Text = "Articles" },
            new SelectListItem { Value = "Word forms", Text = "Word forms" },
            new SelectListItem { Value = "Prepositions", Text = "Prepositions" },
            new SelectListItem { Value = "Conjunctions", Text = "Conjunctions" },
            new SelectListItem { Value = "Passive voices", Text = "Passive voices" },
            new SelectListItem { Value = "Conditional sentences", Text = "Conditional sentences" },
            new SelectListItem { Value = "Inversions", Text = "Inversions" },
            new SelectListItem { Value = "Relative clauses", Text = "Relative clauses" },
            new SelectListItem { Value = "Idioms", Text = "Idioms" },
            new SelectListItem { Value = "Collocations", Text = "Collocations" }
        };
    }
}
