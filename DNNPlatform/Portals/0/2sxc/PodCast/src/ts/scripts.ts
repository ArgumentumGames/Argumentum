$(document).ready(function(){
  /* Configure the jPlayers */
  $('.jp-jplayer').each(function() {
    var episodeNumber = $(this).data('episode-number');

    ($(this) as any).jPlayer({
      ready: function () {
        ($(this) as any).jPlayer('setMedia', {
            mp3: $(this).data('episode-audio'),
        });
      },
      play: function () {
        // exchange play with pause icon
        $('.jp-play-' + episodeNumber + ' svg').attr('data-icon', 'pause');
        $('.jp-play-' + episodeNumber + ' svg').removeClass('fa-play').addClass('fa-pause');

        showAudioPlayerElements(episodeNumber);

        // pause other instances of player when current one plays
        ($(this) as any).jPlayer('pauseOthers');
      },
      timeupdate: function(event: any) {
        $('.pc-drag-handler-' + episodeNumber).css('left', event.jPlayer.status.currentPercentAbsolute + '%');
      },
      pause: function () {
        // exchange pause with play icon
        $('.jp-play-' + episodeNumber + ' svg').attr('data-icon', 'play');
        $('.jp-play-' + episodeNumber + ' svg').removeClass('fa-pause').addClass('fa-play');
      },
      ended: function () {
        // reset the drag handler
        $('.pc-drag-handler-' + episodeNumber).css('left', '0');

        hideAudioPlayerElements(episodeNumber);
      },
      cssSelectorAncestor: '#jp_container_' + episodeNumber,
      swfPath: '/dist/lib',
      supplied: 'mp3',
      useStateClassSkin: true,
      remainingDuration: true,
      toggleDuration: true,
      preload:  'none',
    })
  });

  /* Format the episode duration */
  $('.pc-episode-duration').each(function() {
    var hr = ~~(($(this) as any).context.innerHTML / 60);
    var min = ~~(($(this) as any).context.innerHTML % 60);
    var formatedDuration = (hr < 10 ? '0' + hr : hr) + ':' + (min < 10 ? '0' + min : min) + ':00';

    ($(this) as any).context.innerHTML = formatedDuration;
  });

  /* Show audio player elements */
  var showAudioPlayerElements = function(episodeNumber: number) {
    $('.jp-audio-' + episodeNumber).css('width', '100%');
    $('.jp-interface-' + episodeNumber).css('width', '30%');
    $('.jp-controls-holder-' + episodeNumber).css('padding', '0 10px');
    $('.jp-progress-' + episodeNumber).css('display', 'block');
    $('.jp-current-time-' + episodeNumber + ', .jp-duration-' + episodeNumber).css('display', 'inline');
    $('.pc-episode-duration-' + episodeNumber).css('display', 'none');
  }

  /* Hide audio player elements */
  var hideAudioPlayerElements = function(episodeNumber: number) {
    $('.jp-audio-' + episodeNumber).css('width', '46px');
    $('.jp-interface-' + episodeNumber).css('width', '100%');
    $('.jp-controls-holder-' + episodeNumber).css('padding', '0');
    $('.jp-progress-' + episodeNumber).css('display', 'none');
    $('.jp-current-time-' + episodeNumber + ', .jp-duration-' + episodeNumber).css('display', 'none');
    $('.pc-episode-duration-' + episodeNumber).css('display', 'inline');
  }

  /* Drag handler */
  var timeDrag = false; // Drag status
  var episodeNumber = 0;
  ($('.pc-drag-handler') as any).draggable({
    axis: 'x',
    containment: 'parent',
    start: function (e: any) {
      episodeNumber = $(this).data('episode-number');
      ($('#jquery_jplayer_' + episodeNumber) as any).jPlayer('pause');
      timeDrag = true;
      $('.pc-drag-handler-' + episodeNumber).addClass('dragging');
      updateBar(e.pageX);
    },
    drag: function (e: any) {
      if (timeDrag) {
        updateBar(e.pageX);
      }
    },
    stop: function (e: any) {
      if (timeDrag) {
        timeDrag = false;
        $('.pc-drag-handler-' + episodeNumber).removeClass('dragging');
        updateBar(e.pageX);
        ($('#jquery_jplayer_' + episodeNumber) as any).jPlayer('play');
      }
    }
  });

  /* Update Progress Bar control */
  var updateBar = function (x: any) {
    var progress = $('.jp-progress-' + episodeNumber);
    var maxduration = $('#jquery_jplayer_' + episodeNumber).data('jPlayer').status.duration; // audio duration
    var position = x - progress.offset().left; //Click pos
    var percentage = 100 * position / progress.width();

    // Check within range
    if (percentage > 100) {
      percentage = 100;
    }
    if (percentage < 0) {
      percentage = 0;
    }

    ($('#jquery_jplayer_' + episodeNumber) as any).jPlayer('playHead', percentage);

    // Update progress bar and audio currenttime
    $('.pc-drag-handler-' + episodeNumber).css('left', percentage + '%');
    $('.jp-play-bar-' + episodeNumber).css('width', percentage + '%');
    ($('#jquery_jplayer_' + episodeNumber) as any).jPlayer.currentTime = maxduration * percentage / 100;
  };
});