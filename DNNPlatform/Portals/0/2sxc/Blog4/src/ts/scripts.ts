declare var StickySidebar: any;

$(function() {
  new StickySidebar('#sidebar', {
    containerSelector: '.app-blog',
    innerWrapperSelector: '.sidebar__inner',
    topSpacing: 160,
		bottomSpacing: 20,
  });
});
