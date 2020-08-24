declare let $2sxc: any;
declare let fancybox: any;

require('../scss/_Styles.scss');


if($('body').hasClass('role-admin')) {
  const hotspotImages = document.querySelectorAll('.hotspots');

  hotspotImages.forEach(el => el.addEventListener('click', event => {
    const target = (event.target as HTMLElement);
  
    if(!target.classList.contains('hotspot-image')) {
      return false;
    }
  
    const e = (event as MouseEvent);
    const moduleId = target.parentElement.dataset.moduleId;
    const entityId = target.parentElement.dataset.entityId
    const guid = target.parentElement.dataset.guid
    const bounds = target.getBoundingClientRect();
    const iconOffsetX = target.parentElement.dataset.iconoffsetX;
    const iconOffsetY = target.parentElement.dataset.iconoffsetY;
    const x = e.clientX - bounds.left - parseInt(iconOffsetX);
    const y = e.clientY - bounds.top - parseInt(iconOffsetY);
    const xPercent = x / bounds.width * 100;
    const yPercent = y / bounds.height * 100;
  
    $2sxc(moduleId).manage.run({
      'action': 'new',
      'sortOrder': 0,
      'isPublished': true,
      'entityId': entityId,
      'parent': guid,
      'fields': 'Hotspots',
      'prefill': {
        'X': Math.round(xPercent * 100) / 100,
        'Y': Math.round(yPercent * 100) / 100
      }
    });
  }));
}

($('.hotspots').find('[data-fancybox]') as any).fancybox({
  afterShow : function( instance: any, current: any ) {
      const imgWidth = $(current.src).find('img').width();
      if(!$(current.src).find('.fancybox-copy').attr('style')) {
          $(current.src).find('.fancybox-copy').css('max-width', imgWidth);
      }
      $(current.src + ".fancybox-hotspot-content").css('opacity', 1)
  }
});