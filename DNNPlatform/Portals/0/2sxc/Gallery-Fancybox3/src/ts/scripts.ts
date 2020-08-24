$(function() {
  /* Fancybox */
  if ($('.fancybox').fancybox) {
    $('.ga-fancybox').fancybox(<FancyBoxOptions>{
      loop: true,
      caption: function() {
        return $(this).find('figcaption').html();
      }
    });
  }

  /* Lazy */
  if (($('.lazy') as any).Lazy) {
    ($('.lazy') as any).Lazy({
      effect: 'fadeIn',
      effectTime: 200,
    });
  }
});