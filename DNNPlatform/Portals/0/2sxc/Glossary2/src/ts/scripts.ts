$(document).ready(function(){
  // smooth scrolling
  $('.app-glossary a[href*="#"]:not([href="#"])').click(function() {
    var target = $((this as any).hash);
    if (target.length) {
      $('html, body').animate({
        scrollTop: target.offset().top - 120 // fixed-header height
      }, 500);
    }
  });
})