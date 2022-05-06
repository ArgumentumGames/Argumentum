using ToSic.Razor.Blade;
using System.Linq;
using System;
using System.Collections.Generic;

public class Helpers: Custom.Hybrid.Code12
{
  /// <summary>
  /// Get all the questions and sort as is configured
  /// </summary>
  public IEnumerable<dynamic> GetQuestionsSorted() {
    var questions = AsList(App.Data["Question"] as object);
    var filterCats = AsList(Content.Categories as object);

    // if we have categories, then filter with these
    if(filterCats != null && filterCats.Any()) {
      questions = questions.Where(q => (q.Categories as IEnumerable<dynamic>).Any(qCat => filterCats.Any(fCat => fCat.Key == qCat.Key)));
    }

    // now sort by priority, big numbers first
    var sorted = questions.OrderByDescending(q => q.Priority);

    // now sort the results by the second parameter
    switch(Content.SortOrder as string) {
      case "DESC-id-ASC": return sorted.ThenBy(q => q.EntityId);
      case "DESC-id-DESC": return sorted.ThenByDescending(q => q.EntityId);
      case "DESC-title-ASC": return sorted.ThenBy(q => q.Title);
      default: return sorted; // if no additional sort order was used, return sorted by priority only
    }
  }

  /// <summary>
  /// Create a hover-help-label for admins to better manage the questions
  /// </summary>
  public object AdminHelperLabel(dynamic q) {
    var itemCategories = AsList(q.Categories as object).Select(cat => cat.Name);
    var label = q.InternalTitle + ", "
      + "Prio: " + q.Priority + ", "
      + "Categories: " + String.Join(",", itemCategories);
    return Edit.Attribute("title", label);
  }
}