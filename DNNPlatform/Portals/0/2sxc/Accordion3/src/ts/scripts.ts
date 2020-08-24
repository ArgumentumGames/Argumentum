$(function() {
	// attach click to all accordions when loading
	$('.co-accordion-title').click(function(e){
		e.preventDefault();
    var _this = $(this);
    var navHeight = 119;

    // add hash to url
		location.hash = $(this).attr('id');

    $(this).toggleClass('active')
      .next('.co-accordion-content').slideToggle('normal', function(){
        $(window).resize();
        $('html, body').animate({
          scrollTop: _this.offset().top - navHeight
        }, 500);
      });
	});

	// get hash from url and open specific item
	if(window.location.hash){
		var hash = window.location.hash;
		if($(hash).length > 0){
      $(hash).click();
		}
	}
});
