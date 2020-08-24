($.fn as any).isInViewport = function() {
  var elementTop = $(this).offset().top;
  var elementBottom = elementTop + $(this).outerHeight();

  var viewportTop = $(window).scrollTop();
  var viewportBottom = viewportTop + $(window).height();

  return elementBottom > viewportTop && elementTop < viewportBottom;
};

($.fn as any).setCounterText = function(val: any) {
  while (/(\d+)(\d{3})/.test(val.toString())) {
    val = val.toString().replace(/(\d+)(\d{3})/, "$1" + "'" + "$2");
  }

  $(this).text(val);
};

$(window).scroll(function () {
  if (($('.co-counter-wrapper') as any).isInViewport()) {
    if(!$('.co-counter-wrapper').hasClass('isInViewport')){
      $('.counter').each(function() {
        var $this = $(this);
        var countTo = $this.attr('data-count');

        $({
          countNum: $this.text()
        }).animate({
          countNum: countTo
        }, {
          duration: 2000,
          easing: 'linear',
          step: function() {
            ($this as any).setCounterText(Math.floor((this.countNum as any)));
          },
          complete: function() {
            ($this as any).setCounterText(this.countNum);
          }
        });
      });
    }

    $('.co-counter-wrapper').addClass('isInViewport');
  }
});