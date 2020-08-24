// TODO: don't replace onload

(function () {
  window.addEventListener('load', function(e) {
    initUrlTracking();
    initEventTracking();
    initMobiusFormTracking();

    doLandingPageTracking('landing-page', 'visit', '');
    
    const ctaBtn = document.querySelectorAll('.cta');
    for(var i = 0; i < ctaBtn.length; i++) {
      ctaBtn[i].addEventListener("click", (e) => {
        const _this = (e.currentTarget as HTMLElement);
        const href = _this.getAttribute('href');
        const ctaForm = document.querySelector('.cta-form');

        if(href == "#cta-form" && ctaForm !== null) {
          const offY = ctaForm.getBoundingClientRect().top + window.scrollY;
          
          window.scrollTo({ top: offY - 100, left: 0, behavior: "smooth" })
        }
      })
    }
  })

  function initMobiusFormTracking() {
    document.addEventListener("trackMobiusForm", function(event) {
      const details = (event as any).detail;
      doLandingPageTracking(details.category, details.action, details.label);
    });
  }

  function initUrlTracking() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    const utmSource = urlParams.get('utm_source');
    const utmMedium = urlParams.get('utm_medium');
    const utmCampaign = urlParams.get('utm_campaign');

    if(utmSource !== null && utmMedium !== null && utmCampaign !== null) {
      doLandingPageTracking('landing-page', utmSource, utmCampaign);
    }
  }

  function initEventTracking() {
    const eventAction = document.querySelectorAll('[data-trackingevent]');
    
    for(var i = 0; i < eventAction.length; i++) {
      eventAction[i].addEventListener("click", (e) => {
        const _this = (e.currentTarget as HTMLElement);
        const action = _this.dataset.trackingevent;
        const label = _this.textContent;

        doLandingPageTracking('landing-page', action, label);
      })
    }
  }

  function doLandingPageTracking (category: string, action: string, label: string) {
    if((window as any).ga && ga.create) {
      ga('gtm1.send', 'event', category, action, label);
    }
  }
})();