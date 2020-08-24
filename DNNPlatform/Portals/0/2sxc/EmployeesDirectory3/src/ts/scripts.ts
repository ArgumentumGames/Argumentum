declare const google : any;

/* Google Maps API Key */
(function ($) {
  const winAny = window as any;
  let googleApiKey = winAny.googleMapsApiKey;
  if(!googleApiKey) return;

  const showApiKeyWarning = googleApiKey.indexOf('warning!') > -1;
  googleApiKey = googleApiKey.replace('warning!', '');

  // the init-code
  function initGoogleMaps() {

    // if this had already been run, stop here
    if (($.fn as any).toGoogleMap) {
      return;
    }

    var googleMapLoadDeferred = $.Deferred();

    ($.fn as any).toGoogleMap = function (position: any) {
      var mapElement = this.get(0);

      googleMapLoadDeferred.promise().then(function () {
        var mapOptions = {
          zoom: 14,
          mapTypeControl: true,
          center: position,
          zoomControl: true,
          scaleControl: true,
          scrollwheel: false,
          mapTypeId: 'roadmap'
        };

        var map = new google.maps.Map(mapElement, mapOptions);

        // Create Marker
        var marker = new google.maps.Marker({position: position, map: map});

      });

      return this;
    };

    // Register google map load callback
    winAny.googleMapLoadCallback = function () {
      googleMapLoadDeferred.resolve(true);
    };

    if (!winAny.googleMapsLoaded) {
      winAny.googleMapsLoaded = true;
      $.getScript('//maps.google.com/maps/api/js?key=' + googleApiKey + '&sensor=true&callback=googleMapLoadCallback');
    }
  }

  // check if it's the original key, which shouldn't be used in live sites
  // do not turn this off, it's important!
  // to disable the warning, replace the API key as explained in this checklist: https://azing.org/2sxc/r/XTj19dNS
  function showWarningIfDemoKeyIsUsed() {
    if (showApiKeyWarning) {
      $('.app-empdir-google-map-container').append('<p class="alert alert-danger googlemap-apiwarning"><strong>Warning:</strong> This map uses a demo API-Key, which will cause problems on live web sites. Change the GoogleApiKey using <a class="alert-link target="_blank" href="https://azing.org/2sxc/r/XTj19dNS" target="_blank">these instructions</a></p>');
    }
  }

  $(showWarningIfDemoKeyIsUsed);

  initGoogleMaps();

}(jQuery));