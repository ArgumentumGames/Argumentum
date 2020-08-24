declare const addsearch: any;

$(function() {
  (window as any).addsearch_settings = {
    display_url: true,
    display_resultscount: true,
    display_date: true,
    display_sortby: true
  };

  $("#query").keyup(function(e) {
    if(e.which == 13) {
      search();
    }
  });

  $("#queryMobile").keyup(function(e) {
    if(e.which == 13) {
      searchMobile();
    }
  });

  $("#submitSearch").click(function() {
    search();
  });

  $("#submitSearchMobile").click(function() {
    searchMobile();
  });

  // Listens to enter key on result page and execute the search
  // Prevent submit of DNN Form so the page doesn't reload
  $("#Form").submit(function() {
    addsearch.submit();
    return false;
  });

  function search() {
    var jQs = $("#query").val();
    var resultPage = $(".app-add-search-desktop").data("resultpage");
    location.href = resultPage + "?addsearch=" + jQs;
  }

  function searchMobile() {
    var jQsM = $("#queryMobile").val();
    var resultPage = $(".app-add-search-mobile").data("resultpage");
    location.href = resultPage + "?addsearch=" + jQsM;
  }

});
