declare var Cookies: any;

$(function() {

  /**
   * Show popup if it was rendered
   */
  if($(".app-popup:not(.admin)").length) {
    window.setTimeout(() => {
      $(".app-popup").fadeIn();
      $("body").css("overflow", "hidden");
      Cookies.set('seenpopup', 'true', { expires: $(".app-popup").data("cookie-duration") });
    }, 5000)
  }


  /**
   * Administration functions
   */

  $("#app-popup-test-popup").click(function() {
    $(".app-popup").fadeIn();
    $("body").css("overflow", "hidden");
  });


  /**
   * Close functions
   */

  function popupOut() {
    $(".app-popup").fadeOut();
    $("body").css("overflow", "auto");
  }

  // Click in BG
  $(".app-popup .background").click(function() {
    popupOut();
  });

  // Click on close
  $(".app-popup .popup-close").click(function() {
    popupOut();
  });

  // Press ESC on Keyboard
  $(document).keyup(function(e) {
    if (e.keyCode == 27) {
      popupOut();
    }
  });
});